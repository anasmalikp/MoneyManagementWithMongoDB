﻿using Microsoft.IdentityModel.Tokens;
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
        public UserServices(MongoDBService service, IConfiguration config, ILogger<UserServices> logger)
        {
            users = service.Database?.GetCollection<Users>("users");
            this.config = config;
            this.logger = logger;
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
                user.cashBalance = 0;
                user.bankBalance = 0;
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

        public async Task<string> Login(Users user)
        {
            try
            {
                var filter = Builders<Users>.Filter.Eq(x => x.email, user.email);
                var isExist = await users.Find(filter).FirstOrDefaultAsync();
                if(isExist == null)
                {
                    logger.LogError("Email not found. please Register first");
                    return null;
                }
                var isVerified = PasswordHasher.VerifyPassword(isExist.Password, user.Password);
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:key"]));
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
