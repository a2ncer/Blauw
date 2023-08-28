using Blauw.Common.Abstractions.EventBus;
using Blauw.Common.Abstractions.Events.Transactions;
using Blauw.Transactions.Abstractions.Enums;
using Blauw.Transactions.Abstractions.Models;
using Blauw.Transactions.Abstractions.Repositories;
using Blauw.Transactions.Application.Commands;
using MediatR;

namespace Blauw.Transactions.Application.Handlers;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Transaction>
{
    readonly ITransactionRepository _transactionRepository;
    readonly IEventBus _eventBus;

    public CreateTransactionHandler(ITransactionRepository transactionRepository, IEventBus eventBus)
    {
        _transactionRepository = transactionRepository;
        _eventBus = eventBus;
    }

    public async Task<Transaction> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        // Validate request: if account exists, if currency is valid, if amount is valid, etc.
        var transaction = new Transaction
        {
            AccountId = request.AccountId,
            Amount = request.Amount,
            Currency = request.Currency,
            Status = TransactionStatus.Pending
        };

        await _transactionRepository.CreateAsync(transaction);
        
        await _eventBus.PublishAsync(new TransactionEvent
            {
                Amount = request.Amount,
                Currency = request.Currency,
                AccountId = request.AccountId,
                TransactionId = transaction.Id
            },
            cancellationToken);

        return transaction;
    }
}
