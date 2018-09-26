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
        private static ControllerErrorCode _code;

        public static ControllerErrorCode GetError(this DbSet<Project> projects)
        {
            return _code;
        }

        public static async Task<bool> AddUserToProjectAsync(this ProjectContext db, string fromEmail, AddUserToProjectForm form)
        {
            var fromUser = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(fromEmail, StringComparison.OrdinalIgnoreCase));

            var toUser = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(form.UserEmail, StringComparison.OrdinalIgnoreCase));

            if (fromUser == null || toUser == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return false;
            }

            var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == form.ProjectId);

            if (project == null)
            {
                _code = ControllerErrorCode.ProjectNotFound;
                return false;
            }

            var fromUserRole = db.Roles.AsNoTracking().FirstOrDefault(x => x.User.Id == fromUser.Id && x.Project.Id == project.Id);

            var toUserRole = db.Roles.FirstOrDefault(x => x.Project.Id == form.ProjectId && x.User.Id == toUser.Id);

            if (fromUserRole == null || fromUserRole.Type != ProjectUserRole.ProjectManager)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return false;
            }

            if (toUserRole != null)
                    db.Remove(toUserRole);

            var newRole = new Role()
            {
                Project = project,
                User = toUser,
                Type = form.Role
            };

            await db.Roles.AddAsync(newRole);

            await db.SaveChangesAsync();

            return true;
        }

        public static async Task<bool> DeleteProjectAsync(this ProjectContext db, string userEmail, int projectId)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return false;
            }

            var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == projectId);

            if (project == null)
            {
                _code = ControllerErrorCode.ProjectNotFound;
                return false;
            }

            var role = await db.Roles.AsNoTracking().FirstOrDefaultAsync(x=> x.Project.Id == projectId && x.User.Id == user.Id);

            if (role == null || role.Type != ProjectUserRole.ProjectManager)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return false;
            }
            
            db.Projects.Remove(project);

            await db.SaveChangesAsync();

            return true;
        }

        public static async Task<Project> CreateProject(this ProjectContext db, string userEmail, string projectName)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return null;
            }

            var project = new Project
            {
                Name = projectName,
                CreationDate = DateTime.UtcNow
            };

            var role = new Role
            {
                Type = ProjectUserRole.ProjectManager,
                Project = project,
                User = user
            };

            await db.Projects.AddAsync(project);

            await db.Roles.AddAsync(role);

            await db.SaveChangesAsync();

            return project;
        }

        public static async Task<IEnumerable<Project>> GetProjects(this ProjectContext db, string userEmail)
        {
            var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                _code = ControllerErrorCode.UserNotFound;
                return null;
            }

            var roles = await db.Roles.AsNoTracking().Include(x => x.Project).Where(x => x.User.Id == user.Id).ToListAsync();

            if (roles == null)
            {
                _code = ControllerErrorCode.PermissionsDenied;
                return null;
            }

            var projects = roles.Select(x => x.Project);

            return projects;
        }
    }
}
