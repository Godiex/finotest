using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Domain.Ports;
using Infrastructure.Adapters.Messaging;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Extensions.Message;

public static class MessagingExtension
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<MessageSettings>(config.GetSection(nameof(MessageSettings)));
        var messageSettings = config.GetSection(nameof(MessageSettings)).Get<MessageSettings>();
        
        services.AddAzureClients((Action<AzureClientFactoryBuilder>) (_ =>
        {
            _.AddServiceBusClient(messageSettings.ConnectionString).WithName<ServiceBusClient, ServiceBusClientOptions>(messageSettings.ConnectionString);
        }));
        
        services.AddSingleton<IMultimediaMessageConsumer>(service => new MultimediaMessageConsumer(
            service.GetRequiredService<IAzureClientFactory<ServiceBusClient>>().CreateClient(messageSettings.ConnectionString),
            messageSettings.TopicName,
            messageSettings.SubscriptionName,
            service.GetRequiredService<ILogger<MultimediaMessageConsumer>>()
        ));
        
        services.AddSingleton<IMultimediaMessagePublisher>(svc => new MultimediaMessagePublisher(
            svc.GetRequiredService<IAzureClientFactory<ServiceBusClient>>().CreateClient(messageSettings.ConnectionString),
            new ServiceBusAdministrationClient(messageSettings.ConnectionString),
            messageSettings.TopicName
        ));
        
        return services;
    }
}