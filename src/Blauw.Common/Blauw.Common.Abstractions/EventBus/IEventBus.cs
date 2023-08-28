using Blauw.Common.Abstractions.Events;

namespace Blauw.Common.Abstractions.EventBus;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : BaseEvent;
}