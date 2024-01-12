using Azure.Messaging.ServiceBus;
using Domain.Ports;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Adapters.Messaging;

public class MultimediaMessageConsumer : ServiceBusMessageConsumer<BinaryData>, IMultimediaMessageConsumer
{
    public MultimediaMessageConsumer(ServiceBusClient client, string topicName, string subscriptionName, ILogger<ServiceBusMessageConsumer<BinaryData>> logger) : base(client, topicName, subscriptionName, logger)
    {
    }
}