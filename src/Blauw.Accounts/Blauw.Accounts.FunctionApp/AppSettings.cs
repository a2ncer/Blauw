namespace Blauw.Accounts.FunctionApp;

public class AppSettings
{
    public string? ApplicationDbName { get; set; }
    
    public string? AccountTopicName { get; set; }

    public string? ServiceBus { get; set; }

    public ConnectionStrings? ConnectionStrings { get; set; }

}

public class ConnectionStrings
{
    public string? MongoDb { get; set; }
}