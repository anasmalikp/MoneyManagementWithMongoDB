﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("registerusers")]
        public async Task<IActionResult> NewUser(RegisterDto register)
        {
            var response = await services.Register(register);
            if (response)
            {
                return Ok("User Added");
            }
            return BadRequest("something went wrong");
        }
        [HttpPost("loginusers")]
        public async Task<IActionResult> UserLogin(LoginDto user)
        {
            var response = await services.Login(user);
            if(response == null)
            {
                return NotFound("something went wrong");
            }
            return Ok(response);
        }
    }
}
