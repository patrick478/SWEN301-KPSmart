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
    /// 
    /// todo: Still not sure how we will get this instance.  From the Route maybe?  Probably from the RouteService
    /// </summary>
    public class RouteInstance
    {

        public RouteInstance(Route route, DateTime time)
        {
            Route = route;
            DepartureTime = time;
        }

        public Route Route { get; private set; }
    
        // I think this should be a DateTime, and not a Weekly time...?
        public DateTime DepartureTime { get; private set; }
    
    }
}
