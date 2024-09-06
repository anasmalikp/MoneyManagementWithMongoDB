using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyMgtMongo.Interfaces;
using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices services;
        public UserController(IUserServices services)
        {
            this.services = services;
        }

        [HttpPost]
        public async Task<IActionResult> NewUser(Users user)
        {
            var response = await services.Register(user);
            if (response)
            {
                return Ok("User Added");
            }
            return BadRequest("something went wrong");
        }
    }
}
