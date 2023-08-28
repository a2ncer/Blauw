using Blauw.Common.Abstractions.Events.Accounts;
using MediatR;

namespace Blauw.Accounts.Application.Handlers;

public class UpdateAccountBalanceHandler : IRequestHandler<BalanceChangeRequestedEvent, Unit>
{
    public Task<Unit> Handle(BalanceChangeRequestedEvent request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
