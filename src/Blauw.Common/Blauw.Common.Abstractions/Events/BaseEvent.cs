using Blauw.Common.Abstractions.Enums;

namespace Blauw.Common.Abstractions.Events;

public abstract class BaseEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public virtual string? SessionId => EventId.ToString();
    
    public virtual string? EventType => GetType().Name;
    
    public EventStatus Status { get; set; } = EventStatus.Pending;
}
