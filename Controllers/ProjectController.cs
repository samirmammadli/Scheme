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
using Scheme.OutputDataConvert;
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

        [HttpPost("add/project")]
        public async Task<IActionResult> CreateProject([FromBody] string projectName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var mail = User.Identity.Name;

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.Equals(mail, StringComparison.OrdinalIgnoreCase));

            var project = await _db.CreateProject(user, projectName);

            if (project == null)
                return BadRequest("Wrong information!");

            return Ok(project.AdaptForOutput());
        }

        [HttpPost("delete/project")]
        public async Task<IActionResult> DeleteProject([FromBody] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = User.Identity.Name;

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            var isSuccess = await _db.DeleteProjectAsync(user, id);

            if (!isSuccess)
                return BadRequest("Not found or you do not have permission!");

            return Ok("Success!");
        }

        [HttpPost("add/user")]
        public async Task<IActionResult> AddUserToProject([FromBody] AddUserToProjectForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = User.Identity.Name;

            var fromUser = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            var isSuccess = await _db.AddUserToProjectAsync(fromUser, form);

            if (!isSuccess)
                return BadRequest("Not found or you do not have permission!");

            return Ok("Success!");
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = User.Identity.Name;

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return BadRequest("User Not Found!");

            var roles = await _db.Roles.AsNoTracking().Include(x=> x.Project).Where(x => x.User.Id == user.Id).ToListAsync();

            if (roles == null)
                return BadRequest("No projects found!");

            var projects = roles.Select(x => x.Project);

            return Ok(projects.AdaptForOutput());
        }
    }
}