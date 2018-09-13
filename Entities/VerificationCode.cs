using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Entities
{
    public class VerificationCode
    {
        public int Id { get; set; }
        [Required]
        public int Code { get; set; }
        [Required]
        public DateTime Expires { get; set; }
        [Required]
        public User User { get; set; }
    }
}
