using System.Text.Json;
using Application.Common.Dto;
using Application.Common.Exceptions;
using Application.Ports.Messaging;
using Domain.Entities;
using Domain.Entities.Idempotency;
using Domain.Ports;
using Domain.Services;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Contents.Commands.CreateContent;
public class CreateContentHandler : IRequestHandler<CreateContentCommand>
{
    private readonly ContentService _contentService;
    private readonly IMultimediaMessagePublisher _multimediaMessagePublisher;
    private readonly IMultimediaMessageConsumer _messageConsumer;
    private readonly IIdempotencyRepository<ContentId, Guid> _contentIdIdRepository;
    private string[]? CarouselUrl { get; set; }
    private string? LogoUrl { get; set; }
    private TaskCompletionSource<bool> _completionSource = new();
        
    
    public CreateContentHandler(
        ContentService service,
        IIdempotencyRepository<ContentId, Guid> contentIdIdReponsitory,
        IMultimediaMessagePublisher multimediaMessagePublisher,
        IMultimediaMessageConsumer messageConsumer
    )
    {
        _contentService = service ?? throw new ArgumentNullException(nameof(service));
        _contentIdIdRepository = contentIdIdReponsitory ?? throw new ArgumentNullException(nameof(contentIdIdReponsitory));
        _multimediaMessagePublisher = multimediaMessagePublisher ?? throw new ArgumentNullException(nameof(multimediaMessagePublisher));
        _messageConsumer = messageConsumer;
    }

    public async Task<Unit> Handle(CreateContentCommand request, CancellationToken cancellationToken)
    {
        await _contentIdIdRepository.ValidateEntityId(request.Id);
        await _contentIdIdRepository.RemoveAsync(request.Id);
        if (request.Carousel != null)
        {
            await ProcessMultimediaAsync(request.Carousel, ProcessMessageCarouselAsync, cancellationToken);
        }
        
        if (request.Logo != null)
        {
            await ProcessMultimediaAsync(new []{request.Logo}, ProcessMessageLogoAsync, cancellationToken);
        }
        
        Content content = CreateContentCommand.MapCommandToEntity(request, LogoUrl, CarouselUrl);
        await _contentService.CreateAsync(content);
        return new Unit();
    }

    private async Task ProcessMultimediaAsync(IEnumerable<IFormFile> files, Func<MessageEnvelope<BinaryData>, CancellationToken, Task> processMessageFunc, CancellationToken cancellationToken)
    {
        _completionSource = new TaskCompletionSource<bool>();
        var messageId = await SendMessageMultimedia(files, cancellationToken);
        await _multimediaMessagePublisher.CreateQueue(messageId, cancellationToken);
        _messageConsumer.SetQueue(messageId);
        _messageConsumer.ProcessMessageAsync += processMessageFunc;
        await _messageConsumer.StartListeningAsync(cancellationToken);
        await _completionSource.Task;
        await _messageConsumer.StopListeningAsync(cancellationToken);
    }

    private async Task<string> SendMessageMultimedia(IEnumerable<IFormFile> files, CancellationToken cancellationToken)
    {
        IEnumerable<FileDataDto> dataMessage = GetDataMessageDto(files);
        MessageEnvelope<IEnumerable<FileDataDto>> message = new MessageEnvelope<IEnumerable<FileDataDto>>(dataMessage);
        string messageCarouselId = await _multimediaMessagePublisher.SendMessageAsync(message, cancellationToken);
        return messageCarouselId;
    }

    private async Task ProcessMessageCarouselAsync(MessageEnvelope<BinaryData> message, CancellationToken cancellationToken)
    {
        await ProcessMessageAsync(message, urls => CarouselUrl = urls, cancellationToken);
    }

    private async Task ProcessMessageLogoAsync(MessageEnvelope<BinaryData> message, CancellationToken cancellationToken)
    {
        await ProcessMessageAsync(message, urls => LogoUrl = urls.ElementAt(0), cancellationToken);
    }
    
    private Task ProcessMessageAsync(MessageEnvelope<BinaryData> message, Action<string[]> dataProcessor, CancellationToken cancellationToken)
    {
        MessageResponsePublisherDto<string[]> response = GetData(message);
        dataProcessor(response.Data);
        _completionSource.SetResult(true);
        return Task.CompletedTask;
    }

    private static MessageResponsePublisherDto<string[]> GetData(MessageEnvelope<BinaryData> message)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        MessageResponsePublisherDto<string[]>? data = JsonSerializer.Deserialize<MessageResponsePublisherDto<string[]>>(message.Payload, options);
        
        if (data.Error)
        {
            throw new BadRequestException(data.Message);
        }

        return data;
    }

    private IEnumerable<FileDataDto> GetDataMessageDto(IEnumerable<IFormFile> files) =>
        files.Select(ConvertToFileDataDto).ToArray();

    private static FileDataDto ConvertToFileDataDto(IFormFile formFile)
    {
        using MemoryStream memoryStream = new MemoryStream();
        formFile.CopyTo(memoryStream);
        string extension = Path.GetExtension(formFile.FileName);
        byte[] data = memoryStream.ToArray();
        return new FileDataDto(extension, data, formFile.Length);
    }
}