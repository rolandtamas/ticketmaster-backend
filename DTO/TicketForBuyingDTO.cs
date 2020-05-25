using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ticketmaster.DTO
{
    public class TicketForBuyingDTO
    {
        
        public string matchid { get; set; }
        public string home { get; set; }
        public string away { get; set; }
        public string date { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string sector { get; set; }
        public string row { get; set; }
        public int amount { get; set; }
    }
}
