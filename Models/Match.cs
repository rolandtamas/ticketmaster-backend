using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Entities.Core;


namespace ticketmaster.Models
{
    public class Match
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Int32 matchId { get; set; }
        public DateTime date { get; set; }
        [BsonElement]
        public Int32 teamAwayId { get; set; }
        [BsonElement]
        public Int32 teamHostId { get; set; }
        [BsonIgnore]
        public Team teamAway { get; set; }
        [BsonIgnore]
        public Team teamHost { get; set; }
    }
}
