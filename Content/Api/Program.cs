using Api.Filters;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Extensions.Localization;
using Infrastructure.Extensions.Persistence;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
if (builder.Environment.IsEnvironment(ApiConstants.LocalEnviroment))
{
    config.AddUserSecrets<Program>();
}
else
{
    config.AddEnvironmentVariables();
}
builder.Services.AddLocalizationMessages();
builder.Services.Configure<DatabaseSettings>(config.GetSection(nameof(DatabaseSettings)));
var settings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
builder.Services.AddHealthChecks().AddSqlServer(settings.ConnectionString);
builder.Services.AddControllers(opts =>
{
    opts.Filters.Add(typeof(AppExceptionFilterAttribute));
    opts.Filters.Add(typeof(GlobalValidateModelAttribute));
});
builder.Services.AddInfrastructure(config, builder.Environment);

builder.Services.AddEndpointsApiExplorer();

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();
await app.InitializeDatabasesAsync();
app.UseInfrastructure();
app.UseLocalizationMessages();
app.UseRouting().UseHttpMetrics().UseEndpoints(endpoints =>
{
    endpoints.MapMetrics();
    endpoints.MapHealthChecks("/health");
    endpoints.MapGraphQL();
});
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }