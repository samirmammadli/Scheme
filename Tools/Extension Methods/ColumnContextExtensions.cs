using Microsoft.EntityFrameworkCore;
using Scheme.Entities;
using Scheme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Tools.Extension_Methods
{
    public static class ColumnContextExtensions
    {
        private static ControllerErrorCode _code;

        public static ControllerErrorCode GetError(this DbSet<Column> columns)
        {
            return _code;
        }

        public async static Task<IEnumerable<Column>> GetColumns(this ProjectContext db, string userEmail, int projectId)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return null;
            }

            var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == projectId);

            if (project == null)
            {
                _code = ControllerErrorCode.ProjectNotFound;
                return null;
            }

            var role = await db.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.User == user && x.Project == project);

            if (role == null)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return null;
            }

            var columns = await db.Columns.AsNoTracking().Where(x => x.Project == project).ToListAsync();

            return columns;

        }
    }
}
