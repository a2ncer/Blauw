using Blauw.Transactions.Abstractions.Enums;
using Blauw.Transactions.Abstractions.Repositories;
using Blauw.Transactions.Application.Commands;
using MediatR;

namespace Blauw.Transactions.Application.Handlers;

public class BalanceUpdateStatusEventCommandHandler : IRequestHandler<BalanceUpdateStatusEventCommand, Unit>
{
    readonly ITransactionRepository _transactionRepository;

    public BalanceUpdateStatusEventCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public async Task<Unit> Handle(BalanceUpdateStatusEventCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetAsync(request.TransactionId);
        
        if (transaction == null)
        {
            throw new Exception("Transaction not found for id: " + request.TransactionId);
        }

        transaction.Status = request.Success ? TransactionStatus.Completed : TransactionStatus.Failed;
        
        await _transactionRepository.UpdateStatusAsync(request.TransactionId, transaction.Status);
        
        return Unit.Value;
    }
}
