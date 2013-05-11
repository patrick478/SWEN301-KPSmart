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
    /// Represents an instance of a Route, with a specific Time.
    /// Is used by Delivery.cs.
    /// </summary>
    public class RouteInstance
    {

        public RouteInstance(Route route, DateTime time)
        {
            Route = route;
            DepartureTime = time;
        }

        // the route it belongs to
        public Route Route { get; private set; }
    
        // The date of departure
        public DateTime DepartureTime { get; private set; }

        // The date of arrival
        public DateTime ArrivalTime
        {
            get
            {
                return DepartureTime.AddMinutes(Route.Duration);
            }
        }
    
    }
}
