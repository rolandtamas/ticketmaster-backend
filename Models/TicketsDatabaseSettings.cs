using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketmaster.Models
{
    public class TicketsDatabaseSettings:ITicketsDatabaseSettings
    {
        public string TicketsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ITicketsDatabaseSettings {
        string TicketsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

