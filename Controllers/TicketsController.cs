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
    public class TicketsController : ControllerBase
    {
        private readonly TicketsService _ticketsService;
        public TicketsController(TicketsService ticketsService)
        {
            _ticketsService = ticketsService;
        }

        [HttpGet]
        public ActionResult<List<Tickets>> Get() =>
           _ticketsService.Get();

        [HttpGet("{id:length(24)}", Name = "GetTicket")]
        public ActionResult<Tickets> Get(string id)
        {
            var ticket = _ticketsService.Get(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;

        }

        [HttpPost]
        public ActionResult<Tickets> Create(Tickets t)
        {
            _ticketsService.Create(t);

            return CreatedAtRoute("GetTicket", new { id = t.Id.ToString() }, t);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Tickets t)
        {
            var ticket = _ticketsService.Get(id);

            if (ticket == null)
            {
                return NotFound();
            }

            _ticketsService.Update(id, t);

            return NoContent();
        }


    }
}
