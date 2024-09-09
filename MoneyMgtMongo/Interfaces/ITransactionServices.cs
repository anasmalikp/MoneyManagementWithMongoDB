using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.Interfaces
{
    public interface ITransactionServices
    {
        Task<IEnumerable<Transactions>> GetAllTransactions();
        Task<bool> AddNewTransaction(Transactions trans, bool isBank);
    }
}
