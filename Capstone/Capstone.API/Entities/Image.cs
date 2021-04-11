using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Capstone.API.Entities
{
    public class Image : MongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        
        public string Id { get; set; }
        public ImageSource ContentImage { get; set; }
    }
}
