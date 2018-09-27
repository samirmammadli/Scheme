using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.InputForms.Account
{
    public class EmailInputForm
    {
        [Required(ErrorMessage = "Email field is empty!")]
        [EmailAddress(ErrorMessage = "Wrong Email address!")]
        public string Email { get; set; }
    }
}
