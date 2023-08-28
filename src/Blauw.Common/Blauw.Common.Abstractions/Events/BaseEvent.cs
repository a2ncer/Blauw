using MediatR;

namespace Blauw.Common.Abstractions.Events;

public class BaseEvent
{
    private string? _eventType;
    public Guid EventId { get; set; } = Guid.NewGuid();

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public virtual string? SessionId => EventId.ToString();

    public string? EventType
    {
        get => _eventType ?? GetType().FullName;
        set => _eventType = value ?? GetType().FullName;
    }
}