using Blauw.Accounts.Abstractions.Models;
using Blauw.Accounts.Abstractions.Repositories;
using Blauw.Accounts.Application.Queries;
using MediatR;

namespace Blauw.Accounts.Application.Handlers;

public class GetCustomerAccountsHandler : IRequestHandler<GetCustomerAccountsQuery, IEnumerable<Account>>
{
    readonly IAccountRepository _accountRepository;

    public GetCustomerAccountsHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<IEnumerable<Account>> Handle(GetCustomerAccountsQuery request, CancellationToken cancellationToken)
    {
        return await _accountRepository.GetByCustomerIdAsync(request.CustomerId);
    }
}
