using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMgtMongo.Interfaces;
using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices services;
        public AccountController(IAccountServices services)
        {
            this.services = services;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> NewAccount(Accounts acc)
        {
            var result = await services.AddNewAccount(acc);
            return Ok("Account Added");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AllAccounts(int catId)
        {
            var response = await services.GetAllAccounts(catId);
            return Ok(response);
        }
    }
}
