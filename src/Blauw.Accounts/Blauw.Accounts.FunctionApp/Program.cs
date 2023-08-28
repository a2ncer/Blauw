using Blauw.Accounts.FunctionApp.DI;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var host = new HostBuilder().ConfigureFunctionsWorkerDefaults(builder =>
    {
        builder.UseNewtonsoftJson(new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>()
            {
                new StringEnumConverter()
            }
        });
    },
    options =>
    {
        options.EnableUserCodeException = true;
    }).ConfigureOpenApi().ConfigureServices(services => services.AddInfrastructureServices()).Build();

host.Run();
