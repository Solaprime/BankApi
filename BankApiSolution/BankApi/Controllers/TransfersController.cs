using AutoMapper;
using BankApi.Entities;
using BankApi.Model;
using BankApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private ITransactionService _accountService;
        IMapper _mapper;

        public TransfersController(ITransactionService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        [HttpPost("CreateNewTransaction")]
        public IActionResult CreateNewTransaction([FromBody]TransactionRequestModel transactionRequest)
        {
            if (!ModelState.IsValid) return BadRequest();

            var transaction = _mapper.Map<Transaction>(transactionRequest);
            return Ok(_accountService.CreateNewTransaction(transaction));
           
        }

        [HttpPost("MakeDeposit")]
        public IActionResult MakeDeposit(string accountNumber, decimal amount, string transactionPin)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account Number must be 10-digit");

            return Ok(_accountService.MakeDeposit(accountNumber, amount, transactionPin));

           

        }




        [HttpPost("MakeWithdrawal")]
        public IActionResult MakeWithdrawal(string accountNumber, decimal amount, string transactionPin)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account Number must be 10-digit");

            return Ok(_accountService.MakeWithdrawal(accountNumber, amount, transactionPin));



        }


        [HttpPost("MakeFundsTransfer")]
        public IActionResult MakeFundsTransfer(string fromaccount, string ToAccount, decimal amount, string transactionPin)
        {
            if (!Regex.IsMatch(fromaccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$") ||!Regex.IsMatch(ToAccount, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$") ) return BadRequest("Account Number must be 10-digit");

            return Ok(_accountService.MakeFundsTransfer(fromaccount, ToAccount, amount, transactionPin));



        }

    }
}
