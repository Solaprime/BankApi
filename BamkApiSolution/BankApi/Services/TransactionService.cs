using BankApi.AppContext;
using BankApi.Common;
using BankApi.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankApi.Services
{
    public class TransactionService : ITransactionService
    {
        private BankApiDbContext _dbContext;
        ILogger<TransactionService> _logger;
        //WE want ot Mirrro AppSettings to a ClaSS fLOW
        private AppSettings _settings;
        private static string _BankSettlementAccount;
        private readonly IAccountService _accountService;
        public TransactionService(BankApiDbContext dbContext, 
            ILogger<TransactionService> logger, IOptions<AppSettings> settings, IAccountService accountService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _settings = settings.Value;
            _BankSettlementAccount = _settings.BankSettlementAccount;
            _accountService = accountService;
        }
        public Response CreateNewTransaction(Transaction transaction)
        {
            Response response = new Response();
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction Created Succesffully";
            response.Data = null;
            return response;
        }

        public Response FindTransactionByDate(DateTime date)
        {
            Response response = new Response();
            var transaction = _dbContext.Transactions
                 .Where(x => x.TransactionDate == date).ToList();

            response.ResponseCode = "00";
            response.ResponseMessage = "Transaction found Successfully";
            response.Data = transaction;
            return response;
        }


        public Response MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //checj user account Number

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);

            if (authUser == null)
            {
                throw new ApplicationException("Invalid Credentials");
            }

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(_BankSettlementAccount);
                destinationAccount = _accountService.GetByAccountNumber(AccountNumber);
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //Checks if there is Update 
                if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified)&&
                    (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //So transaction is succsfull
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction Succesfull";
                    response.Data = null;
                }

                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction Failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"An Error Occcured...{ex.Message}");
            }

            //Set other Properties, You can wrap these In a Finaaly for refacto

            transaction.TransactionType = TranType.Deposit;
            transaction.TransactionSourceAccount = _BankSettlementAccount;
            transaction.TransactionDestinationAccount = AccountNumber;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"New Destination from Source=>" +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $" TO Destination Account => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                $"ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                $"Transaction Type => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $"Transaction Status => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            return response;
        }

        public Response MakeFundsTransfer(string fromAccount, string toAccount, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //checj user account Number

            var authUser = _accountService.Authenticate(fromAccount, TransactionPin);

            if (authUser == null)
            {
                throw new ApplicationException("Invalid Credentials");
            }

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(fromAccount );
                destinationAccount = _accountService.GetByAccountNumber(toAccount);
                //Update Account Balance 
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //Checks if there is Update 
                if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //So transaction is succsfull
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction Succesfull";
                    response.Data = null;
                }

                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction Failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"An Error Occcured...{ex.Message}");
            }

            //Set other Properties, You can wrap these In a Finaaly for refacto

            transaction.TransactionType = TranType.Withdrawal;
            transaction.TransactionSourceAccount = _BankSettlementAccount;
            transaction.TransactionDestinationAccount = fromAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"New Destination from Source=>" +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $" TO Destination Account => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                $"ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                $"Transaction Type => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $"Transaction Status => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            return response;
        }

        public Response MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin)
        {
            Response response = new Response();
            Account sourceAccount;
            Account destinationAccount;
            Transaction transaction = new Transaction();

            //checj user account Number

            var authUser = _accountService.Authenticate(AccountNumber, TransactionPin);

            if (authUser == null)
            {
                throw new ApplicationException("Invalid Credentials");
            }

            try
            {
                sourceAccount = _accountService.GetByAccountNumber(AccountNumber);
                destinationAccount = _accountService.GetByAccountNumber(_BankSettlementAccount);
               //Update Account Balance 
                sourceAccount.CurrentAccountBalance -= Amount;
                destinationAccount.CurrentAccountBalance += Amount;

                //Checks if there is Update 
                if ((_dbContext.Entry(sourceAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified) &&
                    (_dbContext.Entry(destinationAccount).State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                {
                    //So transaction is succsfull
                    transaction.TransactionStatus = TranStatus.Success;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Transaction Succesfull";
                    response.Data = null;
                }

                else
                {
                    transaction.TransactionStatus = TranStatus.Failed;
                    response.ResponseCode = "02";
                    response.ResponseMessage = "Transaction Failed";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError($"An Error Occcured...{ex.Message}");
            }

            //Set other Properties, You can wrap these In a Finaaly for refacto

            transaction.TransactionType = TranType.Withdrawal;
            transaction.TransactionSourceAccount = AccountNumber;
            transaction.TransactionDestinationAccount = _BankSettlementAccount;
            transaction.TransactionAmount = Amount;
            transaction.TransactionDate = DateTime.Now;
            transaction.TransactionParticulars = $"New Destination from Source=>" +
                $"{JsonConvert.SerializeObject(transaction.TransactionSourceAccount)}" +
                $" TO Destination Account => {JsonConvert.SerializeObject(transaction.TransactionDestinationAccount)}" +
                $"ON DATE => {transaction.TransactionDate} FOR AMOUNT => {JsonConvert.SerializeObject(transaction.TransactionAmount)}" +
                $"Transaction Type => {JsonConvert.SerializeObject(transaction.TransactionType)}" +
                $"Transaction Status => {JsonConvert.SerializeObject(transaction.TransactionStatus)}";

            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            return response;
        }
    }
}
