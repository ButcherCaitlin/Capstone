using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.API.Entities
{
    public class Showing : MongoEntity
    {
        [Required]
        public string PropertyID { get; set; }
        [Required]
        public string RealtorID { get; set; }
        [Required]
        public string ProspectID { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
