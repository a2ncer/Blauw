using Blauw.Accounts.Abstractions.Models;
using MediatR;

namespace Blauw.Accounts.Application.Queries;

public class GetAccountQuery : IRequest<Account>
{
    public GetAccountQuery(Guid id)
    {
        AccountId = id;
    }

    public Guid AccountId { get; }
}
