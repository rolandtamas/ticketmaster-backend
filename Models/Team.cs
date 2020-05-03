using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using MongoDB.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticketmaster.Models
{
    public class Team
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public Int32 teamId { get; set; }
        public string teamName { get; set; }

        /* One to Many with MongoDB.Entities.Core
        public Many<Match> Matches { get; set; }
        public Team() => this.InitOneToMany(() => Matches); */
        
    }
}
