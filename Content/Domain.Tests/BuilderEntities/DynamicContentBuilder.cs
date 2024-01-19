using Domain.Entities;
using Domain.Enums;

namespace Domain.Tests.BuilderEntities;

public class DynamicContentBuilder
{
    private ContentType _contentType = ContentType.ACCORDION_DETAIL;
    private object _data = new object();
    
    public DynamicContentBuilder WithContentType(ContentType contentType)
    {
        _contentType = contentType;
        return this;
    }
    public DynamicContentBuilder WithContentData(object data)
    {
        _data = data;
        return this;
    }

    public DynamicContent Build()
    {
        return new DynamicContent(_contentType, _data);
    }
}