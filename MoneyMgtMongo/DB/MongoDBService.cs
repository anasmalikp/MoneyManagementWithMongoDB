using MongoDB.Driver;

namespace MoneyMgtMongo.DB
{
    public class MongoDBService
    {
        private readonly IConfiguration config;
        private readonly IMongoDatabase? database;
        public MongoDBService(IConfiguration config)
        {
            this.config = config;
            var connection = config.GetConnectionString("DefaultConnection");
            var mongoUrl = MongoUrl.Create(connection);
            var mongoClient = new MongoClient(mongoUrl);
            database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        public IMongoDatabase? Database => database;
    }
}
