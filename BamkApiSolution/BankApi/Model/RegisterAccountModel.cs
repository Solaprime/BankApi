using BankApi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Model
{
    public class RegisterAccountModel
    {

      
        public string FirstName { get; set; }
        public string LastName { get; set; }
      //  public string AccountName { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

      //  public decimal CurrentAccountBalance { get; set; }
        //Basicaaly an enum to show user acccount type
        public AccountType AccountType { get; set; }

      //  public string AccountNumberGenrated { get; set; }


        //Note hash and Pin can not be Saved in Plain text 

        //public byte[] PinHash { get; set; }
        //public byte[] PinSalt { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }

        //We wrote a regix to make sure it is A 4pin Pasword 
        [Required]
      
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin Must  be more thaan 4 dIGITs")]

        public string Pin { get; set; }
        [Required]
        [StringLength(4, MinimumLength =4)]
        [Compare("Pin", ErrorMessage = "Confirm Pin must match Pin")]
        public string ConfrimPin { get; set; }
    }
}
