namespace MoneyMgtMongo.Models
{
    public class HistoryDto
    {
        public DateTime? transactionTime { get; set; }
        public string? amount { get; set; }
        public int? transactionType { get; set; }
        public string? transactionName { get; set; }
        public bool isBank { get; set; }
    }
}
