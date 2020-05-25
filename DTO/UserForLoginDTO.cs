using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ticketmaster.DTO
{
    public class UserForLoginDTO
    {
        
        public string username { get; set; }

        
        public string password { get; set; }
    }
}
