using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketmaster.Models;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;


namespace ticketmaster.Services
{
    public class MatchesService
    {
        private readonly IMongoCollection<Match> _matches;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly TeamsService _teamService;
        public MatchesService(IMatchesDatabaseSettings settings, TeamsService teamService)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
    

            _matches = _database.GetCollection<Match>(settings.DatabaseCollectionName);
            _teamService = teamService;


         
        }
       
        public List<Match> Get() {
            var teams = _teamService.GetCollection();
            /* THIS IS A JOIN QUERY */
            var query1 = from m in _matches.AsQueryable()
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

            var matchesAndTeams = query2.ToList();
            return matchesAndTeams;



            /*return _matches.Find(match => true).ToList();*/

        }
        public IQueryable<Match> GetMatchesAsIQueryable()
        {
            var teams = _teamService.GetCollection();
            /* THIS IS A JOIN QUERY */
            var query1 = from m in _matches.AsQueryable()
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
            var query2 = from m in query1
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


            return query2;



            /*return _matches.Find(match => true).ToList();*/

        }



        public Match Get(string id) =>
            _matches.Find<Match>(match => match.Id.Equals( id)).FirstOrDefault();

        public Match Create(Match Match)
        {
            _matches.InsertOne(Match);
            return Match;
        }

        public void Update(string id, Match MatchIn) =>
            _matches.ReplaceOne(Match => Match.Id.Equals(id), MatchIn);

        public void Remove(Match MatchIn) =>
            _matches.DeleteOne(Match => Match.Id == MatchIn.Id);

        public void Remove(string id) =>
            _matches.DeleteOne(Match => Match.Id.Equals(id));

        /* Small updates 02.05.2020  */
        /* Inserting a Filter Function So You Can Query The Collection With Simple jsonQueries */
        public List<Match> Filter(string jsonQuery)
        {
            var queryDoc = new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(jsonQuery));
            return _matches.Find<Match>(queryDoc).ToList();
        }
        /*Exporting Collecion to Controller*/
        public IMongoCollection<Match> GetCollection() =>
            _matches;
        /* Exporting the Database Object for the FetchDBRefAs to use it in the Match Controller */
        public IMongoDatabase GetDatabase(IMatchesDatabaseSettings settings) =>
            _client.GetDatabase(settings.DatabaseName);
        public IMongoClient GetClient() =>
            _client;
        public TeamsService GetTeamsService() =>
            _teamService;
        
    }
        
}
