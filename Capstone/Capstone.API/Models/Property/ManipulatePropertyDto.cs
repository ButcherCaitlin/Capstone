using Capstone.API.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Capstone.API.Models
{
    public abstract class ManipulatePropertyDto
    {
        [Required]
        [MaxLength(50)]
        public string Address { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public float Bathrooms { get; set; }
        public float Acres { get; set; }
        public int Bedrooms { get; set; }
        public int SqFootage { get; set; }
        [Required]
        public int BuildYear { get; set; }
        public string Type { get; set; }
        public TimeSpan ShowingDuraiton { get; set; }
        public bool CustomAvailability { get; set; }
        public Availability Availability { get; set; }
    }
}
