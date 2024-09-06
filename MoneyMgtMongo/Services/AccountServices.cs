using MoneyMgtMongo.DB;
using MoneyMgtMongo.Interfaces;
using MoneyMgtMongo.Models;
using MongoDB.Driver;

namespace MoneyMgtMongo.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IMongoCollection<Accounts> accounts;
        public AccountServices(MongoDBService services)
        {
            accounts = services.Database?.GetCollection<Accounts>("accounts");
        }

        public async Task<bool> AddNewAccount(string accountName, int type)
        {
            try
            {
                Accounts acc = new Accounts
                {
                    AccountName = accountName,
                    TransactionType = type
                };
                await accounts.InsertOneAsync(acc);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Accounts>> GetAllAccounts()
        {
            try
            {
                return await accounts.Find(FilterDefinition<Accounts>.Empty).ToListAsync();
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
