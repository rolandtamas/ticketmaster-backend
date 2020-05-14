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
    public class UserController : ControllerBase

    {
        
        private readonly UsersService _usersService;
       
        public UserController(UsersService usersService)
        {
            _usersService = usersService;
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            
            return _usersService.Get();

        }
        
        [Authorize]
        [HttpGet("byUsername")]
        public async Task<ActionResult<User>> GetByUsername(string username)
        {
            var user = await _usersService.Get(username);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public ActionResult<User> Create(User m)
        {
            _usersService.Create(m);

            return CreatedAtRoute("GetUser", new { id = m.Id.ToString() }, m);
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> Update(string username, User min)
        {
            var user = await _usersService.Get(username);

            if (user == null)
            {
                return NotFound();
            }

            _usersService.Update(username, min);

            return NoContent();
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            var user = await _usersService.Get(username);

            if (user == null)
            {
                return NotFound();
            }

            _usersService.Remove(user);

            return NoContent();
        }
    }
}
