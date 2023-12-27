using Domain.Entities;

namespace Domain.Tests.BuilderEntities;

public class CommercialSegmentBuilder
{
    private readonly Guid _id = Guid.NewGuid();
    private string _name = "Colchones";
    private string _description = "Segmento ventas colchones";

    public CommercialSegmentBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public CommercialSegmentBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public CommercialSegment Build()
    {
        return new CommercialSegment(_id, _name, _description);
    }
}