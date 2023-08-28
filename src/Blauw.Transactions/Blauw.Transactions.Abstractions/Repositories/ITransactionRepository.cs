using Blauw.Transactions.Abstractions.Enums;
using Blauw.Transactions.Abstractions.Models;

namespace Blauw.Transactions.Abstractions.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetAsync(Guid id);
    
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, DateTimeOffset? from = default, DateTimeOffset? to = default);
    
    Task CreateAsync(Transaction transaction);
    
    Task<Transaction> UpdateStatusAsync(Guid id, TransactionStatus status);
}
