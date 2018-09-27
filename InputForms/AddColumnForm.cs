using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.InputForms
{
    public class AddColumnForm
    {
        [Required]
        public string ColumnName { get; set; }

        [Required]
        public int ProjectId { get; set; }
    }
}
