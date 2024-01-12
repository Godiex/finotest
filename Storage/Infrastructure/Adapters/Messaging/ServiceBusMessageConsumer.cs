using Azure.Messaging.ServiceBus;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports;
using Infrastructure.Extensions.Message;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Adapters.Messaging;

public class ServiceBusMessageConsumer<T> : IMessageConsumer<T> where T : class
{
    private readonly ServiceBusClient _client;
    private readonly ILogger<ServiceBusMessageConsumer<T>> _logger;
    private readonly ServiceBusProcessor _serviceBusProcessor;
    private Func<MessageEnvelope<T>, CancellationToken, Task>? _processMessageAsync;
    private string TopicOrQueue { get; set; } 
    private string Subcription { get; set; } 
    
    public event Func<MessageEnvelope<T>, CancellationToken, Task> ProcessMessageAsync
    {
      add
      {
        _processMessageAsync = value ?? throw new ArgumentNullException("ProcessMessageAsync");
      }
      remove
      {
        if (value == null)
          throw new ArgumentNullException("ProcessMessageAsync");
        _processMessageAsync = !(_processMessageAsync != value) ? (Func<MessageEnvelope<T>, CancellationToken, Task>) null : throw new ArgumentException("Error al remover manejador, no corresponde al actualmente asignado", "ProcessMessageAsync");
      }
    }

    public ServiceBusMessageConsumer(
      ServiceBusClient client,
      string topicName,
      string subscriptionName,
      ILogger<ServiceBusMessageConsumer<T>> logger)
    {
      TopicOrQueue = topicName;
      Subcription = subscriptionName;
      _client = client ?? throw new ArgumentNullException(nameof (client));
      _logger = logger ?? throw new ArgumentNullException(nameof (logger));
      _serviceBusProcessor = CreateServiceBusProcessor(topicName, subscriptionName);
      _serviceBusProcessor.ProcessMessageAsync += HandleMessageAsync;
      _serviceBusProcessor.ProcessErrorAsync += HandleErrorAsync;
    }

    public async Task StartListeningAsync(CancellationToken cancellationToken = default (CancellationToken))
    {
      _logger.LogInformation("Iniciando recepción de mensajes desde Topico {TopicOrQueue} subscripcion {Subcription} {entityPath}",TopicOrQueue, Subcription, _serviceBusProcessor.EntityPath);
      await _serviceBusProcessor.StartProcessingAsync(cancellationToken).ConfigureAwait(false);
      _logger.LogInformation("Recepción de mensajes iniciada");
    }

    public async Task StopListeningAsync(CancellationToken cancellationToken = default (CancellationToken))
    {
      _logger.LogInformation("Cerrando recepción de mensajes desde {entityPath}", (object) _serviceBusProcessor.EntityPath);
      await _serviceBusProcessor.StopProcessingAsync(cancellationToken).ConfigureAwait(false);
      await _serviceBusProcessor.CloseAsync(cancellationToken).ConfigureAwait(false);
      _logger.LogInformation("Recepción de mensajes cerrada");
    }

    private async Task HandleMessageAsync(ProcessMessageEventArgs processMessageEventArgs)
    {
      using (_logger.BeginScope<Dictionary<string, object>>(new Dictionary<string, object>()
      {
        ["Subject"] = (object) processMessageEventArgs.Message.Subject,
        ["CorrelationId"] = (object) processMessageEventArgs.Message.CorrelationId,
        ["MessageId"] = (object) processMessageEventArgs.Message.MessageId
      }))
      {
        try
        {
          _logger.LogInformation("Procesando mensaje");
          await OnProcessMessageAsync(processMessageEventArgs.Message.ToMessageEnvelope<T>(), processMessageEventArgs.CancellationToken);
          _logger.LogInformation("Mensaje procesado");
          await processMessageEventArgs.CompleteMessageAsync(processMessageEventArgs.Message);
          _logger.LogInformation("Mensaje completado");
        }
        catch (CoreBusinessException ex)
        {
          await processMessageEventArgs.DeadLetterMessageAsync(processMessageEventArgs.Message, ex.Message);
          _logger.LogError((Exception) ex, "Se envía el mensaje a cola de mensajes muertos");
        }
        catch (Exception ex)
        {
          await processMessageEventArgs.AbandonMessageAsync(processMessageEventArgs.Message);
          _logger.LogError(ex, "Se abandona el mensaje para reintento");
        }
      }
    }

    private Task HandleErrorAsync(ProcessErrorEventArgs args)
    {
      _logger.LogError(args.Exception, "Error al recibir el mensaje");
      return Task.CompletedTask;
    }

    private Task OnProcessMessageAsync(
      MessageEnvelope<T> message,
      CancellationToken cancellationToken)
    {
      Func<MessageEnvelope<T>, CancellationToken, Task> processMessageAsync = _processMessageAsync;
      return processMessageAsync?.Invoke(message, cancellationToken) ?? Task.CompletedTask;
    }

    private ServiceBusProcessor CreateServiceBusProcessor(string topicName, string subscriptionName)
    {
      if (topicName == null)
        throw new ArgumentNullException(nameof (topicName));
      if (subscriptionName == null)
        throw new ArgumentNullException(nameof (subscriptionName));
      return _client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions()
      {
        AutoCompleteMessages = false
      });
    }
  }