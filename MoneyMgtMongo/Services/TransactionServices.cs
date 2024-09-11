using Microsoft.AspNetCore.DataProtection;
using MoneyMgtMongo.DB;
using MoneyMgtMongo.Interfaces;
using MoneyMgtMongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MoneyMgtMongo.Services
{
    public class TransactionServices : ITransactionServices
    {
        private readonly IMongoCollection<Users> users;
        private readonly ILogger<TransactionServices> logger;
        private readonly UserCreds creds;
        private readonly IDataProtector protector;
        public TransactionServices(MongoDBService service, ILogger<TransactionServices> logger, UserCreds creds, IDataProtectionProvider provider, IConfiguration config)
        {
            users = service.Database.GetCollection<Users>("users");
            this.logger = logger;   
            this.creds = creds;
            protector = provider.CreateProtector(config["dataEncryption:key"]);
        }

        public async Task<IEnumerable<Transactions>> GetAllTransactions()
        {
            try
            {
                var filter = Builders<Users>.Filter.Eq(x => x.id, creds.userid);
                var user = await users.Find(filter).FirstOrDefaultAsync();
                return user.transactiondetails;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<bool> AddNewTransaction(TransactionDto tranasction, bool isBank)
        {
            try
            {
                Transactions trans = new Transactions
                {
                    AccountId = tranasction.accountId,
                    transactionTime = tranasction.transactionTime,
                    amount = protector.Protect(tranasction.amount.ToString())
                };
                trans.id = ObjectId.GenerateNewId().ToString();
                var user = await users.Find(x => x.id == creds.userid).FirstOrDefaultAsync();
                if (user == null)
                {
                    logger.LogError("user not found");
                    return false;
                }
                var account = user.customAccounts.Find(x => x.id == trans.AccountId);
                if(account == null)
                {
                    logger.LogError($"{trans.AccountId} not found");
                    return false;
                }
                trans.transactionType = account.TransactionType;
                if (isBank)
                {
                    if (trans.transactionType == 1)
                    {
                        var newBank = int.Parse(protector.Unprotect(user.bankBalance)) - int.Parse(protector.Unprotect(trans.amount));
                        if (newBank < 0)
                        {
                            logger.LogInformation("Low Bank Balance");
                            return false;
                        }
                        var update = Builders<Users>.Update
                            .Push(x => x.transactiondetails, trans)
                            .Set(x => x.bankBalance, protector.Protect(newBank.ToString()));
                        await users.UpdateOneAsync(x => x.id == creds.userid, update);

                    } else
                    {
                        var newBank = int.Parse(protector.Unprotect(user.bankBalance)) + int.Parse(protector.Unprotect(trans.amount));
                        var update = Builders<Users>.Update
                            .Push(x => x.transactiondetails, trans)
                            .Set(x => x.bankBalance, protector.Protect(newBank.ToString()));
                        await users.UpdateOneAsync(x => x.id == creds.userid, update);
                    }
                }
                else
                {
                    if(trans.transactionType == 1)
                    {
                        var newCash = int.Parse(protector.Unprotect(user.cashBalance)) - int.Parse(protector.Unprotect(trans.amount));
                        if(newCash < 0)
                        {
                            logger.LogError("Low Cash Balance");
                            return false;
                        }

                        var update = Builders<Users>.Update
                            .Push(x => x.transactiondetails, trans)
                            .Set(x => x.cashBalance, protector.Protect(newCash.ToString()));
                        await users.UpdateOneAsync(x => x.id == creds.userid, update);
                    } else
                    {
                        var newCash = int.Parse(protector.Unprotect(user.cashBalance)) + int.Parse(protector.Unprotect(trans.amount));
                        var update = Builders<Users>.Update
                            .Push(x => x.transactiondetails, trans)
                            .Set(x => x.cashBalance, protector.Protect(newCash.ToString()));
                        await users.UpdateOneAsync(x => x.id == creds.userid, update);
                    }
                }
                return true;

            } catch (Exception e)
            {
                logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<BalanceDto> GetBalances()
        {
            try
            {
                var filter = Builders<Users>.Filter.Eq(x => x.id, creds.userid);
                var user = await users.Find(filter).FirstOrDefaultAsync();
                if(user == null)
                {
                    logger.LogError("user not found");
                    return null;
                }
                BalanceDto balance = new BalanceDto
                {
                    cashBalance = int.Parse(protector.Unprotect(user.cashBalance)),
                    bankBalance = int.Parse(protector.Unprotect(user.bankBalance))
                };
                return balance;
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return null;
            }
        }
    }
}
