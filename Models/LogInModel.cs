using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Models
{
    public class LogInModel
    {
        [Required(ErrorMessage = "Email field is empty!")]
        [EmailAddress(ErrorMessage = "Wrong Email address!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field is empty!")]
        public string Password { get; set; }
    }
}
