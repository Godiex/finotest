using Domain.Entities;

namespace Domain.Ports;

public interface IMessageConsumer<T>
{
    event Func<MessageEnvelope<T>, CancellationToken, Task> ProcessMessageAsync;

    Task StartListeningAsync(CancellationToken cancellationToken = default (CancellationToken));

    Task StopListeningAsync(CancellationToken cancellationToken = default (CancellationToken));
}