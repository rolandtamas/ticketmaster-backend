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

    public class UsersService
    {
        private readonly IMongoCollection<User> _users;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IUsersDatabaseSettings _settings;
        private readonly MatchesService _matchesService;
        public UsersService(IUsersDatabaseSettings settings, MatchesService matchesService)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
    

            _users = _database.GetCollection<User>(settings.DatabaseCollectionName);
            _settings = settings;
            _matchesService = matchesService;

         
        }
       
        public List<User> Get() {
            return _users.Find(user => true).ToList();
        }

        public async Task<User> Get(string username) 
        {
          return await _users.Find<User>(user => user.username.Equals(username)).FirstOrDefaultAsync();
       
        }

        public async Task<User> Create(User User)
        {
             await _users.InsertOneAsync(User);
            return User;
        }

        public void Update(string id, User UserIn) =>
            _users.ReplaceOne(User => User.Id.Equals(id), UserIn);

        public void Remove(User UserIn) =>
            _users.DeleteOne(User => User.Id == UserIn.Id);

        public void Remove(string id) =>
            _users.DeleteOne(User => User.Id.Equals(id));

        /* Small updates 02.05.2020  */
        /* Inserting a Filter Function So You Can Query The Collection With Simple jsonQueries */
        public List<User> Filter(string jsonQuery)
        {
            var queryDoc = new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(jsonQuery));
            return _users.Find<User>(queryDoc).ToList();
        }
        /*Exporting Collecion to Controller*/
        public IMongoCollection<User> GetCollection() =>
            _users;
        /* Exporting the Database Object for the FetchDBRefAs to use it in the User Controller */
        public IMongoDatabase GetDatabase(IMatchesDatabaseSettings settings) =>
            _client.GetDatabase(settings.DatabaseName);
        
    }
        
}
