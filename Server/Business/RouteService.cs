//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System.Collections.Generic;
using System.Linq;
using Common;
using Server.Data;

namespace Server.Business
{
    /// <summary>
    /// The idea of this class is to contain all the methods to manipulate Routes.  
    /// When we receive an edit from a client, it goes through here, and these methods call the right methods in the Server.Data and Server.Network
    /// to save and update accordingly.
    /// </summary>
    public class RouteService: Service<Route>
    {
        //TODO
        private PathFinder pathfinder;

        public RouteService(CurrentState state) : base(state, new RouteDataHelper())
        {
            // initialise current routes
            if (!state.RoutesInitialised)
            {
                //var routes = dataHelper.LoadAll();
                var routes = new Dictionary<int, Route>();
                state.InitialiseRoutes(routes);
                pathfinder = new PathFinder(this);
            }
        }

        public override Route Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<Route> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public override bool Exists(Route obj)
        {
            throw new System.NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

       
        /// <summary>
        /// This is a method for pathfinder class to use.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IList<Route> GetAll(RouteNode node)
        {
            // get all routes as queryable
            var routes = state.GetAllRoutes().AsQueryable();

            // find relevant ones
            var releventRoutes = routes.Where(r => r.Origin.Equals(node));

            // return
            return releventRoutes.ToList();
        }
    }
}
