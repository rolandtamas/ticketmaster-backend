using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ticketmaster.DTO
{
    public class UserForUpdateDTO
    {
        [Required]
        public string username { get; set; }

        
        [StringLength(20,MinimumLength = 4, ErrorMessage = "You Must Specify A Password With At Least 4 And Maximum 20 Characters")]
        public string password { get; set; }
        [Required]
        public string firstName {get; set;}
        [Required]
        public string lastName {get; set;}

        [EmailAddress(ErrorMessage = "E-mail address is not a valid e-mail format")]
        [Required]
        public string email {get; set;}
    }
}
