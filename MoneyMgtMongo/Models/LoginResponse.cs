namespace MoneyMgtMongo.Models
{
    public class LoginResponse
    {
        public string? token { get; set; }
        public string? username { get; set; }
        public int? statusCode { get; set; }
        public string? message { get; set; }
    }
}
