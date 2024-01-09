using Application.Common.Helpers.Pagination;

namespace Application.UseCases.Contents.Queries.GetAllContentsPaginated;

public record GetAllContentsPaginatedQuery: PaginationRequest, IRequest<PaginationResponse<GetAllContentsPaginatedDto>>
{
    public string Filter { get; private set; }
    
    public GetAllContentsPaginatedQuery(string filter, int pageNumber, int pageSize) : base(pageNumber, pageSize)
    {
        Filter = filter;
    }
}