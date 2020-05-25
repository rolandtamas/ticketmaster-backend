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
using ticketmaster.Data;
using ticketmaster.DTO;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ticketmaster.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase

    {
        
        private readonly UsersService _usersService;
        private readonly IAuthRepository _authRepo;
       
        public UserController(UsersService usersService, IAuthRepository authRepo)
        {
            _usersService = usersService;
            _authRepo = authRepo;
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
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(UserForUpdateDTO model)
        {
            var user = await _usersService.Get(model.username);

            if (user == null)
            {
                return NotFound();
            }
            
            user.firstName=model.firstName;
            user.lastName=model.lastName;
            user.email=model.email;
            if(model.password != null)
            {
                _authRepo.Update(user, model.password );
            }
           
           _usersService.Update(model.username, user);

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
