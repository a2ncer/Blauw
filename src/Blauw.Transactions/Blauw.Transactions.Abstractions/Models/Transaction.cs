namespace Blauw.Transactions.Abstractions.Models;

public class Transaction
{
    public Transaction(Guid id, Guid accountId, double amount, DateTimeOffset createdAt, DateTimeOffset processedAt, TransactionStatus status)
    {
        Id = id;
        AccountId = accountId;
        Amount = amount;
        CreatedAt = createdAt;
        ProcessedAt = processedAt;
        Status = status;
    }

    public Guid Id { get; }
    public Guid AccountId { get; }
    public double Amount { get; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset ProcessedAt { get; }
    public TransactionStatus Status { get; }
}

public enum TransactionStatus
{
    Pending,
    Completed,
    Failed
}
