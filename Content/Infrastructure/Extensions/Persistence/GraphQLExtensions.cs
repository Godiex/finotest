using Infrastructure.Adapters.graphql;
using Infrastructure.Adapters.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.Persistence;

public static class GraphQLExtensions
{
    public static IServiceCollection AddGraphQL(this IServiceCollection svc)
    {
        svc.AddGraphQLServer()
            .AddQueryType<CompanyQuery>()
            .AddFiltering();
        return svc;
    }
}