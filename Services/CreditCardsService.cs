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
    public class CreditCardsService
    {
        private readonly IMongoCollection<CreditCard> _creditCards;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly ICreditCardsDatabaseSettings _settings;

        
        private readonly UsersService _usersService;
        public CreditCardsService(ICreditCardsDatabaseSettings settings, UsersService usersService)
        {
            _client = new MongoClient(settings.ConnectionString);
            _database = _client.GetDatabase(settings.DatabaseName);
    

            _creditCards = _database.GetCollection<CreditCard>(settings.DatabaseCollectionName);
            _settings = settings;
            _usersService = usersService;
         
        }
       
        public List<CreditCard> Get() {
           return _creditCards.Find(creditCard => true).ToList();
        }
        
        //Return The CreditCards of a Specific User
        public async Task<List<CreditCard>> GetByUsername(string username) {
           var user = await _usersService.Get(username);
           
            List <CreditCard> list = new List<CreditCard>();
            foreach (ObjectId creditCardId in user.creditCards)
            {
                list.Add(_creditCards.Find<CreditCard>(creditCard => creditCard.Id.Equals(creditCardId)).FirstOrDefault());
            }
            
            return list;
        }

         public CreditCard Get(string id) =>
            _creditCards.Find<CreditCard>(creditCard => creditCard.Id.Equals( id)).FirstOrDefault();


        public CreditCard Create(CreditCard CreditCard)
        {
            _creditCards.InsertOne(CreditCard);
            return CreditCard;
        }

        public void Update(string id, CreditCard CreditCardIn) =>
            _creditCards.ReplaceOne(CreditCard => CreditCard.Id.Equals(id), CreditCardIn);

        public void Remove(CreditCard CreditCardIn) =>
            _creditCards.DeleteOne(CreditCard => CreditCard.Id == CreditCardIn.Id);

        public void Remove(string id) =>
            _creditCards.DeleteOne(CreditCard => CreditCard.Id.Equals(id));

        /* Small updates 02.05.2020  */
        /* Inserting a Filter Function So You Can Query The Collection With Simple jsonQueries */
        public List<CreditCard> Filter(string jsonQuery)
        {
            var queryDoc = new QueryDocument(BsonSerializer.Deserialize<BsonDocument>(jsonQuery));
            return _creditCards.Find<CreditCard>(queryDoc).ToList();
        }
        /*Exporting Collecion to Controller*/
        public IMongoCollection<CreditCard> GetCollection() =>
            _creditCards;
        /* Exporting the Database Object for the FetchDBRefAs to use it in the CreditCard Controller */
        public IMongoDatabase GetDatabase(IMatchesDatabaseSettings settings) =>
            _client.GetDatabase(settings.DatabaseName);
        
    }
        
}
