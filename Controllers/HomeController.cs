using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Scheme.Controllers
{
    public class HomeController : Controller
    {
        public async Task Index()
        {
            await Response.WriteAsync("Hello World!");
        }
    }
}