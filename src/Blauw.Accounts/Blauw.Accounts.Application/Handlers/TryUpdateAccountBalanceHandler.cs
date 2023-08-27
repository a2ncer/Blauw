using Blauw.Accounts.Abstractions.Repositories;
using Blauw.Accounts.Application.Commands;
using MediatR;

namespace Blauw.Accounts.Application.Handlers;

public class TryUpdateAccountBalanceHandler : IRequestHandler<UpdateAccountBalanceCommand, Unit>
{
    readonly  IAccountRepository _accountRepository;

    public TryUpdateAccountBalanceHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<Unit> Handle(UpdateAccountBalanceCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAsync(request.AccountId);
        
        if (account == null)
        {
            throw new Exception("Account not found for id: " + request.AccountId);
        }
        
        if (account.Balance + request.Amount < 0)
        {
            throw new Exception("Insufficient funds");
        }
        
        // TODO: publish event for transactions service, using IEventBus which will be defined later
        //await _eventBus.PublishAsync(new AccountBalanceUpdatedEvent(request.AccountId, request.Amount));

        return Unit.Value;
    }
}
