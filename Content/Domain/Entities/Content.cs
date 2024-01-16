namespace Domain.Entities;

public class Content : EntityBase<Guid>, IAggregateRoot
{
    public string Tag { get; set; }
    public string? LogoUrl { get; set; }
    public List<string>? Multimedia { get; set; }
    public List<string>? Languages { get; set; }
    public List<Item>? Items { get; set; }

    public Content() { }

    public Content(
        Guid id,
        string tag,
        string? logoUrl = null,
        List<string>? multimedia = null,
        List<string>? languages = null,
        List<Item>? items = null)
    {
        Id = id;
        Tag = tag;
        LogoUrl = logoUrl;
        Multimedia = multimedia;
        Languages = languages;
        Items = items;
    }

    public void Update(
        string tag,
        string? logoUrl = null,
        List<string>? multimedia = null,
        List<string>? languages = null,
        List<Item>? items = null)
    {
        Tag = tag;
        LogoUrl = logoUrl;
        Multimedia = multimedia;
        Languages = languages;
        Items = items;
    }
}