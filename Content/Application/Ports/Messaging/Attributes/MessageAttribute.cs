namespace Domain.Entities.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MessageAttribute : Attribute
{
    public string Subject { get; }

    public MessageAttribute(string subject) => this.Subject = subject;
}