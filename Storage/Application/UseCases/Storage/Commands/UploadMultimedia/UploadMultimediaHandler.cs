using Application.Common.Dto;
using Domain;
using Domain.Entities;
using Domain.Exceptions.Common;
using Domain.Ports;
using Domain.Services;

namespace Application.UseCases.Storage.Commands.UploadMultimedia
{
    public class UploadMultimediaHandler : IRequestHandler<UploadMultimediaCommand, UploadMultimediaDto>
    {
        private readonly IMultimediaMessagePublisher _messagePublisher;
        private readonly FileStorageService _fileStorageService;
        private const long MaxFileSize = 20 * 1024 * 1024;

        public UploadMultimediaHandler(FileStorageService fileStorageService, IMultimediaMessagePublisher messagePublisher)
        {
            _fileStorageService = fileStorageService ?? throw new ArgumentNullException(nameof(fileStorageService));
            _messagePublisher = messagePublisher;
        }

        public async Task<UploadMultimediaDto> Handle(UploadMultimediaCommand request, CancellationToken cancellationToken)
        {
            if (request.FilesMetadata.Files.Any(file => file.Size > MaxFileSize))
            {
                var message = string.Format(Messages.InvalidFileSizeException, MaxFileSize / (1024 * 1024));
                throw new InvalidFileSizeException(message);
            }
            
            string[] multimediaUrls = await Task.WhenAll(request.FilesMetadata.Files.Select(file =>
                _fileStorageService.UploadAsync(file.Data, file.Extension, file.ContentType)));
            var messagePublisher = new MessageResponsePublisherDto<string[]>(multimediaUrls);
            await _messagePublisher.CreateQueueSender(request.FilesMetadata.Subscription);
            var messageToPublish = new MessageEnvelope<MessageResponsePublisherDto<string[]>>(messagePublisher);
            messageToPublish.SetSubscription(request.FilesMetadata.Subscription);
            await _messagePublisher.SendMessageAsync(messageToPublish, cancellationToken);
            return new UploadMultimediaDto(multimediaUrls.ToArray());
        }
    }
}