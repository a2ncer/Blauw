using Blauw.Common.Abstractions.Enums;

namespace Blauw.Common.Abstractions.Events;

public class TransactionEvent : BaseEvent
{
    public Guid TransactionId { get; set; }
    public Guid AccountId { get; set; }
    
    public decimal Amount { get; set; }
    
    public Currency Currency { get; set; }
    
    public override string? SessionId => $"accountId_{AccountId}";
}
