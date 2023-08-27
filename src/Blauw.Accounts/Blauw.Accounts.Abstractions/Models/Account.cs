using Blauw.Accounts.Abstractions.Enums;
using Blauw.Common.Abstractions.Enums;

namespace Blauw.Accounts.Abstractions.Models;

public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid CustomerId { get; set; }
    
    public decimal Balance { get; set; }

    public Currency Currency { get; set; } = Currency.EUR;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    public AccountStatus Status { get; set; } = AccountStatus.Active;
}
