using System;

namespace Capstone.API.Entities
{
    public class Showing : MongoEntity
    {
        public Showing()
        {
            Available = false;
        }
        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Available { get; set; }
        public string PropertyID { get; set; }
        public string RealtorID { get; set; }
        public string ProspectID { get; set; }
    }
}
