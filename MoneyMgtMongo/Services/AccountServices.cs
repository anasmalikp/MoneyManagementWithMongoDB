using MoneyMgtMongo.DB;
using MoneyMgtMongo.Interfaces;
using MoneyMgtMongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MoneyMgtMongo.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IMongoCollection<Users> users;
        private readonly UserCreds creds;
        private readonly ILogger<AccountServices> logger;
        public AccountServices(MongoDBService services, UserCreds creds, ILogger<AccountServices> logger)
        {
            users = services.Database?.GetCollection<Users>("users");
            this.creds = creds;
            this.logger = logger;
        }

        public async Task<bool> AddNewAccount(Accounts acc)
        {
            try
            {
                acc.id = ObjectId.GenerateNewId().ToString();
                var filter = Builders<Users>.Filter.Eq(x => x.id, creds.userid);
                var update = Builders<Users>.Update
                    .Push(x => x.customAccounts, acc);
               await users.UpdateOneAsync(filter, update);

                return true;
            }catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Accounts>> GetAllAccounts()
        {
            var filter = Builders<Users>.Filter.Eq(x => x.id, creds.userid);
            var user = await users.Find(filter).FirstOrDefaultAsync();
            return user.customAccounts;
        }
    }
}
