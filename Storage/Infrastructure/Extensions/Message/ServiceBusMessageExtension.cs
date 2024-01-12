using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Domain.Entities;

namespace Infrastructure.Extensions.Message;

internal static class ServiceBusMessageExtension
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static ServiceBusMessage ToServiceBusMessage<T>(
        this MessageEnvelope<T> message,
        JsonSerializerOptions options)
    {
        BinaryData body = message.Payload as BinaryData ?? BinaryData.FromObjectAsJson<T>(message.Payload, options);
        var serviceBusMessage = new ServiceBusMessage(body)
        {
            ContentType = message.ContentType,
            MessageId = message.MessageId ?? IdGenerator.FromBytes(body.ToArray()).ToString(),
            SessionId = message.SessionId,
            CorrelationId = message.CorrelationId,
            Subject = message.Subject,
        };
        if (!string.IsNullOrEmpty(message.To))
        {
            serviceBusMessage.To = message.To;
        }

        return serviceBusMessage;
    }

    public static ServiceBusMessage ToServiceBusMessage<T>(this MessageEnvelope<T> message)
    {
        return message.ToServiceBusMessage<T>(JsonOptions);
    }

    public static MessageEnvelope<T> ToMessageEnvelope<T>(
        this ServiceBusReceivedMessage message,
        JsonSerializerOptions options) where T : class
    {
        BinaryData body = message.Body;
        return new MessageEnvelope<T>((body is not T ? message.Body.ToObjectFromJson<T>(options) : body as T)!, message.Subject)
        {
            ContentType = message.ContentType,
            MessageId = message.MessageId,
            SessionId = message.SessionId,
            CorrelationId = message.CorrelationId
        };
    }

    public static MessageEnvelope<T> ToMessageEnvelope<T>(this ServiceBusReceivedMessage message) where T : class
    {
        return message.ToMessageEnvelope<T>(JsonOptions);
    }
}