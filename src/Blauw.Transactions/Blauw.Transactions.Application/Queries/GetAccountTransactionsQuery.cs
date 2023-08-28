using Blauw.Transactions.Abstractions.Models;
using MediatR;

namespace Blauw.Transactions.Application.Queries;

public class GetAccountTransactionsQuery : IRequest<IEnumerable<Transaction>>
{
    public GetAccountTransactionsQuery(Guid accountId)
    {
        AccountId = accountId;
    }

    public Guid AccountId { get; }
}
