using Blauw.Common.Abstractions.Enums;
using Blauw.Transactions.Abstractions.Models;
using MediatR;

namespace Blauw.Transactions.Application.Commands;

public class CreateTransactionCommand : IRequest<Transaction>
{
    public Guid AccountId { get; set; }
    
    public decimal Amount { get; set; }
    
    public Currency Currency { get; set; }
}
