using Application.UseCases.Contents.Queries.GetAllContentsPaginated;
using Domain.Entities;
using Mapster;

namespace Application;

public class MapsterSettings
{
    public static void Configure()
    {
        TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);
        TypeAdapterConfig<Content, GetAllContentsPaginatedDto>.NewConfig()
            .Map(dest => dest.Carousel, src => src.Multimedia)
            .Map(dest => dest.Logo, src => src.LogoUrl)
            .Map(dest => dest.Languages, src => src.Languages);

        TypeAdapterConfig<Item, ContentLanguageDto>.NewConfig()
            .Map(dest => dest.Details, src => src.Contents.ConvertAll(c => (AccordionDetailDto)c.Data));
    }
}