using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ShortLink.Domain;

namespace ShortLink.Infra
{

    public class MongoContext
    {
        private readonly IMongoDatabase _database;
        private readonly string? _connectionString;
        public MongoContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MongoDbConnectionS");
            MongoClient mongoClient = new(_connectionString);
            _database = mongoClient.GetDatabase("ShortLink");
        }
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Link> Link => _database.GetCollection<Link>("Links");
    }
}
