using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Capstone.API.Models
{
    public class Showing
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Property Property { get; set; }
        public User Realtor { get; set; }
        public User Prospect { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public float Duration { get; set; }

    }
}
