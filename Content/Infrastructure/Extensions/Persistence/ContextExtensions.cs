using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions.Persistence;

public static class ContextExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection svc, IConfiguration config) {
        DatabaseSettings? settings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        svc.AddDbContext<PersistenceContext>(opt =>
        {
            opt.UseSqlServer(settings?.ConnectionString ?? "DefaultText", sqlopts =>
            {
                sqlopts.MigrationsHistoryTable(settings?.MigrationsHistoryTable  ?? "DefaultText", settings?.SchemaName ?? "DefaultText");
            });
        });
        return svc;
    }
}