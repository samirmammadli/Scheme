using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.InputForms.Column
{
    public class ChangeCardNameForm
    {
        [Required]
        public int ColumnId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int CardId { get; set; }

        [Required]
        public string NewName { get; set; }
    }
}
