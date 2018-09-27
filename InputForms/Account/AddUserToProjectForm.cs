using Scheme.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.InputForms.Account
{
    public class AddUserToProjectForm
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public ProjectUserRole Role { get; set; }
    }
}
