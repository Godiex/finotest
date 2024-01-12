using Domain.Enums;

namespace Application.UseCases.Contents.Queries.GetAllContentsPaginated;

public record GetAllContentsPaginatedDto(
    Guid Id,
    string Tag,
    string Logo,
    List<string> Carousel,
    List<string> Languages,
    List<ContentLanguageDto> Contents,
    StylesDto Styles
);

public record ContentLanguageDto(int LanguageIndex, string? Title, List<AccordionDetailDto> Details);

public record AccordionDetailDto(int Index, BehaviorType Behavior, string? Label, string Data);

public record StylesDto(string? BackgroundColor, string? Color);