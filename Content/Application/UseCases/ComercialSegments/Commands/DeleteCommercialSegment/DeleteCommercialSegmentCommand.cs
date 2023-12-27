namespace Application.UseCases.ComercialSegments.Commands.DeleteCommercialSegment;

public record DeleteCommercialSegmentCommand(Guid Id) : IRequest<Unit>;
