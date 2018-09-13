using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Scheme.Controllers
{
    [Route("api/values")]
    public class ValuesController : Controller
    {
        
        [Route("getlogin")]
        public IActionResult GetLogin()
        {
            string ident = User.Identity.Name;
            var claims = User.Claims;
            string response = "";
            foreach (var item in claims)
            {
                response += item.Value + "  ";
            }
            return Ok(ident + "\n" + response);//($"Ваш логин: {User.Identity.Name}");
        }
        public IActionResult Update()
        {
            return Ok();
        }

        [Authorize(Roles = "admin")]
        [Route("getrole")]
        public IActionResult GetRole()
        {
            return Ok("Ваша роль: администратор");
        }
    }
}