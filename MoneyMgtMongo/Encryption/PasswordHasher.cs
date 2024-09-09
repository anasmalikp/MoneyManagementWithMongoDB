using System.IdentityModel.Tokens.Jwt;

namespace MoneyMgtMongo.Encryption
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool VerifyPassword(string hashed, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }

        public static string TokenDecoder (string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decoded = handler.ReadJwtToken(token);
            var userid = decoded.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            return userid;
        }
    }
}
