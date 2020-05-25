using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ticketmaster.Models;
using ticketmaster.DTO;
using ticketmaster.Services;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ticketmaster.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TicketController : ControllerBase

    {
        
        private readonly TicketsService _ticketService;
        private readonly UsersService _usersService;
       
        public TicketController(TicketsService ticketService, UsersService usersService)
        {
            _ticketService = ticketService;
            _usersService = usersService;
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult<List<Ticket>> Get()
        {
            
            return _ticketService.Get();

        }
        
        [Authorize]
        [HttpGet("byUsername")]
        public async Task<ActionResult<List<Ticket>>> Get(string username)
        {
            var tickets = await _ticketService.GetByUsername(username);

            if (tickets == null)
            {
                return NotFound();
            }

            return tickets;
        }

        [HttpPost]
        public ActionResult<Ticket> Create(Ticket m)
        {
            _ticketService.Create(m);

            return CreatedAtRoute("GetTicket", new { id = m.Id.ToString() }, m);
        }
        
        [HttpPut]
        public IActionResult Update(string id, Ticket min)
        {
            var contactform = _ticketService.Get(id);

            if (contactform == null)
            {
                return NotFound();
            }

            _ticketService.Update(id, min);

            return NoContent();
        }
        //TICKET CANCELATION
        [Authorize]
        [HttpPut("cancel")]
        public async Task<IActionResult> Update(string username, string ticketId)
        {
            var ticket = _ticketService.Get(ticketId);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.status=1;

            _ticketService.Update(ticketId, ticket);

            var user = await _usersService.Get(username);
            user.tickets.Remove(ticketId);
            _usersService.Update(username, user);


            return NoContent();
        }
        //TICKET PURCHASE
        [Authorize]
        [HttpPut("buy")]
        public async Task<IActionResult> Update([FromBody]TicketForBuyingDTO model, [FromQuery]string username)
        {
            var tickets = _ticketService.GetAvailableByMatchId(model.matchid);

            if (!tickets.Any())
            {
            return NotFound();
            }

            var user = await _usersService.Get(username);
            
            var index= 1;
             foreach (Ticket ticket in tickets)
            {
                if(index>model.amount)
                {
                    break;
                }
                ticket.status = 0;
                user.tickets.Add(ticket.Id);
                _ticketService.Update(ticket.Id, ticket);
                index = index+1;
            }


            
            _usersService.Update(username, user);
            return NoContent();
        }


        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var ticket = _ticketService.Get(id);

            if (ticket == null)
            {
                return NotFound();
            }

            _ticketService.Remove(ticket);

            return NoContent();
        }
    }
}
