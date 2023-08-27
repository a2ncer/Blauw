using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Blauw.Accounts.Application.Commands;
using Blauw.Accounts.Application.Queries;
using Blauw.Accounts.FunctionApp.Dto;
using Blauw.Accounts.FunctionApp.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Blauw.Accounts.FunctionApp.Api;

public class AccountsFunctions
{
    readonly IMediator _mediator;
    readonly IMapper _mapper;
    readonly ILogger<AccountsFunctions> _logger;

    public AccountsFunctions(IMediator mediator, IMapper mapper, ILogger<AccountsFunctions> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }
    
    [Function("Accounts.Create")]
    [OpenApiOperation(operationId: "createAccount",
        tags: new[] {"Accounts"},
        Summary = "Create an account for a customer",
        Description = "This creates new account for a customer",
        Visibility = OpenApiVisibilityType.Important)]
    [OpenApiRequestBody("application/json", bodyType: typeof(CreateAccountCommand), Required = true)]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(SingleAccountDto),
        Description = "Successful operation")]
    public async Task<HttpResponseData> CreateAccountAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "accounts")] HttpRequestData req, [FromBody][Required] CreateAccountCommand accountCommand)
    {
        _logger.LogInformation("Create account for customer {CustomerId}", accountCommand.CustomerId);
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        var account = await _mediator.Send(accountCommand);
        
        var accountResponse = _mapper.Map<SingleAccountDto>(account);

        await response.WriteAsJsonAsync(accountResponse);

        return response;
    }
    
    [Function("Accounts.GetAll")]
    [OpenApiOperation(operationId: "getAccounts",
        tags: new[] {"Accounts"},
        Summary = "Get accounts for a customer",
        Description = "This gets the accounts for a customer",
        Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter("customerId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "The customer id")]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(AccountsDto),
        Description = "Successful operation")]
    public async Task<HttpResponseData> GetAccountsAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "accounts")] HttpRequestData req, [FromQuery] Guid customerId, FunctionContext executionContext)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        var accounts = await _mediator.Send(new GetCustomerAccountsQuery(customerId)); 
        
        var accountsResponse = new AccountsDto
        {
            Data = _mapper.Map<IEnumerable<SingleAccountDto>>(accounts)
        };

        await response.WriteAsJsonAsync(accountsResponse);

        return response;
    }
    
    [Function("Accounts.Get")]
    [OpenApiOperation(operationId: "getAccount",
        tags: new[] {"Accounts"},
        Summary = "Get account",
        Description = "This gets the account",
        Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter("id", In = ParameterLocation.Path, Required = true, Type = typeof(Guid), Summary = "The account id")]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(SingleAccountDto),
        Description = "Successful operation")]
    public async Task<HttpResponseData> GetAccountAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "accounts/{id}")] HttpRequestData req, Guid id, FunctionContext executionContext)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);

        var account = await _mediator.Send(new GetAccountQuery(id));
        
        var accountResponse = _mapper.Map<SingleAccountDto>(account);

        await response.WriteAsJsonAsync(accountResponse);

        return await Task.FromResult(response);
    }
    
    [Function("Accounts.UpdateBalance")]
    [OpenApiOperation(operationId: "updateBalance",
        tags: new[] {"Accounts"},
        Summary = "Update an account for a customer",
        Description = "This updates an account for a customer",
        Visibility = OpenApiVisibilityType.Important)]
    [OpenApiRequestBody("application/json", bodyType: typeof(UpdateAccountBalanceRequest), Required = true)]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "Successful operation")]
    public async Task<HttpResponseData> UpdateAccountBalanceAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "accounts/{id}/balance")] HttpRequestData req, Guid id, [FromBody][Required] UpdateAccountBalanceRequest updateAccountBalanceRequest)
    {
        _logger.LogInformation("Update account {Id} balance with {Increment}", id, updateAccountBalanceRequest.Amount);
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        await _mediator.Send(new UpdateAccountBalanceCommand(id, updateAccountBalanceRequest.Amount));

        return response;
    }
}
