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

namespace Scheme.Controllers
{
    [Route("api/Project")]
    [Authorize]
    public class ProjectController : Controller
    {
        private ProjectContext _db;
        public ProjectController(ProjectContext db)
        {
            _db = db;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProject([FromBody] NewProjectModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email.Equals(model.CreatorEmail, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return NotFound("User not found!");

            var project = new Project
            {
                Name = model.ProjectName,
                CreationDate = DateTime.UtcNow
            };

            var role = new Role
            {
                Type = ProjectUserRole.Master.ToString(),
                Project = project, User = user
            };

            await _db.Projects.AddAsync(project);
            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();

            var proj = _db.Projects.Include(x => x.Roles).FirstOrDefaultAsync();

            return Ok(proj);
        }
    }
}