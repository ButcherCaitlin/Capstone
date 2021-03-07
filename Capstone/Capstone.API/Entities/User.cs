using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Newtonsoft.Json;

namespace Capstone.API.Entities
{
    public class User : MongoEntity
    {
        [JsonProperty("auth_token")]
        [BsonIgnore]
        public string AuthToken { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(20)]
        public string Phone { get; set; }

        // field: type
        // can be removed or modified later. mainly adding in case need to know if realtor/seller/other
        //public string UserType { get; set; }

        [JsonIgnore]
        public string HashedPassword { get; set; }
        public bool CustomAvailability { get; set; }
        //[BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Availability Availability { get; set; }
    }
}
