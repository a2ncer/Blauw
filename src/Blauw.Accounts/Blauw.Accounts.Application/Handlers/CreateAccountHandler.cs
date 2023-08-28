using Blauw.Accounts.Abstractions.Enums;
using Blauw.Accounts.Abstractions.Models;
using Blauw.Accounts.Abstractions.Repositories;
using Blauw.Accounts.Application.Commands;
using Blauw.Common.Abstractions.EventBus;
using Blauw.Common.Abstractions.Events.Accounts;
using MediatR;

namespace Blauw.Accounts.Application.Handlers;

public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, Account>
{
    readonly IAccountRepository _accountRepository;
    readonly IEventBus _eventBus;

    public CreateAccountHandler(IAccountRepository accountRepository, IEventBus eventBus)
    {
        _accountRepository = accountRepository;
        _eventBus = eventBus;
    }

    public async Task<Account> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        // TODO: Validate request: if customer exists, if currency is valid, if balance is valid, etc.

        var account = new Account
        {
            CustomerId = request.CustomerId,
            Currency = request.Currency,
            Balance = 0,
        };

        if (request.Balance > 0)
        {
            account.Status = AccountStatus.Pending;

            await _accountRepository.CreateAsync(account);

            // Publish event, so that the balance can be updated
            await _eventBus.PublishAsync(new BalanceChangeRequestedEvent
                {
                    Amount = request.Balance,
                    Currency = request.Currency,
                    AccountId = account.Id
                },
                cancellationToken);
        }
        else
        {
            await _accountRepository.CreateAsync(account);
        }

        return account;
    }
}
