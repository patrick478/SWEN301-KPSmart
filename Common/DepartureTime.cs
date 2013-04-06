//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

namespace Common
{
    /// <summary>
    /// Not sure about this class yet.
    /// 
    /// In the DB it will probably be represented as DaysOfTheWeek and WeeklyTime.  Not sure if we want to make a Route have a list of WeeklyTime only (with the WeeklyTime holding the day of the week info too.)
    /// Would it help with pathfinding?
    /// </summary>
    public class DepartureTime
    {
        private DaysOfTheWeek days;

        private WeeklyTime departureWeeklyTime;

    }
}
