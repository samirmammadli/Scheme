using Microsoft.EntityFrameworkCore;
using Scheme.Entities;
using Scheme.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheme.Tools
{

    //TODO:
    public class NotificationDTO
    {
        public string Message { get; set; }
        private History _action;
        private ProjectContext _db;

        public NotificationDTO(History action, ProjectContext db)
        {
            _action = action;
            _db = db;
        }
    }
}
