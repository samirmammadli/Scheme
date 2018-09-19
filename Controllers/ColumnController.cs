using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scheme.Entities;

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
    }
}