using System.Text.Json.Serialization;

namespace Application.UseCases.ComercialSegments.Commands.CreateCommercialSegment;

public record CreateCommercialSegmentCommand : IRequest<Unit>
{
    public CreateCommercialSegmentCommand(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
