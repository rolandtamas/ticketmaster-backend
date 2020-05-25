using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketmaster.Models;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Diagnostics;

namespace ticketmaster.Services
{
    public class TicketsService
    {
        private readonly IMongoCollection<Ticket> _tickets;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly ITicketsDatabaseSettings _settings;
        private readonly MatchesService _matchesService;
        public TicketsService(ITicketsDatabaseSettings settings, MatchesService matchesService)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
    

            _tickets = _database.GetCollection<Ticket>(settings.DatabaseCollectionName);
            _settings = settings;
            _matchesService = matchesService;

         
        }
       
        public List<Ticket> Get() {

            var matches = _matchesService.GetMatchesAsIQueryable();
            /* THIS IS A JOIN QUERY */
            
            var query = from t in _tickets.AsQueryable()
                         join m in matches.AsQueryable() on t.matchId equals m.Id into matchInfo

                         select new Ticket()
                         {
                             Id = t.Id,
                             status = t.status,
                             seat = t.seat,
                             sector = t.sector,
                             price = t.price,
                             matchId = t.matchId,
                             match = matchInfo.First() 
                     };
            var ticketsAndMatches = query.ToList();
            return ticketsAndMatches;
        }
        
        public Ticket Get(string id) =>
            _tickets.Find<Ticket>(match => match.Id.Equals( id)).FirstOrDefault();

        public Ticket Create(Ticket Ticket)
        {
            _tickets.InsertOne(Ticket);
            return Ticket;
        }

        public void Update(string id, Ticket TicketIn) =>
            _tickets.ReplaceOne(Ticket => Ticket.Id.Equals(id), TicketIn);

        public void Remove(Ticket TicketIn) =>
            _tickets.DeleteOne(Ticket => Ticket.Id == TicketIn.Id);

        public void Remove(string id) =>
            _tickets.DeleteOne(Ticket => Ticket.Id.Equals(id));

        /* Small updates 02.05.2020  */
        /* Inserting a Filter Function So You Can Query The Collection With Simple jsonQueries */
        public List<Ticket> Filter(string jsonQuery)
        {
            var queryDoc = new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(jsonQuery));
            return _tickets.Find<Ticket>(queryDoc).ToList();
        }
        /*Exporting Collecion to Controller*/
        public IMongoCollection<Ticket> GetCollection() =>
            _tickets;
        /* Exporting the Database Object for the FetchDBRefAs to use it in the Ticket Controller */
        public IMongoDatabase GetDatabase(IMatchesDatabaseSettings settings) =>
            _client.GetDatabase(settings.DatabaseName);
        
    }
        
}
