using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.API.Models
{
    public class CreateShowingDto
    {
        public CreateShowingDto()
        {
            Available = false;
        }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        public bool Available { get; set; }
        public string PropertyID { get; set; }
        public string RealtorID { get; set; }
        public string ProspectID { get; set; }
    }
}
