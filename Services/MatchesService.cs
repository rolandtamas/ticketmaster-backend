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

        public MatchesService(IMatchesDatabaseSettings settings)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);

            _matches = _database.GetCollection<Match>(settings.DatabaseCollectionName);
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

        public void Remove(Match MatchIn) =>
            _matches.DeleteOne(Match => Match.Id == MatchIn.Id);

        public void Remove(string id) =>
            _matches.DeleteOne(Match => Match.Id == id);

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
    }
}
