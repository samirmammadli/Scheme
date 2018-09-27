using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.InputForms.Account
{
    public class RegistrationForm
    {
        [Required(ErrorMessage = "Email field is empty!")]
        [EmailAddress(ErrorMessage = "Wrong Email address!")]
            public string Email { get; set; }

        [Required(ErrorMessage = "Name field is empty!")]
            public string Name { get; set; }

        [Required(ErrorMessage = "SurnName field is empty!")]
            public string Surname { get; set; }

        [Required(ErrorMessage = "Password field is empty!")]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match the confirm password!")]
        [DataType(DataType.Password)]
            public string Password { get; set; }

        [Required(ErrorMessage = "Password confirm field is empty!")]
        [DataType(DataType.Password)]
            public string ConfirmPassword { get; set; }
    }
}
