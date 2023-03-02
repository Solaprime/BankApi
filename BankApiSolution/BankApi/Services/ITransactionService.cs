using BankApi.Common;
using BankApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankApi.Services
{
   public interface ITransactionService
    {
        Task<Response> CreateNewTransaction(Transaction transaction);
        Task<Response> FindTransactionByDate(DateTime date);

        //Refacto all these string values to a Response 
        Task<Response> MakeDeposit(string AccountNumber, decimal Amount, string TransactionPin);
        Task<Response> MakeWithdrawal(string AccountNumber, decimal Amount, string TransactionPin);
        Task<Response>  MakeFundsTransfer(string fromAccount, string toAccount, decimal Amount, string TransactionPin);
    }
}
