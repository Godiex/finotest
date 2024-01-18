using System.Text.Json;
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
            .Map(dest => dest.Languages, src => src.Languages)
            .Map(dest => dest.Contents, src =>  MapContentLanguageDto(src.Items));
    }

    private static List<ContentLanguageDto> MapContentLanguageDto(List<Item>? items)
    {
        if (items != null)
        {
            return items.ConvertAll(i => new ContentLanguageDto(i.LanguageIndex, i.Title, i.Contents.ConvertAll(c =>
            {
                if (c.Data is JsonElement jsonElement)
                {
                    return JsonSerializer.Deserialize<AccordionDetailDto>(jsonElement.GetRawText());
                }

                return null;
            })));
        }

        return new List<ContentLanguageDto>();
    }
}