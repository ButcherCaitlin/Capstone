using System;
using Scheduling.Events;

namespace Scheduling
{
    public class Property
    {

        public Property()
        {
        }

        public string Address { get; set; }
        public string OwnerID { get; set; }
        public double Price { get; set; }
        public double Bathrooms { get; set; }
        public double Acres { get; set; }
        public int Bedrooms { get; set; }
        public int SqFooteage { get; set; }
        public int BuildYear { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public TimeSpan ShowingDuraiton { get; set; }
        public Availability Availability { get; set; }
        public bool ScheduleShowing(DateTime start)
        {
            return Availability.ScheduleEvent(new Showing(start, this.ShowingDuraiton));
        }
    }
}
