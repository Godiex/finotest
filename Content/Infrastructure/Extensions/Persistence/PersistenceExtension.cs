using System.Data;
using Application.Ports;
using Domain.Ports;
using Infrastructure.Adapters.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.Persistence;

public static class PersistenceExtensions {
    public static IServiceCollection AddRepositories(this IServiceCollection svc, IConfiguration config) {
        svc.Configure<DatabaseSettings>(config.GetSection(nameof(DatabaseSettings)));
        var settings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        svc.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        svc.AddTransient(typeof(IIdempotencyRepository<,>), typeof(IdempotencyRepository<,>));
        svc.AddTransient(typeof(IReadRepository<>), typeof(GenericReadRepository<>));
        svc.AddScoped<IDbConnection>((sp) => new SqlConnection(settings.ConnectionString));
        return svc;
    }
}