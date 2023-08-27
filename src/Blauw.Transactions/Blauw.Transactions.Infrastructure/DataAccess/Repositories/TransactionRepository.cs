using Blauw.Transactions.Abstractions.Models;
using Blauw.Transactions.Abstractions.Repositories;

namespace Blauw.Transactions.Infrastructure.DataAccess.Repositories;

public class TransactionRepository : ITransactionRepository
{
    public Task<Transaction> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Transaction>> GetAsync(Guid accountId, DateTimeOffset? from, DateTimeOffset? to)
    {
        throw new NotImplementedException();
    }

    public Task<Transaction> CreateAsync(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public Task<Transaction> UpdateStatusAsync(Guid transactionId, TransactionStatus status)
    {
        throw new NotImplementedException();
    }
}
