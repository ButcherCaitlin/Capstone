using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Capstone.API.Entities
{
    public class Session : MongoEntity
    {
        [BsonElement("user_id")]
        public string UserId { get; set; }
        public string Jwt { get; set; }
    }
}
