using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Domain.Ports;

namespace Infrastructure.Adapters.Messaging;

public class MultimediaMessagePublisher : ServiceBusMessagePublisher, IMultimediaMessagePublisher
{
    public MultimediaMessagePublisher(
        ServiceBusClient serviceBusClient,
        ServiceBusAdministrationClient serviceBusAdministrationClient,
        string topicOrQueue) : base(serviceBusClient, serviceBusAdministrationClient, topicOrQueue)
    {
    }
}