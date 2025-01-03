﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyMgtMongo.Interfaces;
using MoneyMgtMongo.Models;

namespace MoneyMgtMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionServices services;
        public TransactionController(ITransactionServices services)
        {
            this.services = services;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTransactions()
        {
            var response = await services.GetAllTransactions();
            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTransaction(TransactionDto trans, bool isBank)
        {
            var response = await services.AddNewTransaction(trans, isBank);
            return Ok(response);
        }

        [HttpGet("get_balances")]
        [Authorize]
        public async Task<IActionResult> GetAllBalances()
        {
            var response = await services.GetBalances();
            if(response == null)
            {
                return BadRequest("something went wrong");
            }
            return Ok(response);
        }
    }
}
