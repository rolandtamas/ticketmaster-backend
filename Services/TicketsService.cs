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
using Microsoft.VisualBasic;
using ticketmaster.Services;

namespace ticketmaster.Services
{
    public class TicketsService
    {
        private readonly IMongoCollection<Ticket> _tickets;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly ITicketsDatabaseSettings _settings;
        private readonly MatchesService _matchesService;
        private readonly UsersService _usersService;
        public TicketsService(ITicketsDatabaseSettings settings, MatchesService matchesService, UsersService usersService)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
    

            _tickets = _database.GetCollection<Ticket>(settings.DatabaseCollectionName);
            _settings = settings;
            _matchesService = matchesService;
            _usersService = usersService;

         
        }
       
        public List<Ticket> Get() {

            var matches = _matchesService.GetCollection();
            /*var query = _tickets.Aggregate()
                .Unwind(x => x.match)
                .Lookup(
                        foreignCollectionName: "teams",
                        localField: "match.teamHostId",
                        foreignField: "Id",
                        @as: "match");
            var result = query.ToList();*/
          
            /*JOIN QUERY FOR TICKETS & MATCHES */
            
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
        //Return The Tickets of a Specific User
        public async Task<List<Ticket>> GetByUsername(string username) {
           var user = await _usersService.Get(username);
           var  matches = _matchesService.GetCollection();
            List <Ticket> list = new List<Ticket>();
            foreach (string ticketid in user.tickets)
            {
                list.Add(_tickets.Find<Ticket>(ticket => ticket.Id.Equals(ticketid)).FirstOrDefault());
            }
           /* if (!list.Any())
            {
                return null;   
            }*/
            var query = from t in list.AsQueryable()
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
        public List<Ticket> GetByMatchId(string matchId)
        {
            return _tickets.Find(ticket => ticket.matchId == matchId).ToList();
        }

        public List<Ticket> GetAvailableByMatchId(string matchId)
        {
            return _tickets.Find(ticket => ticket.matchId == matchId && ticket.status == 1).ToList();
        }

         public Ticket Get(string id) =>
            _tickets.Find<Ticket>(ticket => ticket.Id == id).FirstOrDefault();


        public Ticket Create(Ticket Ticket)
        {
            _tickets.InsertOne(Ticket);
            return Ticket;
        }

        public void Update(string id, Ticket TicketIn) =>
            _tickets.ReplaceOne(Ticket => Ticket.Id==id, TicketIn);

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
