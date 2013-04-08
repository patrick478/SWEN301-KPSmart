//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////
using System;

namespace Common
{
    /// <summary>
    /// Still not too sure how this class works.  I'm thinking if we represent time in minutes, then we can add and subtract easily.
    /// 
    /// TODO: Not sure if we need this class to limit it to 7 days and have nice constructors, or if we can just use the TimeSpan class, and add an extension method to work out the day of the week?
    /// 
    /// 
    /// TODO: if this is a good idea, we need to test it.
    /// </summary>
    public class WeeklyTime
    {
        private static int MINUTES_IN_A_WEEK = 10080;

        public WeeklyTime(int inMinutes)
        {
            
            if (inMinutes > MINUTES_IN_A_WEEK)
            {
                throw new ArgumentException("inMinutes must be under the number of minutes in a week");
            }

            // todo: set the value
        }

        public WeeklyTime(DayOfWeek day, int hour, int minute)
        {
            //todo: set the value
        }


        // the underlying field for this class. 
        // WeeklyTime is represented in minutes from Monday 00:00 to Sunday 23:59.  
        // Eg. 01:45 on Monday would be represented as 105, 13:45 on Monday would be represented as 825.
        public TimeSpan Value { get; private set; }



        // the day of the week component of this time
        public DayOfWeek DayOfTheWeek
        {
            get
            {
                int day = (int)Value.TotalDays;

                switch (day)
                {
                    case(0): return DayOfWeek.Monday;
                    case(1): return DayOfWeek.Tuesday;
                    case(2): return DayOfWeek.Wednesday;
                    case(3): return DayOfWeek.Thursday;
                    case(4): return DayOfWeek.Friday;
                    case(5): return DayOfWeek.Saturday;
                    case(6): return DayOfWeek.Sunday;
                    default: throw new Exception("WeeklyTime is invalid - shouldn't ever be more than the number of minutes in a week");           
                }
            }
        }
    }
}
