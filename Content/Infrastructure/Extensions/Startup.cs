using Infrastructure.Context;
using Infrastructure.Extensions.Cors;
using Infrastructure.Extensions.Logs;
using Infrastructure.Extensions.Mapper;
using Infrastructure.Extensions.Mediator;
using Infrastructure.Extensions.OpenApi;
using Infrastructure.Extensions.Persistence;
using Infrastructure.Extensions.Service;
using Infrastructure.Extensions.Validation;
using Infrastructure.Inicialize;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class Startup
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
    {
        services
            .AddGraphQL()
            .AddOpenApiDocumentation(env)
            .AddValidation()
            .AddMediator()
            .AddMapper()
            .AddPersistence(config)
            .AddCorsPolicy(config)
            .AddLogger()
            .AddRepositories(config)
            .AddDomainServices();
    }

    public static void UseInfrastructure(this IApplicationBuilder builder)
    {
        builder
            .UseOpenApiDocumentation()
            .UseCorsPolicy();
    }

    public static async Task InitializeDatabasesAsync(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        var contex = scope!.ServiceProvider.GetRequiredService<PersistenceContext>();
        var start = new Start(contex);
        try
        {
            await start.InitializeDatabasesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}