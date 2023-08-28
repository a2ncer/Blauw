using Blauw.Transactions.Abstractions.Models;
using Blauw.Transactions.Abstractions.Repositories;
using Blauw.Transactions.Application.Commands;
using Blauw.Transactions.Application.Queries;
using MediatR;

namespace Blauw.Transactions.Application.Handlers;

public class GetAccountTransactionsHandler : IRequestHandler<GetAccountTransactionsQuery, IEnumerable<Transaction>>
{
    readonly ITransactionRepository _transactionRepository;

    public GetAccountTransactionsHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    
    public Task<IEnumerable<Transaction>> Handle(GetAccountTransactionsQuery request, CancellationToken cancellationToken)
    {
        return _transactionRepository.GetByAccountIdAsync(request.AccountId);
    }
}
