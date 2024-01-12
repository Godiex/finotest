using System.Text.Json;
using Application.Ports.Messaging;
using Azure.Messaging.ServiceBus;
using Domain.Entities;
using Infrastructure.Adapters.Messaging;

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
        return new ServiceBusMessage(body)
        {
            ContentType = message.ContentType,
            MessageId = Guid.NewGuid().ToString(),
            SessionId = message.SessionId,
            CorrelationId = message.CorrelationId,
            Subject = message.Subject
        };
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