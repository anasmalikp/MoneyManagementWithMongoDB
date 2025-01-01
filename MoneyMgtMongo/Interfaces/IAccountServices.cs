using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.Interfaces
{
    public interface IAccountServices
    {
        Task<ApiResponse<bool>> AddNewAccount(Accounts acc);
        Task<ApiResponse<List<Accounts>>> GetAllAccounts(int catId);
    }
}
