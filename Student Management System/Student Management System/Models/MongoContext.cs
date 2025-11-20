using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace StudentManagement.Models
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IConfiguration config)
        {
            var conn = config.GetConnectionString("MongoDB") ?? "mongodb://localhost:27017";
            var client = new MongoClient(conn);
            _database = client.GetDatabase("StudentDB");
        }

        public IMongoCollection<Student> Students => _database.GetCollection<Student>("Students");
    }
}