using BankApi.AppContext;
using BankApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApi.Services
{
    public class AccountService : IAccountService
    {
        private BankApiDbContext _dbContext;
        public AccountService(BankApiDbContext context)
        {
            _dbContext = context;
        }
        public async Task<Account> Authenticate(string AccountNumber, string pin)
        {
            var account = await  _dbContext.Accounts.Where(x => x.AccountNumberGenrated == AccountNumber).SingleOrDefaultAsync();
            if (account == null)
            {
                return null;
            }
            if (!VerifyPinHash(pin, account.PinHash, account.PinSalt))
            {
                return null;
            }

            //We need to Autrhenticate
            return account;
        }

        private  static bool VerifyPinHash(string pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrWhiteSpace(pin))
            {
                
                throw new ArgumentNullException("pin");
            }
            using (var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pin));
                for (int i = 0; i < computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //You can lET THESE TAKE and Object
        //den pass model Validation on the OBJECT 
        public  async Task<Account> Create(Account account, string Pin, string ConfrimPin)
        {
            //This is to create a new Account

            //Refacto to use Asp.Net identity and Jwt to Login
            //
            if (await _dbContext.Accounts.AnyAsync(x=>x.Email == account.Email))
            {
                throw new ApplicationException("Alcount with these eMAIL AL" +
                    "already Exist");
            }
            if (!Pin.Equals(ConfrimPin))
            {
                throw new ArgumentException("Pin do not Match", "Pin");
            }

            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt);
            account.PinHash = pinHash;
            account.PinSalt = pinSalt;

         //   account.DateCreated = new DateTimeOffset().UtcDateTime.Now;

            //
           await _dbContext.Accounts.AddAsync(account);
          await  _dbContext.SaveChangesAsync();

            return account;

        }

        private static   void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
           using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }

        public async Task Delete(int Id)
        {
            var account = await _dbContext.Accounts.FindAsync(Id);
            if (account != null)
            {
                _dbContext.Accounts.Remove(account);
             await    _dbContext.SaveChangesAsync();
            }
        }

        public async  Task<IEnumerable<Account>> GetAllAccounts()
        {
            return  await _dbContext.Accounts.ToListAsync();
        }

        public  async Task<Account> GetByAccountNumber(string AccountNUmber)
        {
            var account = await _dbContext.Accounts.Where(x => x.AccountNumberGenrated == AccountNUmber).FirstOrDefaultAsync();
            if (account == null)
            {
                return null;
            }

            return account;
        }

        public async  Task<Account> GetByID(int Id)
        {
            var account = await  _dbContext.Accounts.Where(x=> x.Id == Id ).FirstOrDefaultAsync();
            if (account == null)
            {
                return null;
            }
            return account;
        }


        //We want to allow the user to
        //change email, Pin, PhoneNumber 

        public async Task Update(Account account, string pin = null)
        {
            //Update an Accpunt Flow
            var accountToBeUpdated = _dbContext.Accounts.Where(x => x.Email == account.Email).SingleOrDefault();
            if (accountToBeUpdated == null)
            {
                throw new ApplicationException("Account is Null");

            }
            //Email Update 
            //if (!string.IsNullOrWhiteSpace(account.Email))
            //{
            //    //Check if the Email u want to change too has
            //    //Been taken 
            //    if (_dbContext.Accounts.Any(x=>x.Email == account.Email))
            //    {
            //        throw new ApplicationException("Account has been taken");
            //    }
            //    accountToBeUpdated.Email = account.Email;
            //}


            //

            //Phone number Update 
            if (!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                //Check if the Email u want to change too has
                //Been taken 
                if (_dbContext.Accounts.Any(x => x.PhoneNumber == account.PhoneNumber))
                {
                    throw new ApplicationException("PhoneNumber  has been taken");
                }
                accountToBeUpdated.PhoneNumber = account.PhoneNumber;
            }

            //Pin Update
            if (!string.IsNullOrWhiteSpace(pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(pin, out pinHash, out pinSalt);

                accountToBeUpdated.PinHash = pinHash;
                accountToBeUpdated.PinSalt = pinSalt;

                
            }
            accountToBeUpdated.DateLastUpdated = DateTime.Now;


            //It seems UpdateAsync does not exist 
            _dbContext.Accounts.Update(accountToBeUpdated);
          await   _dbContext.SaveChangesAsync();
        }
    }
}



//Refacto to use Asp.Net identity and Jwt to Login
//