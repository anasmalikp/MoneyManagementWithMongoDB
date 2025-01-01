using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using MoneyMgtMongo.CustomMiddlewares;
using MoneyMgtMongo.DB;
using MoneyMgtMongo.Interfaces;
using MoneyMgtMongo.Models;
using MoneyMgtMongo.Services;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddScoped<IAccountServices, AccountServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<UserCreds>();
builder.Services.AddScoped<GetUserid>();
builder.Services.AddScoped<ITransactionServices, TransactionServices>();
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"])),
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
    };
});
builder.Services.AddAuthorization();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var redisConfig = builder.Configuration.GetSection("Redis");
var redisConnection = ConnectionMultiplexer.Connect(
            new ConfigurationOptions
            {
                EndPoints = { { "redis-17819.crce179.ap-south-1-1.ec2.redns.redis-cloud.com", 17819 } },
                User = builder.Configuration["redis_user"],
                Password = builder.Configuration["redis_password"]
            }
        );

builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(redisConnection, "dataProtection_keys")
    .SetApplicationName("MoneyMgtMongo");

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*app.UseHttpsRedirection();*/
app.UseCors("ReactPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GetUserid>();
app.MapControllers();

app.Run();
