namespace Application.Common.Dto;

public class MessageResponsePublisherDto<T>
{
    public T Data { get; set; }
    public bool Error { get; set; }
    public string Message { get; set; }

    public MessageResponsePublisherDto()
    {
        
    }

    public MessageResponsePublisherDto(T data, bool error = false, string message = "")
    {
        Data = data;
        Error = error;
        Message = message;
    }
};