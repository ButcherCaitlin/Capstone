using Capstone.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Utility
{
    public static class AvailibilityUtility
    {
        public static Task<Dictionary<DayOfWeek, TimeBlock>> CreateDefaultAvailibility()
        {
            Dictionary<DayOfWeek, TimeBlock> handmade = new Dictionary<DayOfWeek, TimeBlock>();
            handmade.Add(DayOfWeek.Monday, new TimeBlock { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0) });
            handmade.Add(DayOfWeek.Tuesday, new TimeBlock { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0) });
            handmade.Add(DayOfWeek.Wednesday, new TimeBlock { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0) });
            handmade.Add(DayOfWeek.Thursday, new TimeBlock { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0) });
            handmade.Add(DayOfWeek.Friday, new TimeBlock { Start = new TimeSpan(9, 0, 0), End = new TimeSpan(17, 0, 0) });
            return Task.FromResult(handmade);
        }
    }
}
