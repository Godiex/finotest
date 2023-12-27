using Infrastructure.Extensions.Cors;
using Infrastructure.Extensions.Logs;
using Infrastructure.Extensions.Mapper;
using Infrastructure.Extensions.Mediator;
using Infrastructure.Extensions.OpenApi;
using Infrastructure.Extensions.Service;
using Infrastructure.Extensions.Storage;
using Infrastructure.Extensions.Validation;
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
            .AddOpenApiDocumentation(env)
            .AddStorage(config)
            .AddValidation()
            .AddMediator()
            .AddMapper()
            .AddCorsPolicy(config)
            .AddLogger()
            .AddDomainServices();
    }

    public static void UseInfrastructure(this IApplicationBuilder builder)
    {
        builder
            .UseOpenApiDocumentation()
            .UseCorsPolicy();
    }
}