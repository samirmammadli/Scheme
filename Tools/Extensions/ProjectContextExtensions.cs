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
        public static bool ProjectRoleIsOwner(this ProjectContext context, int ProjectId, int userId)
        {
            var role = context.Roles.AsNoTracking().FirstOrDefault(x => x.User.Id == userId && x.Project.Id == ProjectId);
            if (role == null) return false;
            if (role.Type == ProjectUserRole.Owner) return true;
            return false;
        }

        public static bool ProjectRoleIsMaster(this ProjectContext context, int ProjectId, int userId)
        {
            var role = context.Roles.AsNoTracking().FirstOrDefault(x => x.User.Id == userId && x.Project.Id == ProjectId);
            if (role == null) return false;
            var roleNumber = (int)role.Type;
            if (roleNumber < 2) return true;
            return false;
        }
    }
}
