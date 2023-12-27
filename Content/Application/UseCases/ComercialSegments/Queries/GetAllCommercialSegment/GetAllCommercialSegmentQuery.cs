namespace Application.UseCases.ComercialSegments.Queries.GetAllCommercialSegment;

public record GetAllCommercialSegmentQuery : IRequest<IEnumerable<GetAllCommercialSegmentDto>>;