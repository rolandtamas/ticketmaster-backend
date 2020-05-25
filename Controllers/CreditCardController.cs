using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ticketmaster.Models;
using ticketmaster.Services;
using ticketmaster.DTO;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ticketmaster.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase

    {
        
        private readonly CreditCardsService _creditCardsService;
        private readonly UsersService _usersService;
       
        public CreditCardController(CreditCardsService CreditCardService, UsersService UsersService)
        {
            _creditCardsService = CreditCardService;
            _usersService = UsersService;
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
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CreditCard>> Create(string username, CreditCardForAddingDTO creditCardModel)
        {

            var user = await _usersService.Get(username);

            if (user == null)
            {
                return NotFound();
            }

            var creditCard = new CreditCard {
                creditCardNumber=creditCardModel.creditCardNumber,
                creditCardHolder=creditCardModel.creditCardHolder,
                expirationDate=creditCardModel.expirationDate,
                cvv=creditCardModel.cvv,
            };
          var newcc =  _creditCardsService.Create(creditCard);
            user.creditCards.Add(newcc.Id);
            _usersService.Update(user.username, user);


            return StatusCode(201);
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
        [Authorize]
        [HttpDelete]
        public async Task <IActionResult> Delete(string username, string creditCardId)
        {
            var creditCard = _creditCardsService.Get(creditCardId);

            if (creditCard == null)
            {
                return NotFound();
            }

            _creditCardsService.Remove(creditCard);
            var user = await _usersService.Get(username);
            user.creditCards.Remove(creditCardId);
            _usersService.Update(username, user);


            return NoContent();
        }
    }
}
