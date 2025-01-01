using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.Interfaces
{
    public interface ITransactionServices
    {
        Task<ApiResponse<List<HistoryDto>>> GetAllTransactions();
        Task<ApiResponse<bool>> AddNewTransaction(TransactionDto tranasction, bool isBank);
        Task<BalanceDto> GetBalances();
    }
}
