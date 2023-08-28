using Blauw.Transactions.Application.Commands;
using MediatR;

namespace Blauw.Transactions.Application.Handlers;

public class BalanceChangeRequestedEventCommandHandler : IRequestHandler<BalanceChangeRequestedEventCommand, Unit>
{
    readonly IMediator _mediator;

    public BalanceChangeRequestedEventCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Unit> Handle(BalanceChangeRequestedEventCommand request, CancellationToken cancellationToken)
    {
        var createTransactionCommand = new CreateTransactionCommand
        {
            AccountId = request.AccountId,
            Amount = request.Amount,
            Currency = request.Currency
        };
        
        await _mediator.Send(createTransactionCommand, cancellationToken);
        
        return Unit.Value;
    }
}
