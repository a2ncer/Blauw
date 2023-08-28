namespace Blauw.Common.Abstractions.Events.Accounts;

public class BalanceUpdateStatusEvent : BaseEvent
{
    public Guid AccountId { get; set; }

    public Guid TransactionId { get; set; }

    public bool Success { get; set; }

    public override string? SessionId => $"{EventType}_transactionId_{TransactionId}";
}
