using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppointmentSystem
{
    public class TimeSlots
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<TimeSlots> GetAvailableTimeSlots(DateTime date)
        {
            var slots = new List<TimeSlots>
            {
                new TimeSlots { StartTime = date.AddHours(9), EndTime = date.AddHours(10) },
                new TimeSlots { StartTime = date.AddHours(10), EndTime = date.AddHours(11) },
                new TimeSlots { StartTime = date.AddHours(11), EndTime = date.AddHours(12) },
                new TimeSlots { StartTime = date.AddHours(13), EndTime = date.AddHours(14) },
                new TimeSlots { StartTime = date.AddHours(14), EndTime = date.AddHours(15) },
                new TimeSlots { StartTime = date.AddHours(15), EndTime = date.AddHours(16) },
                new TimeSlots { StartTime = date.AddHours(16), EndTime = date.AddHours(17) },
                new TimeSlots { StartTime = date.AddHours(17), EndTime = date.AddHours(18) },
                new TimeSlots { StartTime = date.AddHours(18), EndTime = date.AddHours(19) },
                new TimeSlots { StartTime = date.AddHours(19), EndTime = date.AddHours(20) },
                // Add more slots as needed
            };

            return slots;
        }
    }
}