using Blauw.Common.Abstractions.Events.Accounts;
using MediatR;

namespace Blauw.Transactions.Application.Commands;

public class BalanceUpdateStatusEventCommand : BalanceUpdateStatusEvent, IRequest<Unit>
{
    
}
