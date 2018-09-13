using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Entities
{
    [Table("Sprints")]
    public class Sprint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public Project Project { get; set; }
        public DateTime ExpireDate { get; set; }
        public List<Column> Columns { get; set; } = new List<Column>(); 
    }
}
