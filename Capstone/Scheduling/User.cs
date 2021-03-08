using System;
using System.Collections.Generic;

namespace Scheduling
{
    public class User
    {
        User()
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public bool CustomAvailability { get; set; }
        public Availability Availability { get; set; }
        public bool ScheduleShowing(DateTime start, TimeSpan duraiton)
        {
            return Availability.ScheduleEvent(new Showing(start, duraiton));
        }
    }
}
