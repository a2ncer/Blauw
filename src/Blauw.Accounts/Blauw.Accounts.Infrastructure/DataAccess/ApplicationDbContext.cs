using Blauw.Accounts.Abstractions.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Blauw.Accounts.Infrastructure.DataAccess;

public class ApplicationDbContext
{
    readonly IMongoDatabase _database;

    public ApplicationDbContext(IMongoDatabase database)
    {
        _database = database;
    }
    
    public static void RegisterMappings()
    {
        // Set up MongoDB conventions
        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreIfNullConvention(true),
            new IgnoreExtraElementsConvention(true),
            new EnumRepresentationConvention(BsonType.String)
        };
        ConventionRegistry.Register("Serialization conventions", pack, _ => true);
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.Document));
        
        BsonClassMap.RegisterClassMap<Account>();
    }
    
    public virtual IMongoCollection<Account> Accounts =>
        _database.GetCollection<Account>("Accounts");
    
    public ApplicationDbContext CreateIndexes()
    {
        CreateIndexesAsync().GetAwaiter().GetResult();
        return this;
    }
    
    Task CreateIndexesAsync()
    {
        var tasks = new List<Task>
        {
            AccountsIndexesAsync(),
        };

        return Task.WhenAll(tasks);
    }
    
    Task AccountsIndexesAsync()
    {
        var indexes = new List<CreateIndexModel<Account>>
        {
            new(Builders<Account>.IndexKeys.Ascending(x => x.Id)
                    .Ascending(x => x.CustomerId),
                new CreateIndexOptions { Unique = true }),
        };

        return Accounts.Indexes.CreateManyAsync(indexes);
    }
}
