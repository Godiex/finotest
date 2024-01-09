using System.Reflection;
using Api.Filters;
using Base.Infrastructure.Context;
using Base.Infrastructure.Extensions;
using Base.Infrastructure.Inicialize;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers(opts =>
{
    opts.Filters.Add(typeof(AppExceptionFilterAttribute));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.Load("Base.Application"), typeof(Program).Assembly);
builder.Services.AddAutoMapper(Assembly.Load("Base.Application"));

builder.Services.AddDbContext<PersistenceContext>(opt =>
{
    opt.UseSqlServer(config.GetConnectionString("database"), sqlopts =>
    {
        sqlopts.MigrationsHistoryTable("_MigrationHistory", config.GetValue<string>("SchemaName"));
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        corsPolicyBuilder =>
            corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddHealthChecks().AddSqlServer(config["ConnectionStrings:database"]);

builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

builder.Services.AddPersistence(config).AddDomainServices().AddRabbitSupport(config);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Block Base.Api", Version = "v1" });
});

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Block Base.Api"));
}

app.UseCors("CorsPolicy");

app.UseRouting().UseHttpMetrics().UseEndpoints(endpoints =>
{
    endpoints.MapGet("/ceiba/blockversion", () => new { version = 1.0, by = "Jose A. Fernandez" });
    endpoints.MapMetrics();
    endpoints.MapHealthChecks("/health");
});

using var scope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
var contex = scope!.ServiceProvider.GetRequiredService<PersistenceContext>();
var start = new Start(contex);
start.Inicializar();

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
