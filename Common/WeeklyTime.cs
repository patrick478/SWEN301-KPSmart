//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////
using System;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// This class represents a weekly time.  It has a day of the week component, an hour componenet, and a minute component.
    /// </summary>
    public class WeeklyTime : IComparable<WeeklyTime>
    {

        public static int MINUTES_IN_A_WEEK = 10080;
       
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

        /// <summary>
        /// Constructor to make a WeeklyTime from a DateTime.  It extracts out the DayOfWeek, Hour, and Minute component.
        /// </summary>
        /// <param name="datetime"></param>
        public WeeklyTime(DateTime datetime): this(datetime.DayOfWeek, datetime.Hour, datetime.Minute) {
        }

        /// <summary>
        /// Constructor to create a weekly time from ticks.
        /// </summary>
        /// <param name="ticks"></param>
        public WeeklyTime (long ticks) 
        {
            this.Value = new TimeSpan(ticks);
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


        public int CompareTo(WeeklyTime other)
        {

            var value = (this.Value.Ticks - other.Value.Ticks);

            if (value < 0)
                return -1;
            if (value > 0)
                return 1;

            return 0;
        }

        /// <summary>
        /// Creates a network string for a list of WeeklyTime objects. Used when communicating departing times of Routes. Convert back into List via ParseTimeNetString method.
        /// </summary>
        /// <param name="times"></param>
        /// <returns></returns>
        public static string BuildTimesNetString(IList<WeeklyTime> times)
        {
            //TODO
            return "";
        }

        /// <summary>
        /// Builds a list of WeeklyTime objects from a network string (that was generated via the BuildTimesNetString method). Used when communicating departing times of Routes.
        /// </summary>
        /// <returns>List of WeeklyTime objects.</returns>
        public static IList<WeeklyTime> ParseTimesNetString(string times)
        {
            //TODO
            return null;
        }

    }
}
