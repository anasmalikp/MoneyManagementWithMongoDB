using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.Interfaces
{
    public interface IUserServices
    {
        Task<bool> Register(Users user);
        Task<string> Login(Users user);
    }
}
