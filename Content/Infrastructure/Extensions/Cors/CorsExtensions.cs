using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.Extensions.Cors;

public static class CorsExtensions
{
    private const string CorsPolicy = nameof(CorsPolicy);

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration config)
    {
        try
        {
            services.Configure<CorsSettings>(config.GetSection(nameof(CorsSettings)));
            var corsSettings = config.GetSection(nameof(CorsSettings)).Get<CorsSettings>();
            var origins = new List<string>();
            if (corsSettings.Angular is not null)
                origins.AddRange(corsSettings.Angular.Split(';', StringSplitOptions.RemoveEmptyEntries));

            return services.AddCors(opt =>
                opt.AddPolicy(CorsPolicy, policy =>
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(origins.ToArray())));
        }
        catch (Exception e)
        {
            Log.Error($"Error to configure cors {e.Message}, {e}");
        }

        return services;
    }

    public static void UseCorsPolicy(this IApplicationBuilder app)
    {
        app.UseCors(CorsPolicy);
    }
}