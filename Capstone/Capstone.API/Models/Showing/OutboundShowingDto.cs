using System;

namespace Capstone.API.Models
{
    public class OutboundShowingDto
    {
        public string Id { get; set; }
        public string PropertyID { get; set; }
        public string RealtorID { get; set; }
        public string ProspectID { get; set; }
        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Available { get; set; }
    }
}
