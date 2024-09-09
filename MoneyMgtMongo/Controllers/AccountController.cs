using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            if (result)
            {
                return Ok("Account Added");
            }
            return BadRequest("Something went wrong");
        }

        [HttpGet]
        [Authorize]
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
