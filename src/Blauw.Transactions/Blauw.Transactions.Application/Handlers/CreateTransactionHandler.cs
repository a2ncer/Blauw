using Blauw.Transactions.Abstractions.Enums;
using Blauw.Transactions.Abstractions.Models;
using Blauw.Transactions.Abstractions.Repositories;
using Blauw.Transactions.Application.Commands;
using MediatR;

namespace Blauw.Transactions.Application.Handlers;

public class CreateTransactionHandler : IRequestHandler<CreateTransactionCommand, Transaction>
{
    readonly ITransactionRepository _transactionRepository;

    public CreateTransactionHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<Transaction> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        // Validate request: if account exists, if currency is valid, if amount is valid, etc.

        if (request.Amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Amount), "Amount must be greater than 0.");
        }
        
        var transaction = new Transaction
        {
            AccountId = request.AccountId,
            Amount = request.Amount,
            Currency = request.Currency,
            Status = TransactionStatus.Pending
        };
        
        await _transactionRepository.CreateAsync(transaction);
        
        // TODO: Publish transaction event using IEventBus
        // await _eventBus.PublishAsync(new TransactionCreatedEvent(transaction.Id));
        
        return transaction;
    }
}
