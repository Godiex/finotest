using Domain.Entities;

namespace Domain.Tests.BuilderEntities;

public class ContentBuilder
{
    private Guid _id = Guid.NewGuid();
    private  string _tag = "tag";
    private string? _logoUrl = null;
    private List<string>? _multimedia = null;
    private List<string>? _languajes = null;
    private List<Item>? _items = null;

    public ContentBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }
    public ContentBuilder WithTag(string tag)
    {
        _tag = tag;
        return this;
    }
    
    public Content Build()
    {
        return new Content(_id,_tag,_logoUrl,_multimedia,_languajes,_items);
    }
}