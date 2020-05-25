using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using ticketmaster.Models;
using ticketmaster.Services;

namespace ticketmaster.Data
{
    public class AuthRepository : IAuthRepository
    {
        private UsersService _usersService;

        public AuthRepository(UsersService usersService)
        {
            _usersService = usersService;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _usersService.Get(username);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.passwordHash, user.passwordSalt))
                return null;
            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash =  hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0; i<computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                
            }
            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;

          await _usersService.Create(user);
            return user;
        }

        public User Update(User user, string password)
        {
            byte[]passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;

            _usersService.Update(user.username, user);
            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            var user = await _usersService.Get(username);
            if (user == null) return false;
            else return true;
        }
    }
}
