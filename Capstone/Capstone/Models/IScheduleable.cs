using System;
using System.Collections.Generic;

namespace Capstone.Models
{
    public interface IScheduleable
    {
        Dictionary<DayOfWeek, bool> WorkingDays { get; }
        TimeSpan DayStart { get; }
        TimeSpan DayEnd { get; }
        string TimeZone { get; }
        List<Showing> Events { get; }
    }
}
