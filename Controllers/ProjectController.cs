using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheme.Entities;
using Scheme.Models;
using Scheme.Services;

namespace Scheme.Controllers
{
    [Route("api/Project")]
    [RequireHttps]
    [Authorize]
    public class ProjectController : Controller
    {
        private ProjectContext _db;
        public ProjectController(ProjectContext db)
        {
            _db = db;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProject([FromBody] string projectName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mail = User.Identity.Name;

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.Equals(mail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return NotFound("User not found!");

            var project = new Project
            {
                Name = projectName,
                CreationDate = DateTime.UtcNow
            };

            var role = new Role
            {
                Type = ProjectUserRole.Owner,
                Project = project, User = user
            };

            await _db.Projects.AddAsync(project);
            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();

            return Ok(project);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteProject([FromBody] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = User.Identity.Name;

            var master = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            var project = await _db.Projects.FirstOrDefaultAsync(x => x.Id == id);

            if (master == null || project == null)
                return BadRequest("User or Project not found!");

            if (!_db.ProjectRoleIsOwner(id, master.Id)) 
                return BadRequest("You do not have permission to remove this project!");

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();

            return Ok("Success!");
        }

        [HttpPost("addprogrammer")]
        public async Task<IActionResult> AddProgrammerToProject([FromBody] AddUserToProjectForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = User.Identity.Name;

            var master = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.Equals(form.UserEmail, StringComparison.OrdinalIgnoreCase));

            var project = await _db.Projects.FirstOrDefaultAsync(x => x.Id == form.ProjectId);

            if (master == null || project == null || user == null)
                return BadRequest("User or Project not found!");

            if (!_db.ProjectRoleIsMaster(form.ProjectId, master.Id))
                return BadRequest("You do not have permission to add a user in this project!");

            var oldRole = await _db.Roles.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.User.Id == user.Id);

            if (oldRole != null)
                _db.Roles.Remove(oldRole);

            var newRole = new Role()
            {
                Project = project,
                User = user,
                Type = ProjectUserRole.Programmer
            };

            await _db.Roles.AddAsync(newRole);

            await _db.SaveChangesAsync();

            return Ok("Success!");
        }

        [HttpPost("addmaster")]
        public async Task<IActionResult> AddMasterToProject([FromBody] AddUserToProjectForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = User.Identity.Name;

            var master = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.Equals(form.UserEmail, StringComparison.OrdinalIgnoreCase));

            var project = await _db.Projects.FirstOrDefaultAsync(x => x.Id == form.ProjectId);

            if (master == null || project == null || user == null)
                return BadRequest("User or Project not found!");

            if (!_db.ProjectRoleIsOwner(form.ProjectId, master.Id))
                return BadRequest("You do not have permission to add a user in this project!");

            var oldRole = await _db.Roles.FirstOrDefaultAsync(x => x.Project.Id == form.ProjectId && x.User.Id == user.Id);

            if (oldRole != null)
                _db.Roles.Remove(oldRole);

            var newRole = new Role()
            {
                Project = project,
                User = user,
                Type = ProjectUserRole.Master
            };

            await _db.Roles.AddAsync(newRole);

            await _db.SaveChangesAsync();

            return Ok("Success!");
        }
    }
}