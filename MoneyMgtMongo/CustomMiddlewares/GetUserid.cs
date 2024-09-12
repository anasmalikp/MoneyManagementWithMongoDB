using MoneyMgtMongo.Encryption;
using MoneyMgtMongo.Models;
using System.Timers;

namespace MoneyMgtMongo.CustomMiddlewares
{
    public class GetUserid : IMiddleware
    {
        private readonly UserCreds creds;
        ILogger<GetUserid> logger;
        public GetUserid(UserCreds creds, ILogger<GetUserid> logger)
        {
            this.creds = creds;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                logger.LogInformation(context.Request.Path);
                if (context.Request.Path.StartsWithSegments("/api/User/registerusers") && context.Request.Method.ToUpperInvariant() == "POST"
                    || context.Request.Path.StartsWithSegments("/api/User/loginusers") && context.Request.Method.ToUpperInvariant() == "POST"
                    || context.Request.Path.StartsWithSegments("/swagger")
                    )
                {
                    await next(context);
                    return;
                }

                logger.LogInformation($"headers = {context.Request.Path}");

                string bearerToken = context.Request.Headers["Authorization"];
                if(bearerToken != null )
                {
                    var token = bearerToken?.Split(" ")[1];
                    creds.userid = PasswordHasher.TokenDecoder(token);
                    await next(context);
                    return;
                }
                await next(context);
                return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                await next(context);
                return;
            }
        }
    }
}
