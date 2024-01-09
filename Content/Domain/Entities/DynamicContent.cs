
using Domain.Enums;

namespace Domain.Entities;

public class DynamicContent
{
    public ContentType ContentType { get; set; }
    public object Data { get; set; }

    public DynamicContent(ContentType contentType, object data)
    {
        ContentType = contentType;
        Data = data;
    }

    public DynamicContent() { }
}