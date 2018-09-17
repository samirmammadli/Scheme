using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Models
{
    public class RegCodeCheckForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public int Code { get; set; }
    }
}
