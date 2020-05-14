using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ticketmaster.Models;
using ticketmaster.Services;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ticketmaster.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase

    {
        
        private readonly CreditCardsService _creditCardsService;
       
        public CreditCardController(CreditCardsService CreditCardService)
        {
            _creditCardsService = CreditCardService;
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult<List<CreditCard>> Get()
        {
            
            return _creditCardsService.Get();

        }
        
        [Authorize]
        [HttpGet("byUsername")]
        public async Task<ActionResult<List<CreditCard>>> Get(string username)
        {
            var creditCards = await _creditCardsService.GetByUsername(username);

            if (creditCards == null)
            {
                return NotFound();
            }

            return creditCards;
        }

        [HttpPost]
        public ActionResult<CreditCard> Create(CreditCard m)
        {
            _creditCardsService.Create(m);

            return CreatedAtRoute("GetCreditCard", new { id = m.Id.ToString() }, m);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, CreditCard min)
        {
            var contactform = _creditCardsService.Get(id);

            if (contactform == null)
            {
                return NotFound();
            }

            _creditCardsService.Update(id, min);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var creditCard = _creditCardsService.Get(id);

            if (creditCard == null)
            {
                return NotFound();
            }

            _creditCardsService.Remove(creditCard);

            return NoContent();
        }
    }
}
