using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketmaster.Models;
using MongoDB.Driver;

namespace ticketmaster.Services
{
    public class MatchesService
    {
        private readonly IMongoCollection<Match> _matches;

        public MatchesService(IMatchesDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _matches = database.GetCollection<Match>(settings.MatchesCollectionName);
        }
        public List<Match> Get() =>
            _matches.Find(match => true).ToList();

        public Match Get(string id) =>
            _matches.Find<Match>(match => match.Id == id).FirstOrDefault();

        public Match Create(Match Match)
        {
            _matches.InsertOne(Match);
            return Match;
        }

        public void Update(string id, Match MatchIn) =>
            _matches.ReplaceOne(Match => Match.Id == id, MatchIn);

        public void Update(int amount, Match m) { 
            _matches.ReplaceOne(Match => Match.ticketCount == amount, m);
        }

        public void Remove(Match MatchIn) =>
            _matches.DeleteOne(Match => Match.Id == MatchIn.Id);

        public void Remove(string id) =>
            _matches.DeleteOne(Match => Match.Id == id);


    }
}
