using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheme.Entities;
using Scheme.InputForms;
using Scheme.InputForms.Column;
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

        //[Route("getSingle")]
        //public async Task<IActionResult> GetColumn([FromBody] GetColumnForm form)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ControllerErrorCode.WrongInputData);

        //    var email = User.Identity.Name;

        //    var columns = await _db.GetColumn(email, form);

        //    if (columns == null)
        //        return BadRequest(_db.Columns.GetError());

        //    return Ok(columns.GetDTO());
        //}

        [HttpPost]
        [Route("getall")]
        public async Task<IActionResult> GetColumns([FromBody] GetColumnsForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ControllerErrorCode.WrongInputData);

            var email = User.Identity.Name;

            var columns = await _db.GetColumns(email, form);

            if (columns == null)
                return BadRequest(_db.Columns.GetError());

            return Ok(columns.GetDTO());
        }

        //[Route("remove")]
        //public async Task<IActionResult> RemoveColumn([FromBody]RemoveColumnForm form)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ControllerErrorCode.WrongInputData);

        //    var email = User.Identity.Name;
             
        //    var isSuccess = await _db.RemoveColumn(email, form);

        //    if (!isSuccess)
        //        return BadRequest(_db.Columns.GetError());

        //    return Ok();
        //}

        //[Route("add")]
        //public async Task<IActionResult> AddColumn([FromBody]AddColumnForm form)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ControllerErrorCode.WrongInputData);

        //    var email = User.Identity.Name;

        //    var column = await _db.AddColumn(email, form);

        //    if (column == null)
        //        return BadRequest(_db.Columns.GetError());

        //    return Ok(column.GetDTO());
        //}

        //[Route("change_name")]
        //public async Task<IActionResult> ChangeName([FromBody]ChangeColumnNameForm form)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ControllerErrorCode.WrongInputData);

        //    var email = User.Identity.Name;

        //    var isSuccess = await _db.ChangeColumnName(email, form);

        //    if (!isSuccess)
        //        return BadRequest(_db.Columns.GetError());

        //    return Ok();
        //}
    }
}