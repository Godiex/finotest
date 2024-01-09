namespace Domain.Entities;

public class Content : EntityBase<Guid>, IAggregateRoot
{
    public string Tag { get; set; }
    public string? LogoUrl { get; set; }
    public List<string>? Multimedia { get; set; }
    public List<string>? Languages { get; set; }
    public DynamicContent? TitleContent { get; set; }
    public List<Item>? Items { get; set; }

    public Content() { }

    public Content(
        string tag,
        string? logoUrl = null,
        List<string>? multimedia = null,
        List<string>? languages = null,
        DynamicContent? titleContent = null,
        List<Item>? items = null)
    {
        Tag = tag;
        LogoUrl = logoUrl;
        Multimedia = multimedia;
        Languages = languages;
        TitleContent = titleContent;
        Items = items;
    }
}