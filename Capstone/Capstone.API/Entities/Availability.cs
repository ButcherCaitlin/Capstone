using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.API.Entities
{
    public class Availability
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<DayOfWeek, bool> WorkingDays { get; set; }
        public TimeSpan DayStart { get; set; }
        public TimeSpan DayEnd { get; set; }
        public string TimeZone { get; set; }
        public List<Showing> Events { get; set; }

        public bool ScheduleEvent(Showing toAdd)
        {
            // Make sure that the event to be scheduled is on a working day.
            bool workingDay;
            WorkingDays.TryGetValue(toAdd.Start.DayOfWeek, out workingDay);
            if (!workingDay) return false;

            // Validate that the start of the event is after the start of the workday.
            if (toAdd.Start.TimeOfDay < DayStart) return false;

            // Validate that the end time (start + duration) is before the end of the day. This
            // will also catch if the start time is after the end of the day. 
            if (toAdd.Start.TimeOfDay + toAdd.Duration > DayEnd) return false;

            // The final check is to see if there is an event on that day and time already shceduled.
            var possibleConflicts = Events.Where(e => (e.Start.Date == toAdd.Start.Date) && !e.Available);

            //for each event that day make sure that they do not conflict with the to be added time.
            foreach (Showing e in possibleConflicts)
            {
                Showing first,second;
                if (e.Start < toAdd.Start)
                {
                    first = e;
                    second = toAdd;
                } 
                else
                {
                    first = toAdd;
                    second = e;
                }
                // If the event that occurs first has an end after the start time of the second event there was a conflict.
                if ((first.Start.TimeOfDay + first.Duration) > second.Start.TimeOfDay) return false;
            }

            // if there were no conflicts, add the event.
            Events.Add(toAdd);
            return true;
        }
    }
}
