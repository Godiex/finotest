using Ardalis.Specification;

namespace Application.Common.Helpers.Pagination;

public static class SpecificationBuilderExtensions
{
    public static ISpecificationBuilder<T> PaginateBy<T>(this ISpecificationBuilder<T> query, int pageNumber = 1, int pageSize = 5)
    {
        query = query.Skip((pageNumber - 1) * pageSize);
        return query.Take(pageSize);
    }
}