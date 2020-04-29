using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ticketmaster.Models;
using ticketmaster.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ticketmaster.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly MatchesService _matchService;

        public MatchController(MatchesService MatchService)
        {
            _matchService = MatchService;
        }

        [HttpGet]
        public ActionResult<List<Match>> Get() =>
            _matchService.Get();

        [HttpGet("{id:length(24)}", Name = "GetMatch")]
        public ActionResult<Match> Get(string id)
        {
            var match = _matchService.Get(id);

            if (match == null)
            {
                return NotFound();
            }

            return match;
        }

        [HttpPost]
        public ActionResult<Match> Create(Match m)
        {
            _matchService.Create(m);

            return CreatedAtRoute("GetMatch", new { id = m.Id.ToString() }, m);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Match min)
        {
            var contactform = _matchService.Get(id);

            if (contactform == null)
            {
                return NotFound();
            }

            _matchService.Update(id, min);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var match = _matchService.Get(id);

            if (match == null)
            {
                return NotFound();
            }

            _matchService.Remove(match.Id);

            return NoContent();
        }
    }
}
