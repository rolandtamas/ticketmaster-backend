using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

       
        public MatchController(MatchesService MatchService)
        {
            _matchService = MatchService;
        
          
        }
        /* Changing this bit below so it connects to the Teams Collection and shows the Team Names as well*/
        [HttpGet]
        public ActionResult<List<Match>> Get()
        {
            return _matchService.Get();

            /* THIS IS A JOIN QUERY */
            /*
            var query1 = from m in _matchesCollection.AsQueryable()
                         join t in _teamsCollection.AsQueryable() on m.teamAwayId equals t.Id into teamAwayInfo

                         select new Match()
                         {
                             Id = m.Id,

                             date = m.date,
                             teamAwayId = m.teamAwayId,
                             teamHostId = m.teamHostId,
                            teamAway = teamAwayInfo.First(),
                            teamHost = null
                        };
            var query2 = from m in query1.ToList()
                    join t in _teamsCollection.AsQueryable() on m.teamHostId equals t.Id into teamHostInfo

                    select new Match()
                    {
                        Id = m.Id,
                        date = m.date,
                        teamAwayId = m.teamAwayId,
                        teamHostId = m.teamHostId,
                        teamAway = m.teamAway,
                        teamHost = teamHostInfo.First()
                    };

            var matchesAndTeams = query2.ToList();
            return matchesAndTeams;
            */
            /* THIS IS A QUERY USING DBREF */
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
