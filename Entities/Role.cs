using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Type { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public Project Project { get; set; } 
    }
}
