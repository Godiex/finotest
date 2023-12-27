using System.Runtime.Serialization;

namespace Domain.Exceptions;


[Serializable]
public sealed class InvalidEmailException : CoreBusinessException
{
    public InvalidEmailException()
    {
    }

    public InvalidEmailException(string msg) : base(msg)
    {
    }

    public InvalidEmailException(string message, Exception inner) : base(message, inner)
    {
    }

    private  InvalidEmailException(SerializationInfo info, StreamingContext context)
    : base(info, context)
    {
    }

}