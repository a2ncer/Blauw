using Blauw.Common.Abstractions.Enums;
using MediatR;

namespace Blauw.Common.Abstractions.Events.Accounts;

public class BalanceChangeRequestedEvent : BaseEvent, IRequest<Unit>
{
    public Guid AccountId { get; set; }
    
    public decimal Amount { get; set; }
    
    public Currency Currency { get; set; }
    
    public override string? SessionId => $"{EventType}_accountId_{AccountId}";
}
