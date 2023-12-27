using System.Runtime.Serialization;

namespace Domain.Exceptions;

public class InvalidHostNameException : CoreBusinessException
{
    public InvalidHostNameException()
    {
    }

    public InvalidHostNameException(string msg) : base(msg)
    {
    }

    public InvalidHostNameException(string message, Exception inner) : base(message, inner)
    {
    }

    private  InvalidHostNameException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}