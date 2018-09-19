using Microsoft.EntityFrameworkCore;
using Scheme.Entities;
using Scheme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Services
{
    public static class ProjectContextExtensions
    {
        public static bool CheckRole(this ProjectContext context, int ProjectId, int userId, ProjectUserRole roleName)
        {
            var role = context.Roles.AsNoTracking().FirstOrDefault(x => x.User.Id == userId && x.Project.Id == ProjectId);
            if (role == null) return false;
            if (role.Type.Equals(roleName.ToString(), StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}
