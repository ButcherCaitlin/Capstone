using System;

namespace Scheduling.Events
{
    public class Showing : EventBase
    {
        public Showing(DateTime start, TimeSpan duraiton)
        {
            Start = start;
            Duration = duraiton;
            Available = false;
        }

        public string PropertyID { get; set; }
        public string RealtorID { get; set; }
        public string ProspectID { get; set; }
    }
}
