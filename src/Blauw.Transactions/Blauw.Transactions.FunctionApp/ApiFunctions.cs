using System.Net;
using Blauw.Transactions.FunctionApp.Responses;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;

namespace Blauw.Transactions.FunctionApp;

public class ApiFunctions
{
    [Function("ApiFunctions")]
    [OpenApiOperation(operationId: "getTransactions",
        tags: new[] {"Transactions"},
        Summary = "Get transactions",
        Description = "This gets the transactions",
        Visibility = OpenApiVisibilityType.Important)]
    [OpenApiResponseWithBody(
        statusCode: HttpStatusCode.OK,
        contentType: "application/json",
        bodyType: typeof(TransactionsResponse),
        Description = "Successful operation")]
    public async Task<HttpResponseData> GetAccountsAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "transactions")] HttpRequestData req, FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("ApiFunctions");
        logger.LogInformation("C# HTTP trigger function processed a request"); 
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        
        var accountsResponse = new TransactionsResponse
        {
            Data = new[] {"transaction1", "transaction2"}
        };

        await response.WriteAsJsonAsync(accountsResponse);

        return await Task.FromResult(response);
    }
}
