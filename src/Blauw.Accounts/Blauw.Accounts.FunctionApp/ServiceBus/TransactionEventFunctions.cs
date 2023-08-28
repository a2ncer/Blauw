using Blauw.Common.Abstractions.Events;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;

namespace Blauw.Accounts.FunctionApp.ServiceBus;

public class TransactionFunctions
{
    readonly IMediator _mediator;

    public TransactionFunctions(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Function("TransactionFunctions")]
    public async Task OnTransactionEventAsync([ServiceBusTrigger("%TransactionTopicName%", "%TransactionSubscriptionName%", Connection = "ServiceBus", IsSessionsEnabled = true)] string message,
        DateTime enqueuedTimeUtc)
    {
        var baseEvent = JsonConvert.DeserializeObject<BaseEvent>(message);

        if (baseEvent == null || baseEvent.EventType == null)
        {
            throw new Exception("Could not deserialize message");
        }

        var eventType = Type.GetType($"Blauw.Accounts.Application.Commands.{baseEvent.EventType}Command, Blauw.Accounts.Application");

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
