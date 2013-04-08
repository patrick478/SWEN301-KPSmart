//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System;
using System.Collections.Generic;
using Common;

namespace Server.Data
{
    /// <summary>
    /// The idea is this class is used to extract Routes from the DB, and save routes to the DB.  
    /// It should be used by the RouteService class.
    /// 
    /// Maybe it returns the DateTime instead of void so StateSnapshot can be updated???
    /// </summary>
    public class RouteDataHelper
    {

        /// <summary>
        /// Loads the route of the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Route LoadRoute(int id)
        {
            //todo
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads the most up to date version of all routes in the system.
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, Route> LoadAllRoutes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads all routes that were in the system at the given snapshotTime.
        /// </summary>
        /// <param name="snapshotTime"></param>
        /// <returns></returns>
        public IDictionary<int, Route> LoadAllRoutes(DateTime snapshotTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This either calls editRoute, or newRoute accordingly.
        /// </summary>
        /// <param name="newRoute"></param>
        public Route SaveRoute(Route newRoute)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Used for saving changes to an existing route.
        /// </summary>
        /// <param name="route"></param>
        private Route EditRoute(Route route)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Used for saving a new route.
        /// </summary>
        /// <param name="route"></param>
        private Route NewRoute(Route route)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the route, and returns a copy of the deleted route.
        /// This allows the delete to be time stamped.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void DeleteRoute(int id)
        {
            throw new NotImplementedException();
        }
    }
}
