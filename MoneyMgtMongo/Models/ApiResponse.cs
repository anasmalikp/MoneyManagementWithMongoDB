namespace MoneyMgtMongo.Models
{
    public class ApiResponse <T>
    {
        public int? statusCode { get; set; }
        public string? message { get; set; }
        public T? data { get; set; }
    }
}
