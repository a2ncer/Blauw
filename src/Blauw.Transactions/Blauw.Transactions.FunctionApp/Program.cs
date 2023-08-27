using Blauw.Transactions.Application.Commands;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
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
}).ConfigureOpenApi().ConfigureServices(services =>
{
    services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
    {
        var options = new DefaultOpenApiConfigurationOptions
        {
            OpenApiVersion = OpenApiVersionType.V3,
            Info =
            {
                Title = "Transactions Service"
            }
        };

        return options;
    });
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTransactionCommand).Assembly));
}).Build();

host.Run();
