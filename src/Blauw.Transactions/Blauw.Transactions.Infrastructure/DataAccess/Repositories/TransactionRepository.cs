using Blauw.Transactions.Abstractions.Enums;
using Blauw.Transactions.Abstractions.Models;
using Blauw.Transactions.Abstractions.Repositories;
using MongoDB.Driver;

namespace Blauw.Transactions.Infrastructure.DataAccess.Repositories;

public class TransactionRepository : ITransactionRepository
{
    readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Transaction?> GetAsync(Guid id)
    {
        var result = await _context.Transactions.FindSync(x => x.Id == id).ToListAsync();

        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(Guid accountId, DateTimeOffset? from, DateTimeOffset? to)
    {
        var filter = Builders<Transaction>.Filter.Eq(x => x.AccountId, accountId);
        
        if (from.HasValue)
        {
            filter = Builders<Transaction>.Filter.And(filter, Builders<Transaction>.Filter.Gte(x => x.CreatedAt, from.Value));
        }
        
        if (to.HasValue)
        {
            filter = Builders<Transaction>.Filter.And(filter, Builders<Transaction>.Filter.Lte(x => x.CreatedAt, to.Value));
        }
        
        var result = await _context.Transactions.FindSync(filter).ToListAsync();
        
        return result.OrderByDescending(x => x.CreatedAt);
    }

    public Task CreateAsync(Transaction transaction)
    {
        return _context.Transactions.InsertOneAsync(transaction);
    }

    public Task<Transaction> UpdateStatusAsync(Guid id, TransactionStatus status)
    {
        var filter = Builders<Transaction>.Filter.Eq(x => x.Id, id);
        
        var update = Builders<Transaction>.Update.Set(x => x.Status, status);
        
        return _context.Transactions.FindOneAndUpdateAsync(filter, update);
    }
}
