using BankApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Services
{
   public interface IAccountService
    {
        Account Authenticate(string AccountNumber, string pin);
        IEnumerable<Account> GetAllAccounts();
        Account Create(Account account, string Pin, string ConfrimPin);
        void Update(Account account, string pin = null);
        void Delete(int Id);

        Account GetByID(int Id);
        Account GetByAccountNumber(string AccountNUmber);

    }
}
