using Ardalis.Specification;
using Domain.Entities;

namespace Application.UseCases.Contents.Queries.GetAllContentsPaginated;

public class GetAllContentsPaginatedSpec : Specification<Content, GetAllContentsPaginatedDto>
{
    public GetAllContentsPaginatedSpec(string filter)
    {
        if (!string.IsNullOrEmpty(filter))
        {
            Query.Where(content => content.Tag.Contains(filter));
        }

        Query.OrderByDescending(content => content.CreatedOn);
    }
}

public class GetAll2ContentsPaginatedSpec : Specification<Content>
{
    public GetAll2ContentsPaginatedSpec(string filter)
    {
        if (!string.IsNullOrEmpty(filter))
        {
            Query.Where(content => content.Tag.Contains(filter));
        }

        Query.OrderByDescending(content => content.CreatedOn);
    }
}