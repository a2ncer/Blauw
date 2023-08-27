using Blauw.Accounts.Abstractions.Enums;
using Blauw.Common.Abstractions.Enums;

namespace Blauw.Accounts.FunctionApp.Dto;

public class SingleAccountDto
{
    public Guid AccountId { get; set; }
    
    public Guid CustomerId { get; set; }
    
    public double Balance { get; set; }
    
    public Currency Currency { get; set; }
    
    public AccountStatus Status { get; set; }
}
