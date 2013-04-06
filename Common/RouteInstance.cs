//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////
namespace Common
{
    /// <summary>
    /// Represents an instance of a Route, with a specific Time.
    /// Is used by Delivery.cs.
    /// 
    /// todo: Still not sure how we will get this instance.  From the Route maybe?  
    /// </summary>
    public class RouteInstance
    {

        public RouteInstance(Route route, WeeklyTime time)
        {
            //todo check that the time is valid for the route

            Route = route;
            DepartureTime = time;
        }

        public Route Route { get; private set; }
    
        // don't know if this should be a WeeklyTime or a DateTime
        public WeeklyTime DepartureTime { get; private set; }
    
    }
}
