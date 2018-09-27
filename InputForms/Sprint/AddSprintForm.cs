using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.InputForms
{
    public class AddSprintForm
    {
        [Required]
        public string SprintName { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public DateTime ExprieDate { get; set; }
    }
}
