using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ShortLink.Domain;

namespace ShortLink.Infra;

public class MongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("MongoDbConnectionS")
            ?? configuration["MongoDbConnectionS"];

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "MongoDB connection string is missing. Configure ConnectionStrings:MongoDbConnectionS.");

        var mongoClient = new MongoClient(connectionString);
        _database = mongoClient.GetDatabase("ShortLink");
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IMongoCollection<Link> Links => _database.GetCollection<Link>("Links");
    public IMongoCollection<Log> Logs => _database.GetCollection<Log>("Logs");
}
