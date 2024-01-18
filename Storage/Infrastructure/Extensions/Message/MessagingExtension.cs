using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Domain.Ports;
using Infrastructure.Adapters.Messaging;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure.Extensions.Message;

public static class MessagingExtension
{
    private const string DefaultValue = "DEFAULT_VALUE";
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration config)
    {
        try
        {
            services.Configure<MessageSettings>(config.GetSection(nameof(MessageSettings)));
            MessageSettings? messageSettings = config.GetSection(nameof(MessageSettings)).Get<MessageSettings>();

            services.AddAzureClients((Action<AzureClientFactoryBuilder>)(_ =>
            {
                _.AddServiceBusClient(messageSettings?.ConnectionString ?? DefaultValue)
                    .WithName<ServiceBusClient, ServiceBusClientOptions>(messageSettings?.ConnectionString ?? DefaultValue);
            }));

            services.AddSingleton<IMultimediaMessageConsumer>(service => new MultimediaMessageConsumer(
                service.GetRequiredService<IAzureClientFactory<ServiceBusClient>>()
                    .CreateClient(messageSettings?.ConnectionString ?? DefaultValue),
                messageSettings?.TopicName ?? DefaultValue,
                messageSettings?.SubscriptionName ?? DefaultValue,
                service.GetRequiredService<ILogger<MultimediaMessageConsumer>>()
            ));

            services.AddSingleton<IMultimediaMessagePublisher>(svc => new MultimediaMessagePublisher(
                svc.GetRequiredService<IAzureClientFactory<ServiceBusClient>>()
                    .CreateClient(messageSettings?.ConnectionString ?? DefaultValue),
                new ServiceBusAdministrationClient(messageSettings?.ConnectionString ?? DefaultValue),
                messageSettings?.TopicName ?? DefaultValue
            ));
        }
        catch (Exception e)
        {
            Log.Error($"Error to configure service bus in the config and variables {e.Message}, {e}");
        }
        
        return services;
    }
}