using System.Text.Json;
using Application.Ports.Messaging;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Infrastructure.Extensions.Message;

namespace Infrastructure.Adapters.Messaging;

public class ServiceBusMessagePublisher : IMessagePublisher
{
    private readonly ServiceBusSender _sender;
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusAdministrationClient _serviceBusAdministrationClient;
    private string TopicOrQueue { get; set; }
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
      PropertyNameCaseInsensitive = true,
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ServiceBusMessagePublisher(
      ServiceBusClient serviceBusClient,
      ServiceBusAdministrationClient serviceBusAdministrationClient,
      string topicOrQueue
    )
    {
      ArgumentNullException.ThrowIfNull((object) nameof (serviceBusClient), "nameof(serviceBusClient)");
      TopicOrQueue = topicOrQueue;
      _serviceBusClient = serviceBusClient;
      _serviceBusAdministrationClient = serviceBusAdministrationClient;
      _sender = !string.IsNullOrEmpty(topicOrQueue) ? serviceBusClient.CreateSender(topicOrQueue) : throw new ArgumentException("'topicOrQueue' cannot be null or empty.", nameof (topicOrQueue));
    }

    public async Task<string> SendMessageAsync<T>(
      MessageEnvelope<T> message,
      JsonSerializerOptions options,
      CancellationToken cancellationToken = default)
    {
      ServiceBusMessage serviceBusMessage = message.ToServiceBusMessage(options);
      await _sender.SendMessageAsync((message != null ? serviceBusMessage : (ServiceBusMessage) null) ?? throw new ArgumentNullException(nameof (message)), cancellationToken);
      return serviceBusMessage.MessageId;
    }

    public async Task CreateQueue(string queue, CancellationToken cancellationToken = default(CancellationToken))
    {
      if (!await _serviceBusAdministrationClient.QueueExistsAsync(queue, cancellationToken))
      {
        await _serviceBusAdministrationClient.CreateQueueAsync(queue, cancellationToken);
      }
    }

    public async Task<string> SendMessageAsync<T>(
      MessageEnvelope<T> message,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return await SendMessageAsync<T>(message, JsonOptions, cancellationToken);
    }

    public async Task<string> SendMessagesAsync<T>(
      IEnumerable<MessageEnvelope<T>> messages,
      JsonSerializerOptions options,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      ArgumentNullException.ThrowIfNull((object) nameof (messages), "nameof(messages)");
      ServiceBusMessageBatch messageBatch;
      if (!messages.Any<MessageEnvelope<T>>())
      {
        messageBatch = (ServiceBusMessageBatch) null;
      }
      else
      {
        messageBatch = await _sender.CreateMessageBatchAsync(cancellationToken);
        try
        {
          foreach (MessageEnvelope<T> message in messages)
          {
            ServiceBusMessage serviceBusMessage = message.ToServiceBusMessage<T>(options);
            serviceBusMessage = messageBatch.TryAddMessage(serviceBusMessage) ? (ServiceBusMessage) null : throw new Exception("The message is too large to fit in the batch.");
          }
          await _sender.SendMessagesAsync(messageBatch, cancellationToken);
          messageBatch = (ServiceBusMessageBatch) null;
        }
        finally
        {
          messageBatch?.Dispose();
        }
      }
      return messages.ElementAt(0).MessageId!;
    }

    public async Task<string> SendMessagesAsync<T>(
      IEnumerable<MessageEnvelope<T>> messages,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return await SendMessagesAsync<T>(messages, JsonOptions, cancellationToken);
    }
}
  