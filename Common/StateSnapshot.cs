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
    /// This class is a snapshot of all the data at a specific time.
    /// </summary>
    public class StateSnapshot: State
    {

        private readonly DateTime timeRepresented;
        public DateTime TimeRepresented {
            get { return timeRepresented; }
        }

        public StateSnapshot(DateTime timeRepresented,
                            IDictionary<int, Route> routes,
                            IDictionary<int, Price> prices,
                            IDictionary<int, Delivery> deliveries,
                            IDictionary<int, RouteNode> routeNodes,
                            IDictionary<int, Company> companies,
                            IDictionary<int, Country> countries): base(routes, prices, deliveries, routeNodes, companies, countries)
        {
            this.timeRepresented = timeRepresented;
        }
    }
}
