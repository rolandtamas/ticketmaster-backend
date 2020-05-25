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
        public DateTime date { get; set; }
        public string teamAway { get; set; } /*This is the actual object that will get assigned in the query in the MatchService. */
        public string teamHost { get; set; }
        [BsonIgnore]
        public int ticketCount {get; set;}
        public object teamAwayId { get; internal set; }
        public object teamHostId { get; internal set; }
        /* 
public Team GetTeamAway (MongoDatabase db)
{
return this.teamAwayObject = db.FetchDBRefAs<Team>(teamAway);
}
public Team GetTeamHost (MongoDatabase db)
{
return this.teamHostObject = db.FetchDBRefAs<Team>(teamHost);
} */
    }
}
