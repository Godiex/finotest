using Domain.Entities;

namespace Domain.Tests.BuilderEntities;

public class ContentBuilder
{
    private  string _tag = "tag";
    private string? _logoUrl = null;
    private List<string>? _multimedia = null;
    private List<string>? _languajes = null;
    private List<Item>? _items = null;

    
    public ContentBuilder WithTag(string tag)
    {
        _tag = tag;
        return this;
    }
    
    public Content Build()
    {
        return new Content(_tag,_logoUrl,_multimedia,_languajes,_items);
    }
}