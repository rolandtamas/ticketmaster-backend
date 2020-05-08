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

            var matches = _matchesService.GetCollection();
            var teams = _matchesService.GetTeamsService().GetCollection();
            /*var query = _tickets.Aggregate()
                .Unwind(x => x.match)
                .Lookup(
                        foreignCollectionName: "teams",
                        localField: "match.teamHostId",
                        foreignField: "Id",
                        @as: "match");
            var result = query.ToList();*/

            /* ONE QUERY FOR 3 JOINS */
            
             var query = from ti in _tickets.AsQueryable()
                         join m in matches.AsQueryable()
                             on ti.matchId equals m.Id into matchInfo
                         join te in teams.AsQueryable()
                             on ti.match.teamAwayId equals te.Id into teamAwayInfo

                         join teh in teams.AsQueryable()
                             on ti.match.teamHostId equals teh.Id into teamHostInfo

                         select new Ticket()
                         {
                             Id = ti.Id,
                             status = ti.status,
                             seat = ti.seat,
                             sector = ti.sector,
                             price = ti.price,
                             matchId = ti.matchId,

                             match = new Match()
                             {
                                 Id = ti.match.Id,
                                 date = ti.match.date,
                                 teamAwayId = ti.match.teamHostId,
                                 teamHostId = ti.match.teamHostId,
                                 teamAway = teamAwayInfo.FirstOrDefault(),
                                 teamHost = teamHostInfo.FirstOrDefault()
                             }


                         }; 



            /* THIS IS A JOIN QUERY FOR MATCHES & TEAMS*/
            
            var query1 = from m in matches.AsQueryable()
                         join t in teams.AsQueryable() on m.teamAwayId equals t.Id into teamAwayInfo

                         select new Match()
                         {
                             Id = m.Id,

                             date = m.date,
                             teamAwayId = m.teamAwayId,
                             teamHostId = m.teamHostId,
                             teamAway = teamAwayInfo.First(),
                             teamHost = null
                         };
            var query2 = from m in query1.ToList()
                         join t in teams.AsQueryable() on m.teamHostId equals t.Id into teamHostInfo

                         select new Match()
                         {
                             Id = m.Id,
                             date = m.date,
                             teamAwayId = m.teamAwayId,
                             teamHostId = m.teamHostId,
                             teamAway = m.teamAway,
                             teamHost = teamHostInfo.First()
                         };
          
            /*JOIN QUERY FOR TICKETS & MATCHES */
            
            var query3 = from t in _tickets.AsQueryable()
                         join m in query2 on t.matchId equals m.Id into matchInfo

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
            var ticketsAndMatches = query3.ToList(); 
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
