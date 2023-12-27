using System.Runtime.Serialization;

namespace Domain.Exceptions.Common;

public class InvalidFileSizeException : CoreBusinessException
{
    public InvalidFileSizeException()
    {
    }

    public InvalidFileSizeException(string msg) : base(msg)
    {
    }

    public InvalidFileSizeException(string message, Exception inner) : base(message, inner)
    {
    }

    private  InvalidFileSizeException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}