﻿using System;
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
                Type = ProjectUserRole.Master.ToString(),
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

            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            var project = await _db.Projects.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null || project == null)
                return BadRequest("User or Project not found!");

            if (!_db.CheckRole(id, user.Id, ProjectUserRole.Master)) 
                return BadRequest("You do not have permission to remove this project!");

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();

            return Ok("Success!");
        }
    }
}