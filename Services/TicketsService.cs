using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketmaster.Models;
using MongoDB.Driver;

namespace ticketmaster.Services
{
    public class TicketsService
    {
        private readonly IMongoCollection<Tickets> _tickets;
        public TicketsService(ITicketsDatabaseSettings settings) {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _tickets = database.GetCollection<Tickets>(settings.TicketsCollectionName);
        }

        public List<Tickets> Get() => _tickets.Find(tickets => true).ToList();
        public Tickets Get(string id) =>
            _tickets.Find<Tickets>(ticket => ticket.Id == id).FirstOrDefault();

        public Tickets Create(Tickets tickets)
        {
            _tickets.InsertOne(tickets);
            return tickets;
        }

        public void Update(string id, Tickets ticketIn) =>
            _tickets.ReplaceOne(tickets => tickets.Id == id, ticketIn);

        public void Remove(Tickets ticketIn) =>
            _tickets.DeleteOne(tickets => tickets.Id == ticketIn.Id);

        public void Remove(string id) =>
            _tickets.DeleteOne(tickets => tickets.Id == id);
    }
}
