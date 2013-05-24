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
    /// Represents an instance of a Route, with a specific Time.
    /// Is used by Delivery.cs.
    /// </summary>
    public class RouteInstance : IComparable<RouteInstance>
    {
        public int CompareTo(RouteInstance other)
        {
            if (other.ArrivalTime == this.ArrivalTime && other.DepartureTime == this.DepartureTime && this.Route.Equals(other.Route))
                return 0;
            return -1;
        }

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
        public virtual DateTime ArrivalTime
        {
            get
            {
                return DepartureTime.AddMinutes(Route.Duration);
            }
        }
    
    }
}
