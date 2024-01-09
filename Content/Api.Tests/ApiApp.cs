using Api.Tests.Seeders;
using Infrastructure.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Api.Tests;

public class ApiApp : WebApplicationFactory<Program>
{
    public IServiceProvider GetServiceCollection()
    {
        return Services;           
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<PersistenceContext>));
            services.AddDbContext<PersistenceContext>(opt =>
            {
                opt.UseInMemoryDatabase("testdb");
            });
        });
        SeedDatabase(builder.Build().Services);
        return base.CreateHost(builder);
    }

    private static void SeedDatabase(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var seeder in seeders)
        {
            seeder.Seed();
        }
    }
}

