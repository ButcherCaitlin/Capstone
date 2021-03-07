using Scheduling.Events;
using System.Collections.Generic;

namespace Scheduling
{
    public static class SchedulingUtility
    {
        public static bool ScheduleShowing(Showing showing, User realtor, User client, Property property){
            // what if one cannot shcedule the showing? We need a way to remove it.
            return false;
        }

        public static List<Showing> GetAvailableShowings(User realtor, User client, Property property)
        {
            return new List<Showing>();
        }
    }
}
