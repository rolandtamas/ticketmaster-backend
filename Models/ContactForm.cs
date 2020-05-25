using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ticketmaster.Models
{
    public class ContactForm 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string name{ get; set; }
        public string subject { get; set; }
        public string email { get; set; }
        public string message { get; set; }

    }
}
