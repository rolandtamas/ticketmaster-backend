using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketmaster.Models;
using MongoDB.Driver;

namespace ticketmaster.Services
{
    public class TeamsService
    {
        private readonly IMongoCollection<Team> _teams;

        public TeamsService(ITeamsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _teams = database.GetCollection<Team>(settings.DatabaseCollectionName);
        }
        public List<Team> Get() =>
            _teams.Find(team => true).ToList();

        public Team Get(string id) =>
            _teams.Find<Team>(team => team.Id.Equals( id)).FirstOrDefault();

        public Team Create(Team Team)
        {
            _teams.InsertOne(Team);
            return Team;
        }

        public void Update(string id, Team TeamIn) =>
            _teams.ReplaceOne(team => team.Id.Equals( id), TeamIn);

        public void Remove(Team TeamIn) =>
            _teams.DeleteOne(Team => Team.Id.Equals( TeamIn.Id));

        public void Remove(string id) =>
            _teams.DeleteOne(Team => Team.Id.Equals(id));

        /*Exporting Collection */
        public IMongoCollection<Team> GetCollection() =>
           _teams;
    }
}
