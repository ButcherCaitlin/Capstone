using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone.API.Models
{
    public class ManipulateShowingDto
    {
        //The other fields required to create/update a showing will be collected 
        //in the endpoint, and in the user id included in the header of the request.
        public ManipulateShowingDto()
        {
            Available = false;
        }
        public bool Available { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
    }
}
