using BankApi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Model
{
    public class GetAccountModel
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }

        public string PhoneNumber { get; set; }
    
        public string Email { get; set; }

        public decimal CurrentAccountBalance { get; set; }
        //Basicaaly an enum to show user acccount type
        public AccountType AccountType { get; set; }
        //   [RegularExpression(@"^[0-9]/d{4}$", ErrorMessage = "Pin Must  be more thaan 4 dIGITs")]

        public string AccountNumberGenrated { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }
    }
}
