using System.ComponentModel.DataAnnotations;
using System.Net;
using AutoMapper;
using Blauw.Transactions.Abstractions.Models;
using Blauw.Transactions.Application.Commands;
using Blauw.Transactions.Application.Queries;
using Blauw.Transactions.FunctionApp.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Blauw.Transactions.FunctionApp.Api;

public class TransactionsFunctions
{
    readonly IMediator _mediator;
    readonly IMapper _mapper;
    readonly ILogger<TransactionsFunctions> _logger;

    public TransactionsFunctions(IMediator mediator, IMapper mapper, ILogger<TransactionsFunctions> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }
    
    [Function("Transactions.Create")]
    [OpenApiOperation(operationId: "createTransaction",
        tags: new[] {"Transactions"},
        Summary = "Create a transaction for an account",
        Description = "This creates new transaction for an account",
        Visibility = OpenApiVisibilityType.Important)]
    [OpenApiRequestBody("application/json", bodyType: typeof(CreateTransactionCommand), Required = true)]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(Transaction),
        Description = "Successful operation")]
    public async Task<HttpResponseData> CreateTransactionAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "transactions")] HttpRequestData req, [FromBody][Required] CreateTransactionCommand transactionCommand)
    {
        _logger.LogInformation("Create transaction for account {AccountId}", transactionCommand.AccountId);
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        var transaction = await _mediator.Send(transactionCommand);
        
        await response.WriteAsJsonAsync(transaction);

        return response;
    }
    
    [Function("Transactions.GetAll")]
    [OpenApiOperation(operationId: "getTransactions",
        tags: new[] {"Transactions"},
        Summary = "Get all transactions for an account",
        Description = "This gets all transactions for an account",
        Visibility = OpenApiVisibilityType.Important)]
    [OpenApiParameter("accountId", In = ParameterLocation.Query, Required = true, Type = typeof(Guid), Summary = "The account id")]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(TransactionsDto),
        Description = "Successful operation")]
    public async Task<HttpResponseData> GetAccountsAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "transactions")] HttpRequestData req, [FromQuery] Guid accountId, FunctionContext executionContext)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        var transactions = await _mediator.Send(new GetAccountTransactionsQuery(accountId)); 
        
        var accountsResponse = new TransactionsDto()
        {
            Data = transactions
        };

        await response.WriteAsJsonAsync(accountsResponse);

        return response;
    }
}
