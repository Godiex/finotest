using System.Text.Json;

namespace Application.Ports.Messaging;

public interface IMessagePublisher
{
    Task CreateQueue(string queue, CancellationToken cancellationToken = default (CancellationToken));
    Task<string> SendMessageAsync<T>(MessageEnvelope<T> message, CancellationToken cancellationToken = default (CancellationToken));

    Task <string> SendMessageAsync<T>(
        MessageEnvelope<T> message,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default (CancellationToken));

    Task<string> SendMessagesAsync<T>(
        IEnumerable<MessageEnvelope<T>> messages,
        CancellationToken cancellationToken = default (CancellationToken));

    Task<string> SendMessagesAsync<T>(
        IEnumerable<MessageEnvelope<T>> messages,
        JsonSerializerOptions options,
        CancellationToken cancellationToken = default (CancellationToken));
}
