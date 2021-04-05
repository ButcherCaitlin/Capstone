using System;
using System.Collections.Generic;

namespace Capstone.Models
{
    public class User : Storable, IScheduleable
    {
        public string AuthToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        //public string UserType { get; set; }
        public string HashedPassword { get; set; }
        public bool CustomAvailability { get; set; }
        public Availability Availability { get; set; }
        public Dictionary<DayOfWeek, bool> WorkingDays => this.Availability.WorkingDays;
        public TimeSpan DayStart => this.Availability.DayStart;
        public TimeSpan DayEnd => this.Availability.DayEnd;
        public string TimeZone => this.Availability.TimeZone;
        public List<Showing> Events => this.Availability.Events;
    }
}
