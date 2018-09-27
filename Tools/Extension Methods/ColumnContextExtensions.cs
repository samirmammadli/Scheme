using Microsoft.EntityFrameworkCore;
using Scheme.Entities;
using Scheme.InputForms;
using Scheme.InputForms.Column;
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

        public async static Task<IEnumerable<Column>> GetColumns(this ProjectContext db, string userEmail, GetColumnsForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return null;
            }

            var role = await db.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.User == user && x.Project.Id == form.ProjectId);

            if (role == null)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return null;
            }

            var columns = await db.Columns.Where(x => x.Sprint.Id == form.SprintId).ToListAsync();

            if (columns == null)
            {
                _code = ControllerErrorCode.ColumnNotFound;
                return null;
            }

            return columns;
        }

        public async static Task<Column> GetColumn(this ProjectContext db, string userEmail, GetCardForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return null;
            }

            var role = await db.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.User == user && x.Project.Id == form.ProjectId);

            if (role == null)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return null;
            }

            var column = await db.Columns.FirstOrDefaultAsync(x => x.Sprint.Id == form.SprintId);

            if (column == null)
            {
                _code = ControllerErrorCode.ColumnNotFound;
                return null;
            }

            return column;
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

            var sprint = await db.Sprints.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.Id == form.SprintId);

            if (sprint == null)
            {
                _code = ControllerErrorCode.SprintNotFound;
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
                Sprint = sprint
            };

            await db.Columns.AddAsync(column);

            await db.SaveChangesAsync();

            return column;
        }

        public async static Task<bool> ChangeColumnName(this ProjectContext db, string userEmail, ChangeColumnNameForm form)
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

            column.Name = form.NewName;

            await db.SaveChangesAsync();

            return true;
        }
    }
}
