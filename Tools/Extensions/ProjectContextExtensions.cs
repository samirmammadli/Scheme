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
        public static async Task<bool> AddUserToProjectAsync(this ProjectContext db, User fromUser, AddUserToProjectForm form)
        {
            var toUser = await db.Users.FirstOrDefaultAsync(x => x.Email.Equals(form.UserEmail, StringComparison.OrdinalIgnoreCase));

            var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == form.ProjectId);

            if (fromUser == null || project == null || toUser == null)
                return false;

            var fromUserRole = db.Roles.AsNoTracking().FirstOrDefault(x => x.User.Id == fromUser.Id && x.Project.Id == project.Id);

            var toUserRole = db.Roles.FirstOrDefault(x => x.Project.Id == form.ProjectId && x.User.Id == toUser.Id);

            if (fromUserRole == null || fromUserRole.Type >= form.Role)
                return false;

            if (toUserRole != null)
            {
                if (fromUserRole.Type >= toUserRole.Type)
                    return false;
                else
                    db.Remove(toUserRole);
            }

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

        public static async Task<bool> DeleteProjectAsync(this ProjectContext db, User user, int projectId)
        {
            if (user == null)
                return false;

            var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == projectId);

            if (project == null)
                return false;

            var role = await db.Roles.AsNoTracking().FirstOrDefaultAsync(x=> x.Project.Id == projectId && x.User.Id == user.Id);

            if (role == null)
                return false;

            if (role.Type != ProjectUserRole.Owner)
                return false;
            
            db.Projects.Remove(project);

            await db.SaveChangesAsync();

            return true;
        }

        public static async Task<Project> CreateProject(this ProjectContext db, User user, string projectName)
        {
            if (user == null)
                return null;

            var project = new Project
            {
                Name = projectName,
                CreationDate = DateTime.UtcNow
            };

            var role = new Role
            {
                Type = ProjectUserRole.Owner,
                Project = project,
                User = user
            };

            await db.Projects.AddAsync(project);

            await db.Roles.AddAsync(role);

            await db.SaveChangesAsync();

            return project;
        }
    }
}
