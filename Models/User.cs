using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace ticketmaster.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string username { get; set; }
        
        public byte[] passwordHash { get; set; }
        
        public byte[] passwordSalt { get; set; }

        public string firstName {get; set;}
        public string lastName {get; set;}

        
        public string email {get; set;}

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> creditCards {get; set;}
        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> tickets {get; set;}

    }
}
