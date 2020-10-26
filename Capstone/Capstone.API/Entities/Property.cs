using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Capstone.API.Entities
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Address { get; set; }
        [Required]
        public string OwnerID { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public float Bathrooms { get; set; }
        public float Acres { get; set; }
        public int Bedrooms { get; set; }
        public int SqFootage { get; set; }
        [Required]
        public int BuildYear { get; set; }
        public string Type { get; set; }
    }
}
