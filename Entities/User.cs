using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string Name { get; set; }
        public string SurnName { get; set; }
        public string Salt { get; set; }
        public string PassHash { get; set; }
        public List<Role> Roles { get; set; }
        public bool IsConfirmed { get; set; }

        public User()
        {
            Roles = new List<Role>();
        }
    }
}
