﻿using Blauw.Accounts.Abstractions.Enums;
using Blauw.Accounts.Abstractions.Models;
using Blauw.Accounts.Abstractions.Repositories;
using MongoDB.Driver;

namespace Blauw.Accounts.Infrastructure.DataAccess.Repositories;

public class AccountRepository : IAccountRepository
{
    readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Account?> GetAsync(Guid id)
    {
        var result = await _context.Accounts.FindSync(x => x.Id == id).ToListAsync();

        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<Account>> GetByCustomerIdAsync(Guid customerId)
    {
        var result = await _context.Accounts.FindSync(x => x.CustomerId == customerId).ToListAsync();

        return result;
    }

    public Task CreateAsync(Account account)
    {
        return _context.Accounts.InsertOneAsync(account);
    }

    public async Task IncrementBalanceAsync(Guid id, decimal increment)
    {
        var filter = Builders<Account>.Filter.Eq(x => x.Id, id);
        
        var account = await _context.Accounts.FindSync(filter).FirstOrDefaultAsync();
        
        if (account == null)
        {
            throw new Exception("Account not found");
        }
        
        var update = Builders<Account>.Update.Set(x => x.Balance, account.Balance + increment);

        if (account.Status == AccountStatus.Pending)
        {
            // update status to active
            update = update.Set(x => x.Status, AccountStatus.Active);
        }
        
        var result = await _context.Accounts.UpdateOneAsync(filter, update);
        
        if (result.MatchedCount == 0)
        {
            throw new Exception("Account was not updated" + account.Id);
        }
    }
}
