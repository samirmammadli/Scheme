using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Models
{
    public class NewProjectModel
    {
        [Required]
        public string ProjectName { get; set; }
        [Required]
        [EmailAddress]
        public string CreatorEmail { get; set; }
    }
}
