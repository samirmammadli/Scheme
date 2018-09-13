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
        public static bool CheckRole(this ProjectContext context, int ProjectId, int userId, string roleName)
        {
            var role = context.Roles.AsNoTracking().FirstOrDefault(x => x.Id == userId && x.Project.Id == ProjectId);
            if (role == null) return false;
            if (role.Name == roleName) return true;
            return false;
        }
    }
}
