using Blauw.Accounts.Abstractions.Models;
using MediatR;

namespace Blauw.Accounts.Application.Queries;

public class GetCustomerAccountsQuery : IRequest<IEnumerable<Account>>
{
    public GetCustomerAccountsQuery(Guid id)
    {
        CustomerId = id;
    }

    public Guid CustomerId { get; }
}
