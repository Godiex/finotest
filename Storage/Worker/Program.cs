using Infrastructure;
using Infrastructure.Extensions.Logs;
using Infrastructure.Extensions.Mapper;
using Infrastructure.Extensions.Mediator;
using Infrastructure.Extensions.Message;
using Infrastructure.Extensions.Service;
using Infrastructure.Extensions.Storage;
using Workerw;

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

builder.Services.AddHealthChecks();
builder.Services.AddStorage(config);
builder.Services.AddMediator();
builder.Services.AddMapper();
builder.Services.AddLogger();
builder.Services.AddDomainServices();
builder.Services.AddMessaging(config);

builder.Services.AddHostedService<WorkerTest>();

var app = builder.Build();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health");
});
app.Run();