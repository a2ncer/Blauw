using System.Text;
using Azure.Messaging.ServiceBus;
using Blauw.Common.Abstractions.EventBus;
using Blauw.Common.Abstractions.Events;
using Newtonsoft.Json;

namespace Blauw.Common.Infrastructure.EventBus;

public class EventBus : IEventBus
{
    readonly ServiceBusSender _topicSender;

    public EventBus(string serviceBusConnectionString, string topicName)
    {
        _topicSender = new ServiceBusClient(serviceBusConnectionString).CreateSender(topicName);
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : BaseEvent
    {
        var messageData = JsonConvert.SerializeObject(@event);
        var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageData))
        {
            SessionId = @event.SessionId
        };
        await _topicSender.SendMessageAsync(message, cancellationToken);
    }
}