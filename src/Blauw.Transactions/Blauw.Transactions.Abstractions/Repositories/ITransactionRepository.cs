using Blauw.Transactions.Abstractions.Models;

namespace Blauw.Transactions.Abstractions.Repositories;

public interface ITransactionRepository
{
    Task<Transaction> GetAsync(Guid id);
    
    Task<IEnumerable<Transaction>> GetAsync(Guid accountId, DateTimeOffset? from, DateTimeOffset? to);
    
    Task<Transaction> CreateAsync(Transaction transaction);
    
    Task<Transaction> UpdateStatusAsync(Guid transactionId, TransactionStatus status);
}
