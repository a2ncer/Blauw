using Blauw.Accounts.Abstractions.Repositories;
using Blauw.Accounts.Application.Commands;
using Blauw.Accounts.Infrastructure.DataAccess;
using Blauw.Accounts.Infrastructure.DataAccess.Repositories;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Blauw.Accounts.FunctionApp.DI;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services.AddJsonSerializerOptions().AddConfiguration().AddDbContext().AddRepositories().AddOpenApi().AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAccountCommand).Assembly))
            .AddAutoMapper(typeof(Program));
    }

    static IServiceCollection AddJsonSerializerOptions(this IServiceCollection services)
    {
        return services.Configure<JsonSerializerSettings>(options =>
        {
            options.Converters.Add(new StringEnumConverter());
        });
    }

    static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services.AddOptions<AppSettings>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.Bind(settings);
        });
        services.AddSingleton(s => s.GetRequiredService<IOptions<AppSettings>>().Value);
        return services;
    }

    static IServiceCollection AddOpenApi(this IServiceCollection services)
    {
        return services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
        {
            var options = new DefaultOpenApiConfigurationOptions
            {
                OpenApiVersion = OpenApiVersionType.V3,
                Info =
                {
                    Title = "Accounts Service"
                }
            };

            return options;
        });
    }

    static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        ApplicationDbContext.RegisterMappings();

        services.AddSingleton(s =>
        {
            var settings = s.GetRequiredService<AppSettings>();
            ArgumentNullException.ThrowIfNull(settings.ConnectionStrings);
            ArgumentNullException.ThrowIfNull(settings.ConnectionStrings.MongoDb);

            var client = new MongoClient(settings.ConnectionStrings.MongoDb);
            var database = client.GetDatabase(settings.ApplicationDbName);
            return new ApplicationDbContext(database).CreateIndexes();
        });

        return services;
    }

    static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddSingleton<IAccountRepository, AccountRepository>();
    }
}
