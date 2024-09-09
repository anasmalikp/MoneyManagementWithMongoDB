using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.Interfaces
{
    public interface IAccountServices
    {
        Task<bool> AddNewAccount(Accounts acc);
        Task<IEnumerable<Accounts>> GetAllAccounts();
    }
}
