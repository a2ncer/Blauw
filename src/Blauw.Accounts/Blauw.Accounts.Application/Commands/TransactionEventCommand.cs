using Blauw.Common.Abstractions.Events.Transactions;
using MediatR;

namespace Blauw.Accounts.Application.Commands;

public class TransactionEventCommand : TransactionEvent, IRequest<Unit>
{
    
}
