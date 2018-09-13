using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scheme.Entities;
using Scheme.Models;

namespace Scheme.Controllers
{
    [Produces("application/json")]
    [Route("api/cards")]
    public class CardsController : Controller
    {
        ProjectContext _db;

        public CardsController(ProjectContext db)
        {
            this._db = db;
        }

        [HttpGet]
        public IEnumerable<Card> Get()
        {
            return _db.Cards.ToList();
        }

        [HttpGet("{id}")]
        public IActionResult Card(int id)
        {
            Card card = _db.Cards.FirstOrDefault(x => x.Id == id);
            if (card == null) return NotFound(); 
            return Ok(card);
        }

        [HttpPost]
        public IActionResult AddCard([FromBody] Card card)
        {
            if (card != null)
            {
                _db.Cards.Add(card);
                _db.SaveChanges();
                return Ok(card);
            }
            return BadRequest(ModelState);
        }

        //if (id != 0)
        //    {
        //        Card card = _db.Cards.FirstOrDefault(x => x.Id == id);
        //        if (card != null) return 
        //    }
    }
}