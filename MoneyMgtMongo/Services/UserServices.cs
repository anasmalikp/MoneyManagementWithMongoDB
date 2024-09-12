using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using MoneyMgtMongo.DB;
using MoneyMgtMongo.Encryption;
using MoneyMgtMongo.Interfaces;
using MoneyMgtMongo.Models;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoneyMgtMongo.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMongoCollection<Users> users;
        private readonly IConfiguration config;
        private readonly ILogger<UserServices> logger;
        private readonly IDataProtector protector;
        public UserServices(MongoDBService service, IConfiguration config, ILogger<UserServices> logger, IDataProtectionProvider provider)
        {
            users = service.Database?.GetCollection<Users>("users");
            this.config = config;
            this.logger = logger;
            protector = provider.CreateProtector(config["dataEncryption:key"]);
        }

        public async Task<bool> Register(RegisterDto register)
        {
            try
            {
                Users user = new Users
                {
                    email = register.email,
                    username = register.userName,
                    Password = register.password,
                };
                var filter = Builders<Users>.Filter.Eq(x => x.email, user.email);
                var existingUser = await users.Find(filter).FirstOrDefaultAsync();
                if(existingUser != null)
                {
                    return false;
                }
                user.Password = PasswordHasher.HashPassword(user.Password);
                var zero = protector.Protect(0.ToString());
                user.cashBalance = zero;
                user.bankBalance = zero;
                user.customAccounts = new List<Accounts>();
                user.transactiondetails = new List<Transactions>();

                await users.InsertOneAsync(user);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<string> Login(LoginDto creds)
        {
            try
            {

                var filter = Builders<Users>.Filter.Eq(x => x.email, creds.email);
                var isExist = await users.Find(filter).FirstOrDefaultAsync();
                if(isExist == null)
                {
                    logger.LogError("Email not found. please Register first");
                    return null;
                }
                var isVerified = PasswordHasher.VerifyPassword(isExist.Password, creds.password);
                if(isVerified)
                {
                    return GetToken(isExist);
                }
                logger.LogError("wrong password entered");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        private string GetToken (Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT_SECRET"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim (ClaimTypes.NameIdentifier, user.id)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
