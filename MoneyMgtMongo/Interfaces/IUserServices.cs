using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.Interfaces
{
    public interface IUserServices
    {
        Task<bool> Register(RegisterDto register);
        Task<string> Login(LoginDto creds);
    }
}
