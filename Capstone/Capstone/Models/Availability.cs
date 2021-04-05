using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.Models
{
    public class Availability : Storable
    {
        public Dictionary<DayOfWeek, bool> WorkingDays { get; set; }
        public TimeSpan DayStart { get; set; }
        public TimeSpan DayEnd { get; set; }
        public string TimeZone { get; set; }
        public List<Showing> Events { get; set; }

        public static List<Showing> GetPossibleShowings(List<object> participants)
        {
            // combine todays availability for the 1st, and 2nd object into a single new 
            // availability called "runing availability" do it with every item in the 
            // scheduleable list.

            // if at any point the availability is noexistent (one is not available today) return
            // an empty list of possible showings.

            Property property = participants.Where(p => p is Property).FirstOrDefault() as Property;

            Availability runningAvailability = new Availability()
            {
                WorkingDays = CloneWorkingDays(property),
                DayStart = property.Availability.DayStart,
                DayEnd = property.Availability.DayEnd,
                TimeZone = property.Availability.TimeZone,
                Events = CloneEvents(property)
            };

            participants.Remove(property);

            //for each
            foreach (var item in participants)
            {
                if (item is IScheduleable asScheduleable)
                {
                    // 1: check to make sure they are both available today
                    runningAvailability.WorkingDays.TryGetValue(DateTime.Today.DayOfWeek, out bool runningAvailableToday);
                    asScheduleable.WorkingDays.TryGetValue(DateTime.Today.DayOfWeek, out bool asScheduleAvailableToday);
                    if (!runningAvailableToday && !asScheduleAvailableToday) return new List<Showing>();

                    // 2: make the new avail start time the one that starts later in the day
                    if (asScheduleable.DayStart > runningAvailability.DayStart) runningAvailability.DayStart = asScheduleable.DayStart;
                    // no need for else because the correct time is already assigned to the running avail

                    // 3: make the new avail end time the one that starts earlier
                    if (asScheduleable.DayEnd < runningAvailability.DayEnd) runningAvailability.DayEnd = asScheduleable.DayEnd;

                    // 4: pool all the events in the single events list
                    runningAvailability.Events.AddRange(CloneEvents(asScheduleable));
                }
            }

            // after you have the "pooled availibility, get the possible showings by getting the 
            // properties viewing time.
            List<Showing> possibleShowings = new List<Showing>();
            bool availabilityExhausted = false;
            DateTime nextStart = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                runningAvailability.DayStart.Hours,
                runningAvailability.DayStart.Minutes,
                runningAvailability.DayStart.Seconds);

            // the possible showings will include conflict showings.
            while (!availabilityExhausted)
            {
                // create a new showing and mark it as available if there is no conflict, 
                // and add it to the possible showings.
                possibleShowings.Add(NoConflict(new Showing()
                {
                    StartTime = nextStart,
                    Duration = property.ShowingDuraiton
                }, runningAvailability.Events));

                //update the next start time.
                nextStart = nextStart + property.ShowingDuraiton;

                //if the next event would go out of the working hours we have exhausted the availability.
                if ((nextStart.TimeOfDay + property.ShowingDuraiton) > runningAvailability.DayEnd) availabilityExhausted = true;
            }

            return possibleShowings;
        }

        private static List<Showing> CloneEvents(IScheduleable scheduleable)
        {
            List<Showing> toReturn = new List<Showing>();
            foreach (Showing showing in scheduleable.Events)
            {
                toReturn.Add(new Showing()
                {
                    Available = showing.Available,
                    PropertyID = showing.PropertyID,
                    ProspectID = showing.ProspectID,
                    RealtorID = showing.RealtorID,
                    StartTime = showing.StartTime,
                    Duration = showing.Duration
                });
            }
            return toReturn;
        }

        private static Dictionary<DayOfWeek, bool> CloneWorkingDays(IScheduleable scheduleable)
        {
            Dictionary<DayOfWeek, bool> toReturn = new Dictionary<DayOfWeek, bool>();
            foreach (KeyValuePair<DayOfWeek,bool> kvp in scheduleable.WorkingDays)
                toReturn.Add(kvp.Key, kvp.Value);
            return toReturn;
        }

        public static Showing NoConflict(Showing toMark, List<Showing> existing)
        {
            bool available = true;
            foreach (Showing toCompare in existing)
            {
                if (toCompare.StartTime < toMark.StartTime && 
                    (toCompare.StartTime + toCompare.Duration) > toMark.StartTime)
                {
                    available = false;
                    break;
                } 
                else if (toMark.StartTime < toCompare.StartTime &&
                    (toMark.StartTime + toMark.Duration) > toCompare.StartTime)
                {
                    available = false;
                    break;
                }
            }
            toMark.Available = available;
            return toMark;
        }
    }
}
