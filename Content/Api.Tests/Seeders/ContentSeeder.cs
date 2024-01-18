using Domain.Entities;
using Domain.Ports;
using Domain.Tests.BuilderEntities;

namespace Api.Tests.Seeders;

public class ContentSeeder: IDataSeeder
{
    private readonly IGenericRepository<Content> _contentRepository;
    public ContentSeeder(IGenericRepository<Content> contentRepository)
    {
        _contentRepository = contentRepository;
    }
    public void Seed()
    {
        var commercialSegment = new ContentBuilder().WithTag("old tag").Build();
        _contentRepository.AddAsync(commercialSegment).Wait();
    }
}