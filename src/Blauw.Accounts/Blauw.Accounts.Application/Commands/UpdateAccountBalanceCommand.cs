using MediatR;

namespace Blauw.Accounts.Application.Commands;

public class UpdateAccountBalanceCommand : IRequest<Unit>
{
    public UpdateAccountBalanceCommand(Guid accountId, decimal amount)
    {
        AccountId = accountId;
        Amount = amount;
    }
    
    public Guid AccountId { get; }
    
    public decimal Amount { get; }
}
