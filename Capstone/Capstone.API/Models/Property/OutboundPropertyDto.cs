using Capstone.API.Entities;
using System;
using System.Collections.Generic;

namespace Capstone.API.Models
{
    public class OutboundPropertyDto
    {
        public string Id { get; set; }
        public string Address { get; set; }
        public string OwnerID { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public float Bathrooms { get; set; }
        public float Acres { get; set; }
        public int Bedrooms { get; set; }
        public int SqFootage { get; set; }
        public int BuildYear { get; set; }
        public string Type { get; set; }
        public bool CustomAvailability { get; set; }
        public Availability Availability { get; set; }
    }
}
