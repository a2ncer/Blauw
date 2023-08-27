using Blauw.Accounts.Abstractions.Enums;
using Blauw.Accounts.Abstractions.Models;
using Blauw.Accounts.Abstractions.Repositories;
using Blauw.Accounts.Application.Commands;
using MediatR;

namespace Blauw.Accounts.Application.Handlers;

public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, Account>
{
    readonly IAccountRepository _accountRepository;

    public CreateAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
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

            // TODO: Publish transaction event using IEventBus
            // await _eventBus.PublishAsync(new AccountBalanceUpdatedEvent(account.Id, request.Balance));
        }
        else
        {
            await _accountRepository.CreateAsync(account);
        }
        
        return account;
    }
}
