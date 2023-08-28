using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Blauw.Accounts.FunctionApp.ServiceBus;

public static class TransactionFunctions
{
    [Function("TransactionFunctions")]
    public static void Run([ServiceBusTrigger("%TransactionTopicName%", "%TransactionSubscriptionName%", Connection = "ServiceBus", IsSessionsEnabled = true)] string mySbMsg, DateTime enqueuedTimeUtc)
    {
        Console.WriteLine(mySbMsg);

    }
}
