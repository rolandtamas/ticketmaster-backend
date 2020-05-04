using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketmaster.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
      
        public Int32 status { get; set; }
        public string seat { get; set; }
        public string sector { get; set; }
        public double price { get; set; }
        public ObjectId matchId { get; set; }
        [BsonIgnore]
        public Match match { get; set; }
    }
}
