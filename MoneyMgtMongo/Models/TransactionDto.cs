namespace MoneyMgtMongo.Models
{
    public class TransactionDto
    {
        public string accountId { get; set; }
        public DateTime transactionTime { get; set; }
        public int amount { get; set; }
    }
}
