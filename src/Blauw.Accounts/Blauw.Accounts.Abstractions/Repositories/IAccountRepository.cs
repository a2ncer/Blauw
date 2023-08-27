using Blauw.Accounts.Abstractions.Models;

namespace Blauw.Accounts.Abstractions.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetAsync(Guid id);
    
    Task<IEnumerable<Account>> GetByCustomerIdAsync(Guid customerId);
    
    Task CreateAsync(Account account);
    
    Task IncrementBalanceAsync(Guid id, decimal increment);
}
