using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Models
{
    public class ShowingDto
    {
        public string Id { get; set; }
        public string PropertyID { get; set; }
        public string CounterpartID { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Host { get; set; }
    }
}
