using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BankApi.Entities
{
    [Table("Accounts")]
    public class Account
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

        public string AccountNumberGenrated { get; set; }



      
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }


        //We dont want to Add these for usern

        //Note hash and Pin can not be Saved in Plain text 
        [JsonIgnore]

        public byte[] PinHash { get; set; }
        [JsonIgnore]

        public byte[] PinSalt { get; set; }


        //We need to Genreate the Account Number 
        //We do that in the Comstructor

        //Let first Genrate a random
        Random rand = new Random();

        public Account()
        {
            //The code Line confirm wat it is Doing Flow

            //It seems the 9_000_000_000L + 1_000_000_000L wa used to generate a 10random Digit Number
            AccountNumberGenrated = Convert.ToString((long)Math.Floor(rand.NextDouble() * 9_000_000_000L + 1_000_000_000L));
            AccountName = $"{ FirstName} {LastName}";
        }



    }

    public enum AccountType
    {
        Savings,
        current,
        Government,
        Corporate

    }
}
