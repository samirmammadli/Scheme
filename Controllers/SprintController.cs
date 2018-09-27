using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scheme.Entities;
using Scheme.InputForms.Sprint;
using Scheme.Models;
using Scheme.OutputDataConvert;
using Scheme.Tools.Extension_Methods;

namespace Scheme.Controllers
{
    [Produces("application/json")]
    [Route("api/Sprint")]
    [Authorize]
    public class SprintController : Controller
    {
        private ProjectContext _db;

        public SprintController(ProjectContext db)
        {
            _db = db;
        }

        [Route("add")]
        public async Task<IActionResult> AddSprint([FromBody] AddSprintForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var email = User.Identity.Name;

            var sprint = await _db.AddSprintAsync(email, form);

            if (sprint == null)
                return BadRequest(_db.Sprints.GetError());

            return Ok(sprint.GetDTO());
        }

        [Route("delete")]
        public async Task<IActionResult> DeleteSprint([FromBody] DeleteSprintForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var email = User.Identity.Name;

            var isSuccess = await _db.RemoveSprint(email, form);

            if (!isSuccess)
                return BadRequest(_db.Sprints.GetError());

            return Ok();
        }

        [Route("change_name")]
        public async Task<IActionResult> ChangeName([FromBody] DeleteSprintForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var email = User.Identity.Name;

            var isSuccess = await _db.RemoveSprint(email, form);

            if (!isSuccess)
                return BadRequest(_db.Sprints.GetError());

            return Ok();
        }

        [Route("get_all")]
        public async Task<IActionResult> GetSprints([FromBody] DeleteSprintForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var email = User.Identity.Name;

            var isSuccess = await _db.RemoveSprint(email, form);

            if (!isSuccess)
                return BadRequest(_db.Sprints.GetError());

            return Ok();
        }
    }
}