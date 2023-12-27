using Application.Ports;
using Domain.Entities;

namespace Application.UseCases.ComercialSegments.Queries.GetAllCommercialSegment;

public class GetAllComercialSegmentsHandler : IRequestHandler<GetAllCommercialSegmentQuery, IEnumerable<GetAllCommercialSegmentDto>>
{
    private readonly IReadRepository<CommercialSegment> _repository;

    public GetAllComercialSegmentsHandler(IReadRepository<CommercialSegment> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<GetAllCommercialSegmentDto>> Handle(GetAllCommercialSegmentQuery query, CancellationToken cancellationToken)
    {
        var spec = new GetAllCommercialSegmentsSpec();
        return await _repository.ListAsync(spec, cancellationToken);
    }
}