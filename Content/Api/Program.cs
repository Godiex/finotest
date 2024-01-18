using Api.Filters;
using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Extensions;
using Infrastructure.Extensions.Localization;
using Prometheus;
using Serilog;

StaticLogger.EnsureInitialized();
Log.Information("Server Booting Up...");
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;
if (builder.Environment.IsEnvironment(ApiConstants.LocalEnviroment))
{
    config.AddUserSecrets<Program>();
}
builder.Services.AddLocalizationMessages();
builder.Services.AddHealthChecks();
builder.Services.AddControllers(opts =>
{
    opts.Filters.Add(typeof(AppExceptionFilterAttribute));
    //opts.Filters.Add(typeof(GlobalValidateModelAttribute));
});
builder.Services.AddInfrastructure(config, builder.Environment);

builder.Services.AddEndpointsApiExplorer();

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

WebApplication app = builder.Build();
await app.InitializeDatabasesAsync();
app.UseInfrastructure();
app.UseLocalizationMessages();
app.UseRouting().UseHttpMetrics().UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();
    endpoints.MapHealthChecks("/health");
});
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }