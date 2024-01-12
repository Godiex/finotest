using System.Text.Json;
using Application.UseCases.Storage.Commands.UploadMultimedia;
using Domain.Entities;
using Domain.Ports;
using MediatR;

namespace Workerw;
public sealed class WorkerTest: BackgroundService
{
    private const string ReadMessageError = "Cannot read message {message} with subject {subject}";
    private readonly ILogger<WorkerTest> _logger;
    private readonly IMultimediaMessageConsumer _consumer;
    private readonly IServiceScopeFactory _scopeFactory;
    public WorkerTest(ILogger<WorkerTest> logger, IMultimediaMessageConsumer consumer, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _consumer = consumer;
        _scopeFactory = scopeFactory;
        _consumer.ProcessMessageAsync += ProcessMessageAsync;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.StartListeningAsync(stoppingToken);
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _consumer.StopListeningAsync(cancellationToken);
    }
    
    private async Task ProcessMessageAsync(MessageEnvelope<BinaryData> message, CancellationToken cancellationToken)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        IEnumerable<FileDataDto>? data = JsonSerializer.Deserialize<IEnumerable<FileDataDto>>(message.Payload, options);
        if (data == null)
        {
            _logger.LogError(ReadMessageError,
                message.Payload.ToString(),
                message.Subject);
        }
        else
        {
            var command = new UploadMultimediaCommand(new DataMessageDto(message.MessageId!, data));
            await SendToMediator(command, cancellationToken);
        }
    }
    
    private async Task SendToMediator(UploadMultimediaCommand command, CancellationToken cancellationToken)
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        IMediator mediador = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediador.Send(command, cancellationToken);
    }

}