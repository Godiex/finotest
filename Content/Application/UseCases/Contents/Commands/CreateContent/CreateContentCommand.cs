using System.Text.Json.Serialization;
using Domain.Enums;

namespace Application.UseCases.Contents.Commands.CreateContent;

public record CreateContentCommand : IRequest<Unit>
{
    public CreateContentCommand(Guid id, string tag, string? logoUrl, List<string>? multimedia, List<TextCommand>? titleContent, StylesCommand? styles, List<string>? languages, List<ItemCommand>? items)
    {
        Id = id;
        Tag = tag;
        LogoUrl = logoUrl;
        Multimedia = multimedia;
        TitleContent = titleContent;
        Styles = styles;
        Languages = languages;
        Items = items;
    }

    [JsonIgnore]
    public Guid Id { get; set; }
    public string Tag { get; set; }
    public string? LogoUrl { get; set; }
    public List<string>? Multimedia { get; set; }
    public List<TextCommand>? TitleContent { get; set; }
    public StylesCommand? Styles { get; set; }
    public List<string>? Languages { get; set; }
    public List<ItemCommand>? Items { get; set; }
}

public record SectionContentCommand(string Code, string Label, string Content);

public record TextCommand(string Code, string Content);

public record ItemCommand(int Index, BehaviorType Behavior, List<SectionContentCommand> Contents);

public record StylesCommand(string BackgroundColor, string Color);

