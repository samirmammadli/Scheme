using Microsoft.EntityFrameworkCore;
using Scheme.Entities;
using Scheme.InputForms;
using Scheme.InputForms.Sprint;
using Scheme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Tools.Extension_Methods
{
    public static class SprintContextExtensions
    {

        private static ControllerErrorCode _code;

        public static ControllerErrorCode GetError(this DbSet<Sprint> projects)
        {
            return _code;
        }

        public async static Task<Sprint> AddSprintAsync(this ProjectContext db, string userEmail, AddSprintForm form)
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

            var sprint = new Sprint
            {
                Name = form.SprintName,
                Project = role.Project,
                ExpireDate = form.ExprieDate
            };

            await db.Sprints.AddAsync(sprint);

            await db.SaveChangesAsync();

            return sprint;
        }

        public async static Task<bool> RemoveSprint(this ProjectContext db, string userEmail, DeleteSprintForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return false;
            }

            var sprint = await db.Sprints.Include(x=> x.Project).FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.Id == form.SprintId);

            if (sprint == null)
            {
                _code = ControllerErrorCode.SprintNotFound;
                return false;
            }

            var role = await db.Roles.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.User == user);

            if (role == null || role.Type != ProjectUserRole.ProjectManager)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return false;
            }

            db.Sprints.Remove(sprint);

            await db.SaveChangesAsync();

            return true;
        }

        public async static Task<bool> ChangeSprintName(this ProjectContext db, string userEmail, ChangeSprintNameForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return false;
            }

            var sprint = await db.Sprints.Include(x => x.Project).FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.Id == form.SprintId);

            if (sprint == null)
            {
                _code = ControllerErrorCode.SprintNotFound;
                return false;
            }

            var role = await db.Roles.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.User == user);

            if (role == null || role.Type != ProjectUserRole.ProjectManager)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return false;
            }

            sprint.Name = form.NewName;

            await db.SaveChangesAsync();

            return true;
        }

        public async static Task<IEnumerable<Sprint>> GetSprints(this ProjectContext db, string userEmail, GetSprintsForm form)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return null;
            }

            var role = await db.Roles.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.User == user);

            if (role == null)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return null;
            }

            var sprints = await db.Sprints.Where(x => x.Project.Id == form.ProjectId).ToListAsync();

            if (sprints == null)
            {
                _code = ControllerErrorCode.SprintNotFound;
                return null;
            }

            return sprints;
        }
    }
}
