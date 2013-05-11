//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System;
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

        public RouteService(CurrentState state) : base(state, new RouteDataHelper())
        {
            
            // initialise current routes from DB
            if (!state.RoutesInitialised)
            {
                //var routes = dataHelper.LoadAll();
                var routes = new Dictionary<int, Route>();
                state.InitialiseRoutes(routes);
            }
        }

        /// <summary>
        /// Creates a new route for the given [transportType, company, origin, destination] combination.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="originID"></param>
        /// <param name="destinationID"></param>
        /// <param name="transportType"></param>
        /// <param name="deliveryTimes"></param>
        /// <param name="duration"></param>
        /// <param name="maxWeight"></param>
        /// <param name="maxVolume"></param>
        /// <param name="costPerGram"></param>
        /// <param name="costPerCm3"></param>
        /// <returns>the created object, with ID field, and LastEdited initialised</returns>
        /// <exception cref="DatabaseException">if it already exists</exception>
        /// <exception cref="InvalidObjectStateException">if the fields are invalid</exception>
        /// <exception cref="ArgumentException">if any of the objects referenced by id do not exist</exception>
        public Route Create(TransportType transportType, int companyId, int originID, int destinationID, IList<WeeklyTime> deliveryTimes, int duration, int maxWeight, int maxVolume, int costPerGram, int costPerCm3)
        {
            // load parameters from id
            var origin = state.GetRouteNode(originID);
            if(origin == null)
                throw new ArgumentException(string.Format("There is no location with id = {0}", originID), "originID");

            var destination = state.GetRouteNode(destinationID);
            if (destination == null)
                throw new ArgumentException(string.Format("There is no location with id = {0}", destinationID), "destinationID");

            var company = state.GetCompany(companyId);
            if (company == null)
                throw new ArgumentException(string.Format("There is no company with id = {0}", companyId), "companyId");


            // throws an exception if invalid
            var newRoute = new Route { TransportType = transportType, Company = company, Origin = origin, Destination = destination, DepartureTimes = deliveryTimes, Duration = duration, MaxWeight = maxWeight, MaxVolume = maxVolume, CostPerGram = costPerGram, CostPerCm3 = costPerCm3};

            // throws a database exception if exists already
            dataHelper.Create(newRoute);

            // update state
            state.SaveRoute(newRoute);
            state.IncrementNumberOfEvents();

            return newRoute;
        }

        /// <summary>
        /// Updates the route for the given [transportType, company, origin, destination] combination.
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="deliveryTimes"></param>
        /// <param name="scope"></param>
        /// <param name="duration"></param>
        /// <param name="maxWeight"></param>
        /// <param name="maxVolume"></param>
        /// <param name="costPerGram"></param>
        /// <param name="costPerCm3"></param>
        /// <returns>the updated object, with ID field, and LastEdited initialised</returns>
        /// <exception cref="DatabaseException">if it doesn't exist</exception>
        /// <exception cref="InvalidObjectStateException">if the fields are invalid</exception>
        /// <exception cref="ArgumentException">if any of the objects referenced by id do not exist</exception>
        public Route Update(int routeId, IList<WeeklyTime> deliveryTimes, int duration, int maxWeight, int maxVolume, int costPerGram, int costPerCm3)
        {
            var route = state.GetRoute(routeId);
            if (route == null)
                throw new ArgumentException(string.Format("There is no route with id = {0}", routeId), "routeId");
            
            // throws an exception if invalid
            var newRoute = new Route { TransportType = route.TransportType, Company = route.Company, Origin = route.Origin, Destination = route.Destination, DepartureTimes = deliveryTimes, Duration = duration, MaxWeight = maxWeight, MaxVolume = maxVolume, CostPerGram = costPerGram, CostPerCm3 = costPerCm3 };

            // throws a database exception if doesn't exist
            dataHelper.Update(newRoute);

            // update state
            state.SaveRoute(newRoute);
            state.IncrementNumberOfEvents();

            return newRoute;
        }

        /// <summary>
        /// Adds the given delivery time to the route, and returns the new route.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deliveryTime"></param>
        /// <returns></returns>
        /// <exception cref="DatabaseException">if the delivery time already exists in the route</exception>
        /// <exception cref="ArgumentException">if delivery time is null, or if the route doesn't exist</exception>
        public Route AddDeliveryTime(int id, WeeklyTime deliveryTime)
        {
            if (deliveryTime == null)
            {
                throw new ArgumentException("deliveryTime cannot be null");
            }

            var route = state.GetRoute(id);
            if(route==null)
                throw new ArgumentException("There is no route with id = " + id);

            // throws database exception if already exists
            ((RouteDataHelper)dataHelper).AddDeliveryTime(route, deliveryTime);

            // update state
            state.SaveRoute(route);
            state.IncrementNumberOfEvents();

            return route;
        }

        /// <summary>
        /// Deletes the given delivery time from the route, and returns the updated route.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deliveryTime"></param>
        /// <returns></returns>
        /// <exception cref="DatabaseException">if the delivery time already exists in the route</exception>
        /// <exception cref="ArgumentException">if delivery time is null, or if the route doesn't exist</exception>
        public Route DeleteDeliveryTime(int id, WeeklyTime deliveryTime)
        {
            if (deliveryTime == null)
            {
                throw new ArgumentException("deliveryTime cannot be null");
            }

            var route = state.GetRoute(id);
            if (route == null)
                throw new ArgumentException("There is no route with id = " + id);

            // throws database exception if delivery time doesn't exist
            ((RouteDataHelper)dataHelper).DeleteDeliveryTime(route, deliveryTime);

            // update state
            state.SaveRoute(route);
            state.IncrementNumberOfEvents();

            return route;
        }


        public override Route Get(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            return state.GetRoute(id);
        }

        public override IList<Route> GetAll()
        {
            return state.GetAllRoutes();
        }

        public override bool Exists(Route route)
        {
            var countries = state.GetAllRoutes().AsQueryable();

            return countries.Any(t => t.Equals(route));
        }


        public override void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            // remove from db
            dataHelper.Delete(id);

            // remove from state     
            state.RemoveRoute(id);
            state.IncrementNumberOfEvents();
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
