using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApi.Model
{
    public class AuthenticationModel
    {
        [Required]
        [RegularExpression(@"^[0][1-9]\d{9}$|^[1-9]\d{9}$", ErrorMessage = "Account Number Must  be 10 ddIGITs")]
        public string AccountNuber { get; set; }
        [Required]
        public string Pin { get; set; }
    }
}
