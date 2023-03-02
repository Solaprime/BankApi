using BankApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Services
{
   public interface IAccountService
    {
        Task<Account> Authenticate(string AccountNumber, string pin);
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<Account> Create(Account account, string Pin, string ConfrimPin);
       

       Task<Account> GetByID(int Id);
        Task<Account> GetByAccountNumber(string AccountNUmber);


        Task Update(Account account, string pin = null);
        Task Delete(int Id);
    }
}
