using System;

namespace Capstone.Models
{
    public class TimeBlock
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public bool Available { get; set; }
    }
}
