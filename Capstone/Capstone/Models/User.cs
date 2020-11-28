using System;
using System.Collections.Generic;

namespace Capstone.Models
{
    public class User : Storable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        //public string UserType { get; set; }
        public bool CustomAvailability { get; set; }
        public Dictionary<DayOfWeek, TimeBlock> Availability { get; set; }
    }
}
