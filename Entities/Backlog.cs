using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Entities
{
    public class Backlog
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public short ColumnType { get; set; }

        public Project Project { get; set; }

        public Sprint Sprint { get; set; }

        public List<Card> Cards { get; set; }

        public Backlog()
        {
            Cards = new List<Card>();
        }
    }
}
