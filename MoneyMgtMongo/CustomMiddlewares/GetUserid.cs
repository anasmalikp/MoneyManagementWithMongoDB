using MoneyMgtMongo.Encryption;
using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.CustomMiddlewares
{
    public class GetUserid : IMiddleware
    {
        private readonly UserCreds creds;
        public GetUserid(UserCreds creds)
        {
            this.creds = creds;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWithSegments("/api/User/registerusers") && context.Request.Method.ToUpperInvariant() =="POST"
                || context.Request.Path.StartsWithSegments("/api/User/loginusers") && context.Request.Method.ToUpperInvariant() == "POST"
                )
            {
                await next(context);
                return;
            }
            string bearerToken = context.Request.Headers["Authorization"];
            var token = bearerToken.Split(" ")[1];
            creds.userid = PasswordHasher.TokenDecoder(token);
            await next(context);
            return;
        }
    }
}
