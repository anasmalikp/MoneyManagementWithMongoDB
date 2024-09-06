using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyMgtMongo.Interfaces;

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
        public async Task<IActionResult> NewAccount(string accountName, int type)
        {
            var result = await services.AddNewAccount(accountName, type);
            if (result)
            {
                return Ok("Account Added");
            }
            return BadRequest("Something went wrong");
        }

        [HttpGet]
        public async Task<IActionResult> AllAccounts()
        {
            var response = await services.GetAllAccounts();
            if(response != null)
            {
                return Ok(response);
            }
            return BadRequest("Something went wrong");
        }
    }
}
