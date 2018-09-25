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
using Scheme.Tools.Extension_Methods;

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

            var columns = await _db.GetColumns(email, projectId);

            if (columns == null)
                return BadRequest(_db.Columns.GetError());

            return Ok(columns);
        }
    }
}