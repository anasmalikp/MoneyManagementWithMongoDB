using MoneyMgtMongo.DB;
using MoneyMgtMongo.Encryption;
using MoneyMgtMongo.Interfaces;
using MoneyMgtMongo.Models;
using MongoDB.Driver;

namespace MoneyMgtMongo.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMongoCollection<Users> users;
        public UserServices(MongoDBService service)
        {
            users = service.Database?.GetCollection<Users>("users");
        }

        public async Task<bool> Register(Users user)
        {
            try
            {
                var filter = Builders<Users>.Filter.Eq(x => x.email, user.email);
                var existingUser = await users.Find(filter).FirstOrDefaultAsync();
                if(existingUser != null)
                {
                    return false;
                }
                user.Password = PasswordHasher.HashPassword(user.Password);
                user.walletBalance = 0;

                await users.InsertOneAsync(user);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
