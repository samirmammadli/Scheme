﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheme.Entities;
using Scheme.InputForms;
using Scheme.Models;
using Scheme.OutputDataConvert;
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

        [Route("get_all")]
        public async Task<IActionResult> GetColumns([FromBody]int projectId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var email = User.Identity.Name;

            var columns = await _db.GetColumns(email, projectId);

            if (columns == null)
                return BadRequest(_db.Columns.GetError());

            return Ok(columns.GetDTO());
        }

        [Route("remove")]
        public async Task<IActionResult> RemoveColumn([FromBody]RemoveColumnForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var email = User.Identity.Name;

            var isSuccess = await _db.RemoveColumn(email, form);

            if (!isSuccess)
                return BadRequest(_db.Columns.GetError());

            return Ok();
        }

        [Route("add")]
        public async Task<IActionResult> AddColumn([FromBody]AddColumnForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var email = User.Identity.Name;

           // var isSuccess = await _db.RemoveColumn(email, columnForm);

            //if (!isSuccess)
            //    return BadRequest(_db.Columns.GetError());

            return Ok();
        }
    }
}