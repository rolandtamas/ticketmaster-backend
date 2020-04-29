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
    public class ContactController : ControllerBase
    {
        private readonly FormsService _formsService;

        public ContactController(FormsService formsService)
        {
            _formsService=formsService;
        }

        [HttpGet]
        public ActionResult<List<ContactForm>> Get() =>
            _formsService.Get();

        [HttpGet("{id:length(24)}", Name = "GetForm")]
        public ActionResult<ContactForm> Get(string id)
        {
            var book = _formsService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<ContactForm> Create(ContactForm cf)
        {
            _formsService.Create(cf);

            return CreatedAtRoute("GetForm", new { id = cf.Id.ToString() }, cf);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ContactForm cfin)
        {
            var contactform = _formsService.Get(id);

            if (contactform == null)
            {
                return NotFound();
            }

            _formsService.Update(id, cfin);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var contactform = _formsService.Get(id);

            if (contactform == null)
            {
                return NotFound();
            }

            _formsService.Remove(contactform.Id);

            return NoContent();
        }
    }
}
