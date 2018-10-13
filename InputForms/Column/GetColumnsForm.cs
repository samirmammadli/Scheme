using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.InputForms.Column
{
    public class GetColumnsForm
    {
        [Required]
        public int ProjectId { get; set; }
    }
}
