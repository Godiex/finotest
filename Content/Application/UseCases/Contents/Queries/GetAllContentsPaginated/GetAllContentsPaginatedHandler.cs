using Application.Common.Helpers.Pagination;
using Application.Ports;
using Domain.Entities;

namespace Application.UseCases.Contents.Queries.GetAllContentsPaginated;

public class GetAllContentsPaginatedHandler : IRequestHandler<GetAllContentsPaginatedQuery, PaginationResponse<GetAllContentsPaginatedDto>>
{
    private readonly IReadRepository<Content> _repository;

    public GetAllContentsPaginatedHandler(IReadRepository<Content> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<PaginationResponse<GetAllContentsPaginatedDto>> Handle(GetAllContentsPaginatedQuery query, CancellationToken cancellationToken)
    {
        var spec = new GetAllContentsPaginatedSpec(query.Filter);
        return await _repository.PaginatedListAsync(spec, query.PageNumber, query.PageSize, cancellationToken);
    }
}