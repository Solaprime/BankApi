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
    public class AccountsController : ControllerBase
    {
        private IAccountService _accountService;
        IMapper _mapper;
        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        //Register a New Account
        [HttpPost("RegisternewAccount")]
        public IActionResult RegisterNewAccount([FromBody]RegisterAccountModel newAccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var account = _mapper.Map<Account>(newAccount);

            return Ok(_accountService.Create(account, newAccount.Pin, newAccount.ConfrimPin));
        }

        [HttpGet("GetAccount")]
        public IActionResult GetAllAccount()
        {
            var accounts = _accountService.GetAllAccounts();
            var cleanedAccounts = _mapper.Map<IList<GetAccountModel>>(accounts);
            return Ok(cleanedAccounts);
        }


        
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_accountService.Authenticate(model.AccountNuber, model.Pin));
        }


        [HttpGet("GetAccountNumber")]
        public IActionResult  GetByAccountNumber(string AccountNumber)
        {
            if (!Regex.IsMatch(AccountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$"))
            {
                return BadRequest("Acount Number must Be 10 digits");
            }

            var account = _accountService.GetByAccountNumber(AccountNumber);
            var accountToReturn = _mapper.Map<GetAccountModel>(account);
            return Ok(accountToReturn);
        }


        [HttpGet("GetAccountById")]
        public IActionResult GetByAccountByid(int id)
        {

            var account = _accountService.GetByID(id);
   
            var accountToReturn = _mapper.Map<GetAccountModel>(account);
            return Ok(accountToReturn);
        }



        [HttpPut("UpdateAccount")]
        public IActionResult UpdateAccount([FromBody]UpdateAccountModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            var account = _mapper.Map<Account>(model);
            _accountService.Update(account, model.Pin);
            return Ok();
         
        }
    }



}


//Refactor to best Pratice flow
//Change to response to somethibg bvetter
//When there is no need to throw exception remove them
// Make Jwt Authenticatuon flow
//Make transfer for same Bank
//Make transfer for Airtime
//Make a Tranasction History Flow, Dat we save all the DataTypeb

//Fix the account.Created
//account.update in DTO

//Speicfy the Current Return tYw
