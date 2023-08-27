namespace Blauw.Accounts.FunctionApp.Dto;

public class AccountsDto
{
    public IEnumerable<SingleAccountDto> Data { get; set; } = new List<SingleAccountDto>();
}
