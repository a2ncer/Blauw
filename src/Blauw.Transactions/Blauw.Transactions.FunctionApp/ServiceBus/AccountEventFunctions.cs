using Blauw.Common.Abstractions.Events;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;

namespace Blauw.Transactions.FunctionApp.ServiceBus;

public class AccountEventFunctions
{
    readonly IMediator _mediator;

    public AccountEventFunctions(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Function("TransactionFunctions")]
    public async Task OnAccountEventAsync([ServiceBusTrigger("%AccountTopicName%", "%AccountSubscriptionName%", Connection = "ServiceBus", IsSessionsEnabled = true)] string message, DateTime enqueuedTimeUtc)
    {
        var baseEvent = JsonConvert.DeserializeObject<BaseEvent>(message);
        
        if (baseEvent == null || baseEvent.EventType == null)
        {
            throw new Exception("Could not deserialize message");
        }

        var eventType = Type.GetType($"{baseEvent.EventType}, Blauw.Common.Abstractions");
        
        if (eventType == null)
        {
            throw new Exception("Could not find event type");
        }
        
        var eventObject = JsonConvert.DeserializeObject(message, eventType);
        
        if (eventObject == null)
        {
            throw new Exception("Could not deserialize message");
        }

        await _mediator.Send(eventObject);
    }
}
