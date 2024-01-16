using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Enums;
using Microsoft.AspNetCore.Http;


namespace Application.UseCases.Contents.Commands.CreateContent;

public record CreateContentCommand : IRequest<Unit>
{
    public CreateContentCommand(
        Guid id,
        CreateContentEntryCommand commandEntry
    )
    {
        Id = id;
        Tag = commandEntry.Tag;
        Logo = commandEntry.Logo;
        Carousel = commandEntry.Carousel;
        Styles = commandEntry.Styles;
        Languages = commandEntry.Languages;
        Contents = commandEntry.Contents;
    }

    public Guid Id { get; set; }
    public string Tag { get; set; }
    public IFormFile? Logo { get; set; }
    public List<IFormFile>? Carousel { get; set; }
    public StylesCommand? Styles { get; set; }
    public List<string>? Languages { get; set; }
    public List<ContentCommand>? Contents { get; set; }
    
    public static Content MapCommandToEntity(CreateContentCommand request, string? logoUrl, string[]? carouselUrls)
    {
        return new Content(
            request.Id,
            request.Tag,
            logoUrl,
            carouselUrls?.ToList(),
            request.Languages,
            request.Contents?.ConvertAll(
                MapItem
            )
        );
    }
    
    private static Item MapItem(ContentCommand x)
    {
        return new Item(
            x.LanguageIndex, x.Title, x.Details.ConvertAll(
                section => new DynamicContent(
                    ContentType.ACCORDION_DETAIL,
                    new AccordionDetail(section.Index, section.Behavior, section.Label, section.Data))
            )
        );
    }
}

public record CreateContentEntryCommand
{
    public CreateContentEntryCommand() { }
    public CreateContentEntryCommand(
        string tag,
        IFormFile? logo,
        List<IFormFile>? carousel,
        List<ContentCommand>? contents,
        StylesCommand? styles,
        List<string>? languages
    )
    {
        Tag = tag;
        Logo = logo;
        Carousel = carousel;
        Styles = styles;
        Languages = languages;
        Contents = contents;
    }
    
    public string Tag { get; set; }
    public IFormFile? Logo { get; set; }
    public List<IFormFile>? Carousel { get; set; }
    public StylesCommand? Styles { get; set; }
    public List<string>? Languages { get; set; }
    public List<ContentCommand>? Contents { get; set; }
}

public record ContentCommand(int LanguageIndex, string? Title, List<AccordionDetailCommand> Details);

public record AccordionDetailCommand(int Index, BehaviorType Behavior, string? Label, string Data);

public record StylesCommand(string? BackgroundColor, string? Color);

