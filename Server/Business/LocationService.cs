using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Data;

namespace Server.Business
{
    public class LocationService: Service<RouteNode>
    {
        public LocationService(CurrentState state) : base(state, new RouteNodeDataHelper())
        {
            // initialise current routeNodes
            if (!state.RouteNodesInitialised)
            {
                var routeNodes = dataHelper.LoadAll();
                state.InitialiseRouteNodes(routeNodes);
            }
        }

        /// <summary>
        /// Creates a new International Port.
        /// </summary>
        /// <param name="countryId">The id of the country</param>
        /// <returns>the created object, with ID field, and LastEdited initialised</returns>
        /// <exception cref="DatabaseException">if the object already exists</exception>
        /// <exception cref="InvalidObjectStateException">if the state is invalid</exception>
        /// <exception cref="ArgumentException">if the country doesn't exist</exception>
        public InternationalPort CreateInternationalPort(int countryId)
        {
            // get the country
            var country = state.GetCountry(countryId);
            if(country==null)
                throw new ArgumentException("There is no country with an id of " + countryId);
 
            // throws an exception if invalid
            var port = new InternationalPort(country);

            // throws a database exception if already exists
            dataHelper.Create(port);

            // update state
            state.SaveRouteNode(port);
            state.IncrementNumberOfEvents();

            return port;
        }

        /// <summary>
        /// Creates a new DistributionCentre
        /// </summary>
        /// <param name="name">The name of the distribution centre</param>
        /// <returns>the created object, with ID field, and LastEdited initialised</returns>
        /// <exception cref="DatabaseException">if the object already exists</exception>
        /// <exception cref="InvalidObjectStateException">if the state is invalid</exception>
        public DistributionCentre CreateDistributionCentre(string name)
        {
            // throws an exception if invalid
            var distributionCentre = new DistributionCentre(name);

            // throws a database exception if already exists
            dataHelper.Create(distributionCentre);

            // update state
            state.SaveRouteNode(distributionCentre);
            state.IncrementNumberOfEvents();

            return distributionCentre;
        }

        public override RouteNode Get(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            return state.GetRouteNode(id);
        }

        public override IList<RouteNode> GetAll()
        {
            return state.GetAllRouteNodes();
        }

        public override bool Exists(RouteNode routeNode)
        {
            var routeNodes = state.GetAllRouteNodes().AsQueryable();
            bool result = routeNodes.Any(t => t.Equals(routeNode));
            return result;
        }

        public override void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            var routeNode = state.GetRouteNode(id);

            // check the routeNode isn't used in any current routes
            var routes = state.GetAllRoutes();
            bool isUsed = routes.AsQueryable().Any(t => t.Origin.Equals(routeNode) || t.Destination.Equals(routeNode));
            if (isUsed)
            {
                throw new IllegalActionException("Cannot remove a location that is used in an active route.");
            }

            // remove from db
            dataHelper.Delete(id);

            // remove from state     
            state.RemoveRouteNode(id);
            state.IncrementNumberOfEvents();
        }
    }
}
