using System.Text.Json;
using Domain.Entities;

namespace Domain.Ports;

public interface IMessagePublisher
{
    Task CreateQueueSender(string queue);

    Task SendMessageAsync<T>(MessageEnvelope<T> message, CancellationToken cancellationToken = default (CancellationToken));

    Task SendMessageAsync<T>(
        MessageEnvelope<T> message,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default (CancellationToken));

    Task SendMessagesAsync<T>(
        IEnumerable<MessageEnvelope<T>> messages,
        CancellationToken cancellationToken = default (CancellationToken));

    Task SendMessagesAsync<T>(
        IEnumerable<MessageEnvelope<T>> messages,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default (CancellationToken));
}
