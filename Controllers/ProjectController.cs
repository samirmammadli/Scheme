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

            var email = User.Identity.Name;

            var project = await _db.CreateProject(email, projectName);

            if (project == null)
                return BadRequest("Wrong information!");

            return Ok(project.GetDTO(ProjectUserRole.ProjectManager));
        }

        [HttpPost("delete/project")]
        public async Task<IActionResult> DeleteProject([FromBody] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = User.Identity.Name;

            var isSuccess = await _db.DeleteProjectAsync(email, id);

            if (!isSuccess)
                return BadRequest("Not found or you do not have permission!");

            return Ok("Success!");
        }

        [HttpPost("add/user")]
        public async Task<IActionResult> AddUserToProject([FromBody] AddUserToProjectForm from)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var email = User.Identity.Name;

            var isSuccess = await _db.AddUserToProjectAsync(email, from);

            if (!isSuccess)
                return BadRequest(_db.Projects.GetError());

            return Ok("Success!");
        }
    }
}