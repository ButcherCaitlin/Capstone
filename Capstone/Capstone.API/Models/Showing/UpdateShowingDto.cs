using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.API.Models
{
    public class UpdateShowingDto
    {
        [Required]
        public string PropertyID { get; set; }
        [Required]
        public string RealtorID { get; set; }
        [Required]
        public string ProspectID { get; set; }
        [Required]
        public DateTimeOffset StartTime { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
    }
}
