using BankApi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Model
{
    public class UpdateAccountModel
    {
       
      [Key]
        public int Id { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

      
        //Basicaaly an enum to show user acccount type
 

        //Note hash and Pin can not be Saved in Plain text 

       

  
        public DateTime DateLastUpdated { get; set; }


        [Required]

        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Pin Must  be more thaan 4 dIGITs")]

        public string Pin { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 4)]
        [Compare("Pin", ErrorMessage = "Confirm Pin must match Pin")]
        public string ConfrimPin { get; set; }
    }
}
