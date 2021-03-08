using System;

namespace Scheduling.Events
{
    public class EventBase
    {
        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; }
        public bool Available { get; set; }
    }
}
