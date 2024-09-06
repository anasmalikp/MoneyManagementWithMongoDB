using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.Interfaces
{
    public interface IAccountServices
    {
        Task<bool> AddNewAccount(string accountName, int type);
        Task<IEnumerable<Accounts>> GetAllAccounts();
    }
}
