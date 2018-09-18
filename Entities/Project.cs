using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Column> Columns { get; set; }
        public List<Sprint> Sprints { get; set; }
        public List<Role> Roles { get; set; }

        public Project()
        {
            Roles = new List<Role>();
            Sprints = new List<Sprint>();
            Columns = new List<Column>();
        }
    }
}
