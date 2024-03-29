﻿using GrainInterface;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System;
using System.Threading.Tasks;

namespace WebClient.Controllers
{
    [Route("bank/accounts")]
    [ApiController]
    public class BankAccountController : Controller
    {
        IClusterClient _clusterClient = null;
        public BankAccountController(IClusterClient clusterClient) 
        {
            _clusterClient = clusterClient;
        }

        [Route("{accountId:Guid}")]
        [HttpGet]
        public async Task<decimal> GetBalance(Guid accountId) 
        {
            var grain = _clusterClient.GetGrain<IBankAccountGrain>(accountId);
            var balance = await grain.Balance();

            return balance;
        }

        [HttpPost]
        public IActionResult CreateNew()
        {
            var accountId = Guid.NewGuid();
            var grain = _clusterClient.GetGrain<IBankAccountGrain>(accountId);

            return Created("/bank/accounts/{accountId}", new { accountId = accountId });
        }

        [Route("deposit")]
        [HttpPost]
        public async Task<IActionResult> Deposit([FromBody] MoneyRequest request) 
        {
            var grain = _clusterClient.GetGrain<IBankAccountGrain>(request.AccountId);
            await grain.Deposit(request.Money);

            return NoContent();
        }

        [Route("withdraw")]
        [HttpPost]
        public async Task<IActionResult> Withdraw([FromBody] MoneyRequest request)
        {
            var grain = _clusterClient.GetGrain<IBankAccountGrain>(request.AccountId);
            await grain.Withdraw(request.Money);

            return NoContent();
        }
    }

    public class MoneyRequest 
    {
        public Guid AccountId { get; set; }
        public decimal Money { get; set; }
    }
}
