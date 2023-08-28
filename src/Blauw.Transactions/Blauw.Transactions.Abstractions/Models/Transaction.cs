using Blauw.Common.Abstractions.Enums;
using Blauw.Transactions.Abstractions.Enums;

namespace Blauw.Transactions.Abstractions.Models;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AccountId { get; set; }
    
    public decimal Amount { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow; 
    public DateTimeOffset? ProcessedAt { get; set; }
    
    public Currency Currency { get; set; } = Currency.EUR;
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
}

