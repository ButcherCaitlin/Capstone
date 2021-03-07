using Capstone.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Utility
{
    public static class AvailibilityUtility
    {
        public static Task<Availability> CreateDefaultAvailibility()
        {
            Availability toReturn = new Availability();
            toReturn.DayStart = new TimeSpan(9, 0, 0);
            toReturn.DayEnd = new TimeSpan(17, 0, 0);
            toReturn.TimeZone = "MST";
            toReturn.Events = new List<Showing>();
            Dictionary<DayOfWeek, bool> toReturnWorkingDays = new Dictionary<DayOfWeek, bool>();
            toReturnWorkingDays.Add(DayOfWeek.Monday, true);
            toReturnWorkingDays.Add(DayOfWeek.Tuesday, true);
            toReturnWorkingDays.Add(DayOfWeek.Wednesday, true);
            toReturnWorkingDays.Add(DayOfWeek.Thursday, true);
            toReturnWorkingDays.Add(DayOfWeek.Friday, true);
            toReturn.WorkingDays = toReturnWorkingDays;
            return Task.FromResult(toReturn);
        }

        public static Task<bool> NoConflict(List<Object> participants, Showing showing)
        {
            bool noConflict = true;
            foreach(Object o in participants)
            {
                if (o is User asUser)
                {
                    if (!asUser.Availability.ScheduleEvent(showing)) noConflict = false;
                } 
                else if (o is Property asProperty)
                {
                    if (!asProperty.Availability.ScheduleEvent(showing)) noConflict = false;
                }
            }
            return Task.FromResult(noConflict);
        }
    }
}
