using Blauw.Accounts.Abstractions.Models;
using Blauw.Common.Abstractions.Enums;
using MediatR;

namespace Blauw.Accounts.Application.Commands;

public class CreateAccountCommand : IRequest<Account>
{
    public Guid CustomerId { get; set; }
    
    public decimal Balance { get; set; }
    
    public Currency Currency { get; set; } = Currency.EUR;
}
