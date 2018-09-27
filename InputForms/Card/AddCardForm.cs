﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.InputForms.Column
{
    public class AddCardForm
    {
        [Required]
        public string Card { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int SprintId { get; set; }

        [Required]
        public int ColumnId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
