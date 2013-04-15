//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System.Collections.Generic;
using Common;
using Server.Data;

namespace Server.Business
{
    /// <summary>
    /// The idea of this class is to contain all the methods to manipulate Routes.  
    /// When we receive an edit from a client, it goes through here, and these methods call the right methods in the Server.Data and Server.Network
    /// to save and update accordingly.
    /// </summary>
    public class RouteService
    {
        private StateSnapshot state;
        private RouteDataHelper data;

        public void AddRoute(Route newRoute)
        {

        }

        public void EditRoute(Route editedRoute)
        {

        }

        public void DeleteRoute(int id)
        {
           
        }

        public IEnumerable<Route> GetAllRoutes()
        {
            //todo
            return null;
        }


        







    }
}
