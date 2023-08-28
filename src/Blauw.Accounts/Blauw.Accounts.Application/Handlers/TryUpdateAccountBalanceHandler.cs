using Blauw.Accounts.Abstractions.Repositories;
using Blauw.Accounts.Application.Commands;
using Blauw.Common.Abstractions.EventBus;
using Blauw.Common.Abstractions.Events.Accounts;
using MediatR;

namespace Blauw.Accounts.Application.Handlers;

public class TryUpdateAccountBalanceHandler : IRequestHandler<UpdateAccountBalanceCommand, Unit>
{
    readonly  IAccountRepository _accountRepository;
    readonly IEventBus _eventBus;

    public TryUpdateAccountBalanceHandler(IAccountRepository accountRepository, IEventBus eventBus)
    {
        _accountRepository = accountRepository;
        _eventBus = eventBus;
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
        
        await _eventBus.PublishAsync(new BalanceChangeRequestedEvent
            {
                Amount = request.Amount,
                Currency = account.Currency,
                AccountId = request.AccountId
            },
            cancellationToken);

        return Unit.Value;
    }
}
