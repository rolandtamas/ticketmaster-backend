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
    public class CreditCard
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string creditCardNumber  { get; set; }
        public string creditCardHolder  { get; set; }
        public string expirationDate  {get; set;}
        public string cvv  {get; set;}
    }
}