using Microsoft.EntityFrameworkCore;
using Scheme.Entities;
using Scheme.InputForms;
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

        public async static Task<bool> RemoveColumn(this ProjectContext db, string userEmail, RemoveColumnForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return false;
            }

            var column = await db.Columns.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.Id == form.ColumnId);

            if (column == null)
            {
                _code = ControllerErrorCode.ColumnNotFound;
                return false;
            }

            var role = await db.Roles.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.User == user);

            if (role == null || role.Type != ProjectUserRole.ProjectManager)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return false;
            }

            db.Columns.Remove(column);

            await db.SaveChangesAsync();

            return true;
        }

        public async static Task<Column> AddColumn(this ProjectContext db, string userEmail, AddColumnForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return null;
            }

            var role = await db.Roles.Include(y => y.Project).FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.User == user);

            if (role == null || role.Type != ProjectUserRole.ProjectManager)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return null;
            }

            var column = new Column
            {
                Name = form.ColumnName,
                 Project = role.Project,
                 
            }

            await db.SaveChangesAsync();

            return true;
        }

    }
}
