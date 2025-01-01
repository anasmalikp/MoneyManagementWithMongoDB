using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.Interfaces
{
    public interface IUserServices
    {
        Task<ApiResponse<bool>> Register(RegisterDto register);
        Task<LoginResponse> Login(LoginDto creds);
    }
}
