using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.Extensions.Logs;

public static class LoggerExtensions {
    public static IServiceCollection AddLogger(this IServiceCollection svc) {
        Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
        svc.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        return svc;
    }
}