using Domain.Entities.Attributes;

namespace Domain.Entities;

public class MessageEnvelope<T>
{
    public MessageEnvelope(T payload)
        : this(payload, (string) null)
    {
    }

    public MessageEnvelope(T payload, string? subject)
    {
        Payload = payload;
        Subject = string.IsNullOrWhiteSpace(subject) ? GetSubject() : subject;
    }

    public virtual string? ContentType { get; init; } = "application/json";

    public virtual string? MessageId { get; init; }

    public virtual string? SessionId { get; init; }

    public virtual string? CorrelationId { get; init; }

    public virtual string Subject { get; }

    public virtual T Payload { get; }

    public virtual string To { get; private set; }

    public void SetSubscription(string subscription)
    {
        To = subscription;
    }

    private static string GetSubject()
    {
        Type element = typeof (T);
        return Attribute.GetCustomAttribute(element, typeof (MessageAttribute)) is MessageAttribute customAttribute ? customAttribute.Subject : element.FullName ?? element.Name;
    }
}
