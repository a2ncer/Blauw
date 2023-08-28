using Blauw.Accounts.Abstractions.Repositories;
using Blauw.Accounts.Application.Commands;
using Blauw.Common.Abstractions.EventBus;
using Blauw.Common.Abstractions.Events.Accounts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Blauw.Accounts.Application.Handlers;

public class TransactionEventCommandHandler : IRequestHandler<TransactionEventCommand, Unit>
{
    readonly IAccountRepository _accountRepository;
    readonly IEventBus _eventBus;
    readonly ILogger<TransactionEventCommandHandler> _logger;

    public TransactionEventCommandHandler(IAccountRepository accountRepository, IEventBus eventBus, ILogger<TransactionEventCommandHandler> logger)
    {
        _accountRepository = accountRepository;
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task<Unit> Handle(TransactionEventCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetAsync(request.AccountId);

        var balanceUpdateStatusEvent = new BalanceUpdateStatusEvent
        {
            Success = false,
            TransactionId = request.TransactionId,
            AccountId = request.AccountId
        };

        if (account == null)
        {
            // log error
            _logger.LogWarning("Account not found for id: {AccountId} ", request.AccountId);

            await _eventBus.PublishAsync(balanceUpdateStatusEvent, cancellationToken);

            return Unit.Value;
        }

        if (account.Balance + request.Amount < 0)
        {
            // log error
            _logger.LogWarning("Insufficient funds for account id: {AccountId} ", request.AccountId);

            await _eventBus.PublishAsync(balanceUpdateStatusEvent, cancellationToken);

            return Unit.Value;
        }

        try
        {
            await _accountRepository.IncrementBalanceAsync(request.AccountId, request.Amount);

            balanceUpdateStatusEvent.Success = true;
        }
        catch (Exception e)
        {
            // log error
            _logger.LogError(e, "Error while updating balance for account id: {AccountId} ", request.AccountId);

            await _eventBus.PublishAsync(balanceUpdateStatusEvent, cancellationToken);

            return Unit.Value;
        }

        try
        {
            await _eventBus.PublishAsync(balanceUpdateStatusEvent, cancellationToken);
        }
        catch (Exception e)
        {
            // log error
            _logger.LogError(e, "Error while publishing balance update status event for account id: {AccountId} ", request.AccountId);

            await _accountRepository.IncrementBalanceAsync(request.AccountId, -request.Amount);
        }


        return Unit.Value;
    }
}
