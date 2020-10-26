using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Models.Showing
{
    public abstract class OutboundShowingDto
    {
        public string Id { get; set; }
        public string PropertyID { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
