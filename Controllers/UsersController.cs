using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scheme.Entities;
using Scheme.Models;

namespace Scheme.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [RequireHttps]
    public class UsersController : Controller
    {
        ProjectContext _db;

        public UsersController(ProjectContext db)
        {
            this._db = db;
        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _db.Users.ToList();
        }

        [HttpGet("{id?}")]
        public IActionResult GetUser(int id)
        {
            User user = _db.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            if (user != null && ModelState.IsValid)
            {
                _db.Users.Add(user);
                _db.SaveChanges();
                return Ok(user);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("delete")]
        public IActionResult DeleteUser([FromBody] params int[] ids)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ids.Count() == 1 && ids[0] == 0)
            {
                _db.Users.RemoveRange(_db.Users.AsNoTracking());
                _db.SaveChanges();
                return Ok();
            }
            int deleted = 0;
            foreach (var item in ids)
            {
                var user = _db.Users.AsNoTracking().FirstOrDefault(x => x.Id == item);
                if (user != null)
                {
                    _db.Users.Remove(user);
                    deleted++;
                }
            }
            _db.SaveChanges();
            return Ok($"Deleted accounts: {deleted}");
        }

        [HttpPut]
        public IActionResult ChangeUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                _db.Users.Update(user);
                _db.SaveChanges();
                return Ok(user);
            }
            return BadRequest(ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage);
        }
    }
}