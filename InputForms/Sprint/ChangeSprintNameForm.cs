using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.InputForms.Sprint
{
    public class ChangeSprintNameForm
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int SprintId { get; set; }

        [Required]
        public string NewName { get; set; }
    }
}
