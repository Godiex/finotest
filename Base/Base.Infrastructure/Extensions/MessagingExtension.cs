using Base.Infrastructure.Adapters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Base.Infrastructure.Extensions;

public static class MessagingExtension
{
    public static IServiceCollection AddRabbitSupport(this IServiceCollection services
        , IConfiguration config)
    {
        int port = Int32.Parse(Environment.GetEnvironmentVariable("RABBITPORT") ?? config.GetValue<string>("RABBITMQ:PORT"));
        var factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("RABBITHOST") ?? config.GetValue<string>("RABBITMQ:HOST"),
            UserName = Environment.GetEnvironmentVariable("RABBITUSER") ?? config.GetValue<string>("RABBITMQ:USER"),
            Password = Environment.GetEnvironmentVariable("RABBITPASS") ?? config.GetValue<string>("RABBITMQ:PASS"),
            Port = port,
            VirtualHost = Environment.GetEnvironmentVariable("RABBITVHOST") ?? config.GetValue<string>("RABBITMQ:VHOST"),
            AutomaticRecoveryEnabled = true
        };

        services.AddSingleton<IConnection>((svc) =>
        {
            return factory.CreateConnection($"{config.GetValue<string>("AppName") ?? "MyApp"}_WRITE");
        });

        factory.DispatchConsumersAsync = true;

        services.AddSingleton<IConnection>((svc) =>
        {
            return factory.CreateConnection($"{config.GetValue<string>("AppName") ?? "MyApp"}_READ");
        });

        services.AddTransient<IMessaging, Messaging>();

        return services;
    }
}