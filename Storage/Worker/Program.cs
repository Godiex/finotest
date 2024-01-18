using Infrastructure;
using Infrastructure.Common;
using Infrastructure.Extensions.Logs;
using Infrastructure.Extensions.Mapper;
using Infrastructure.Extensions.Mediator;
using Infrastructure.Extensions.Message;
using Infrastructure.Extensions.Service;
using Infrastructure.Extensions.Storage;
using Workerw;

StaticLogger.EnsureInitialized();
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;
if (builder.Environment.IsEnvironment(ApiConstants.LocalEnviroment))
{
    config.AddUserSecrets<Program>();
}
else
{
    config.AddEnvironmentVariables();
}

builder.Services.AddHealthChecks();
builder.Services.AddStorage(config);
builder.Services.AddMediator();
builder.Services.AddMapper();
builder.Services.AddDomainServices();
builder.Services.AddMessaging(config);

builder.Services.AddHostedService<WorkerTest>();

WebApplication app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health");
});
app.Run();