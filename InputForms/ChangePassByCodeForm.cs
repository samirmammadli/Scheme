using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.InputForms
{
    public class ChangePassByCodeForm
    {
        [Required]
        public int Code { get; set; }

        [Required(ErrorMessage = "Password field is empty!")]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match the confirm password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirm field is empty!")]
        public string ConfirmPassword { get; set; }
    }
}
