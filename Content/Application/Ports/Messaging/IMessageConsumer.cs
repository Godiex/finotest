namespace Application.Ports.Messaging;

public interface IMessageConsumer<T>
{
    void SetQueue(string queue);
    
    event Func<MessageEnvelope<T>, CancellationToken, Task> ProcessMessageAsync;

    Task StartListeningAsync(CancellationToken cancellationToken = default (CancellationToken));

    Task StopListeningAsync(CancellationToken cancellationToken = default (CancellationToken));
}