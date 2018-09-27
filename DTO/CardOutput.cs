using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.DTO
{
    public class CardOutput
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ProjectId { get; set; }
        public int SprintId { get; set; }
        public int ColumnId { get; set; }
    }
}
