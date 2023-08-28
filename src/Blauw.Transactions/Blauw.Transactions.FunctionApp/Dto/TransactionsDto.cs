using Blauw.Transactions.Abstractions.Models;

namespace Blauw.Transactions.FunctionApp.Dto;

public class TransactionsDto
{
    public IEnumerable<Transaction> Data { get; set; } = new List<Transaction>();
}
