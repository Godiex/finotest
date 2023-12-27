using Domain.Entities;
using Domain.Ports;
using Domain.Tests.BuilderEntities;

namespace Api.Tests.Seeders;

public class CommercialSegmentSeeder : IDataSeeder
{
    private readonly IGenericRepository<CommercialSegment> _commercialSegmentRespository;

    public CommercialSegmentSeeder(IGenericRepository<CommercialSegment> commercialSegmentRespository)
    {
        _commercialSegmentRespository = commercialSegmentRespository;
    }
    
    public void Seed()
    {
        var commercialSegment = new CommercialSegmentBuilder().Build();
        _commercialSegmentRespository.AddAsync(commercialSegment).Wait();
    }
}