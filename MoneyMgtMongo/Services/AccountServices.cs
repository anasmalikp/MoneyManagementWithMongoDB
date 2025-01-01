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

        public async Task<ApiResponse<bool>> AddNewAccount(Accounts acc)
        {
            try
            {
                acc.id = ObjectId.GenerateNewId().ToString();
                var filter = Builders<Users>.Filter.Eq(x => x.id, creds.userid);
                var update = Builders<Users>.Update
                    .Push(x => x.customAccounts, acc);
               await users.UpdateOneAsync(filter, update);

                return new ApiResponse<bool>
                {
                    statusCode = 201,
                    message = "Account Added Succeddfully",
                    data = true
                };
            }catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return new ApiResponse<bool>
                {
                    statusCode = 500,
                    message = "Internal Server Error",
                    data = false
                };
            }
        }

        public async Task<ApiResponse<List<Accounts>>> GetAllAccounts(int catId)
        {
            var filter = Builders<Users>.Filter.And(
                Builders<Users>.Filter.Eq(x => x.id, creds.userid),
                Builders<Users>.Filter.ElemMatch(x => x.customAccounts, accounts => accounts.TransactionType == catId)
            );
            var user = await users.Find(filter).FirstOrDefaultAsync();
            var accounts =  user?.customAccounts.Where(accounts => accounts.TransactionType == catId).ToList();
            return new ApiResponse<List<Accounts>>
            {
                statusCode = 200,
                message = "Accounts Fetched Successfully",
                data = accounts
            };
        }
    }
}
