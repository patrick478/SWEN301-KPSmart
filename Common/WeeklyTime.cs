﻿//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////
using System;

namespace Common
{
    /// <summary>
    /// This class represents a weekly time.  It has a day of the week component, an hour componenet, and a minute component.
    /// </summary>
    public class WeeklyTime
    {
       
        /// <summary>
        /// Constructs a new WeeklyTime.
        /// </summary>
        /// <param name="day"></param>
        /// <param name="hour">hour component - between 0 and 23</param>
        /// <param name="minute">minute component - between 0 and 59</param>
        public WeeklyTime(DayOfWeek day, int hour, int minute)
        {
            // check arguments
            if (hour >= 24)
            {
                throw new ArgumentException("Cannot have an hour component higher than 23");
            }

            if (hour < 0)
            {
                throw new ArgumentException("Cannot have a negative hour component");
            }

            if (minute >= 60)
            {
                throw new ArgumentException("Cannot have a minute component higher than 59");
            }

            if (minute < 0)
            {
                throw new ArgumentException("Cannot have a negative minute component");
            }

            // set the value
            Value = new TimeSpan(GetDayOfWeekNumberValue(day), hour, minute, 0);      
        }


        // the underlying field for this class. 
        // WeeklyTime is represented in minutes from Monday 00:00 to Sunday 23:59.  
        // Eg. 01:45 on Monday would be represented as 105, 13:45 on Monday would be represented as 825.
        public TimeSpan Value { get; private set; }

        /// <summary>
        /// Gets the day component of the weekly time.
        /// </summary>
        public DayOfWeek DayComponent
        {
            get
            {
                int day = (int)Value.TotalDays;

                return GetDayOfWeekFromNumberValue(day);
            }
        }

        /// <summary>
        /// Gets the hour component of the weekly time
        /// </summary>
        public int HourComponent
        {
            get {                    
                TimeSpan hours = Value.Subtract(new TimeSpan((int) Value.TotalDays, 0, 0, 0));
                return (int)hours.TotalHours;
            }
        }

        /// <summary>
        /// Gets the minute component of the weekly time
        /// </summary>
        public int MinuteComponent
        {
            get
            {
                TimeSpan hours = Value.Subtract(new TimeSpan((int) Value.TotalDays, 0, 0, 0));
                TimeSpan minutes = hours.Subtract(new TimeSpan(HourComponent, 0, 0));
                return (int)minutes.TotalMinutes;
            }
        }

        /// <summary>
        /// Private helper method to map from DayOfWeek to number value.  Mon = 0, Tue = 1 etc.
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        private int GetDayOfWeekNumberValue(DayOfWeek day)
        {
            switch (day)
            {
                case (DayOfWeek.Monday):
                    return 0;
                case (DayOfWeek.Tuesday):
                    return 1;
                case(DayOfWeek.Wednesday):
                    return 2;
                case (DayOfWeek.Thursday):
                    return 3;
                case(DayOfWeek.Friday):
                    return 4;
                case(DayOfWeek.Saturday):
                    return 5;
                case(DayOfWeek.Sunday):
                    return 6;
                default:
                    throw new Exception("Cannot calculate for a null day.");
            }
        }

        /// <summary>
        /// Private helper method to map from number value to DayOfWeek.  0 = Mon, 1 = Tue etc.
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        private DayOfWeek GetDayOfWeekFromNumberValue(int day)
        {
            switch (day)
            {
                case (0): return DayOfWeek.Monday;
                case (1): return DayOfWeek.Tuesday;
                case (2): return DayOfWeek.Wednesday;
                case (3): return DayOfWeek.Thursday;
                case (4): return DayOfWeek.Friday;
                case (5): return DayOfWeek.Saturday;
                case (6): return DayOfWeek.Sunday;
                default: throw new Exception("WeeklyTime is invalid - shouldn't ever be more than the number of minutes in a week");
            }
        }




    }
}