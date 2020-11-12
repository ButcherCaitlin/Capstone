using System;

namespace Capstone.Models
{
    public class Showing : Storable
    {
        public string PropertyID { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string RealtorID { get; set; }
        public string ProspectID { get; set; }
    }
}
