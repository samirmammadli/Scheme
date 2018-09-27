using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.InputForms.Column
{
    public class ChangeColumnNameForm
    {
        [Required]
        public int ColumnId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public string NewName { get; set; }
    }
}
