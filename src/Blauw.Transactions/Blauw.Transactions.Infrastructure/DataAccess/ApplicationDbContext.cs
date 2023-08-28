using Blauw.Transactions.Abstractions.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Blauw.Transactions.Infrastructure.DataAccess;

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
        
        BsonClassMap.RegisterClassMap<Transaction>();
    }
    
    public virtual IMongoCollection<Transaction> Transactions =>
        _database.GetCollection<Transaction>("Transactions");
    
    public ApplicationDbContext CreateIndexes()
    {
        CreateIndexesAsync().GetAwaiter().GetResult();
        return this;
    }
    
    Task CreateIndexesAsync()
    {
        var tasks = new List<Task>
        {
            TransactionsIndexesAsync(),
        };

        return Task.WhenAll(tasks);
    }
    
    Task TransactionsIndexesAsync()
    {
        var indexes = new List<CreateIndexModel<Transaction>>
        {
            new(Builders<Transaction>.IndexKeys.Ascending(x => x.AccountId)),
            new(Builders<Transaction>.IndexKeys.Ascending(x => x.CreatedAt)),
            new(Builders<Transaction>.IndexKeys.Ascending(x => x.ProcessedAt)),
        };

        return Transactions.Indexes.CreateManyAsync(indexes);
    }
}