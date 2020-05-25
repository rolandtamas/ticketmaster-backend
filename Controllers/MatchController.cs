using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
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
        private readonly IMongoCollection<Match> _matchesCollection;
        private readonly IMongoCollection<Team> _teamsCollection;
        private readonly TicketsService _ticketsService;

       
        public MatchController(MatchesService MatchService, TicketsService ticketsService)
        {
            _matchService = MatchService;
            _ticketsService = ticketsService;
        
          
        }
        //LIST OF AVAILABLE MATCHES
        [HttpGet]
        public ActionResult<List<Match>> Get()
        {
            var all_matches = _matchService.Get();

            foreach(Match match in all_matches)
            {
                var tickets = _ticketsService.GetAvailableByMatchId(match.Id);
                /*if(!tickets.Any())
                {
                    all_matches.Remove(match);
                } */
                match.ticketCount = tickets.Count;
            }
            
            return all_matches;

            

          
        }
        
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
            var match = _matchService.Get(id);

            if (match == null)
            {
                return NotFound();
            }

            _matchService.Update(id, min);

            return NoContent();
        }

        [HttpPut]
        public IActionResult Update(string id, int ticketCount,Match m) {

            var match = _matchService.Get(id);
            if (match == null)
            { return NotFound(); }
            _matchService.Update(ticketCount.ToString(),m);
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

            _matchService.Remove(match);

            return NoContent();
        }
    }
}
