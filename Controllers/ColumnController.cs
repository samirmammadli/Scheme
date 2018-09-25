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
    [RequireHttps]
    [Route("api/Column")]
    [Authorize]
    public class ColumnController : Controller
    {
        private ProjectContext _db;

        public ColumnController(ProjectContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> GetColumns(int projectId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var email = User.Identity.Name;
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                return BadRequest(ControllerErrorCode.UserNotFound);

            var columns = _db.Columns.AsNoTracking().Where(x => x.Project.Id == projectId);

        }
    }
}