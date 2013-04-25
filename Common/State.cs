using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// Base class for the system state.  This class holds all data in concurrent collections,
    /// and provides methods for reading the data.
    /// 
    /// The state doesn't have to be readonly - this is up to the implementing class to decide.
    /// </summary>
    public abstract class State
    {
        protected IDictionary<int, Route> routes;
        protected IDictionary<int, Price> prices;
        protected IDictionary<int, Delivery> deliveries;
        protected IDictionary<int, RouteNode> routeNodes;
        protected IDictionary<int, Company> companies;
        protected IDictionary<int, Country> countries;


        protected State()
        {
        }

        protected State(IDictionary<int, Route> routes, IDictionary<int, Price> prices, IDictionary<int, Delivery> deliveries, IDictionary<int, RouteNode> routeNodes, IDictionary<int, Company> companies, IDictionary<int, Country> countries)
        {
            this.routes = routes;
            this.prices = prices;
            this.deliveries = deliveries;
            this.routeNodes = routeNodes;
            this.companies = companies;
            this.countries = countries;
        }

        #region routes
        public Route GetRoute(int id)
        {
            return routes.ContainsKey(id)? routes[id]:null;
        }

        public IEnumerable<Route> GetAllRoutes()
        {
            return new List<Route>(routes.Values);
        }

        /// <summary>
        /// Initialises the state with a set of active routes in the system. This can only be done once, and must happen
        /// before other methods are used.
        /// 
        /// The given collection is copied into the internal fields of State.
        /// </summary>
        /// <param name="routes">the current routes</param>
        /// <exception cref="IllegalActionException">if has already been initialised.</exception>
        /// <exception cref="ArgumentException">if 'routes' is null</exception>
        public void InitialiseRoutes(IDictionary<int, Route> routes)
        {
            if (this.routes != null)
            {
                throw new IllegalActionException("Cannot set all routes if routes is already initialised.");
            }

            if (routes == null)
            {
                throw new ArgumentException("Cannot set all routes to null.", "routes");
            }

            this.routes = new ConcurrentDictionary<int, Route>(routes);
        }

        #endregion


        #region route nodes
        public RouteNode GetRouteNode(int id)
        {
            return routeNodes.ContainsKey(id) ? routeNodes[id] : null;
        }

        public IEnumerable<RouteNode> GetAllRouteNodes()
        {
            return new List<RouteNode>(routeNodes.Values);
        }

        /// <summary>
        /// Initialises the state with a set of active routeNodes in the system. This can only be done once, and must happen
        /// before other methods are used.
        /// 
        /// The given collection is copied into the internal fields of State.
        /// </summary>
        /// <param name="routeNodes">the current routeNodes</param>
        /// <exception cref="IllegalActionException">if has already been initialised.</exception>
        /// <exception cref="ArgumentException">if 'routeNodes' is null</exception>
        public void InitialiseRouteNodes(IDictionary<int, RouteNode> routeNodes)
        {
            if (this.routeNodes != null)
            {
                throw new IllegalActionException("Cannot set all routes if routeNodes is already initialised.");
            }

            if (routeNodes == null)
            {
                throw new ArgumentException("Cannot set all routeNodes to null.", "routeNodes");
            }
            
            this.routeNodes = new ConcurrentDictionary<int, RouteNode>(routeNodes);
            
        }
        #endregion


        #region prices
        public Price GetPrice(int id)
        {
            return prices.ContainsKey(id)? prices[id]:null;
        }

        public IEnumerable<Price> GetAllPrices()
        {
            return new List<Price>(prices.Values);
        }

        /// <summary>
        /// Initialises the state with a set of active prices in the system. This can only be done once, and must happen
        /// before other methods are used.
        /// 
        /// The given collection is copied into the internal fields of State.
        /// </summary>
        /// <param name="prices">the current prices</param>
        /// <exception cref="IllegalActionException">if has already been initialised.</exception>
        /// <exception cref="ArgumentException">if 'prices' is null</exception>
        public void InitialisePrices(IDictionary<int, Price> prices)
        {

            if (this.prices != null)
            {
                throw new IllegalActionException("Cannot set all routes if prices is already initialised.");
            }

            if (prices == null)
            {
                throw new ArgumentException("Cannot set all prices to null.", "prices");
            }
            
            this.prices = new ConcurrentDictionary<int, Price>(prices); 
        }
        #endregion

        #region countries
        public Country GetCountry(int id)
        {
            return countries.ContainsKey(id) ? countries[id] : null;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            return new List<Country>(countries.Values);
        }

        /// <summary>
        /// Initialises the state with a set of active countries in the system. This can only be done once, and must happen
        /// before other methods are used.
        /// 
        /// The given collection is copied into the internal fields of State.
        /// </summary>
        /// <param name="countries">the current countries</param>
        /// <exception cref="IllegalActionException">if has already been initialised.</exception>
        /// <exception cref="ArgumentException">if 'countries' is null</exception>
        public void InitialiseCountries(IDictionary<int, Country> countries)
        {
            if (this.countries != null)
            {
                throw new IllegalActionException("Cannot set all routes if countries is already initialised.");
            }

            if (countries == null)
            {
                throw new ArgumentException("Cannot set all countries to null.", "countries");
            }

            this.countries = new ConcurrentDictionary<int, Country>(countries);           
        }

        #endregion


        #region deliveries
        public Delivery GetDelivery(int id)
        {
            return deliveries.ContainsKey(id) ? deliveries[id] : null;
        }

        public IEnumerable<Delivery> GetAllDeliveries()
        {
            return new List<Delivery>(deliveries.Values);
        }

        /// <summary>
        /// Initialises the state with a set of active deliveries in the system. This can only be done once, and must happen
        /// before other methods are used.
        /// 
        /// The given collection is copied into the internal fields of State.
        /// </summary>
        /// <param name="deliveries">the current deliveries</param>
        /// <exception cref="IllegalActionException">if has already been initialised.</exception>
        /// <exception cref="ArgumentException">if 'deliveries' is null</exception>
        public void InitialiseDeliveries(IDictionary<int, Delivery> deliveries)
        {
            if (this.deliveries != null)
            {
                throw new IllegalActionException("Cannot set all routes if deliveries is already initialised.");
            }

            if (deliveries == null)
            {
                throw new ArgumentException("Cannot set all deliveries to null.", "deliveries");
            }
            
            this.deliveries = new ConcurrentDictionary<int, Delivery>(deliveries);      
        }

        #endregion


        #region companies
        public Company GetCompany(int id)
        {
            return companies.ContainsKey(id) ? companies[id] : null;
        }

        public IEnumerable<Company> GetAllCompanies()
        {
            return new List<Company>(companies.Values);
        }

        /// <summary>
        /// Initialises the state with a set of active companies in the system. This can only be done once, and must happen
        /// before other methods are used.
        /// 
        /// The given collection is copied into the internal fields of State.
        /// </summary>
        /// <param name="companies">the current companies</param>
        /// <exception cref="IllegalActionException">if has already been initialised.</exception>
        /// <exception cref="ArgumentException">if 'companies' is null</exception>
        public void InitialiseCompanies(IDictionary<int, Company> companies)
        {
            if (this.companies != null)
            {
                throw new IllegalActionException("Cannot set all routes if companies is already initialised.");
            }

            if (companies == null)
            {
                throw new ArgumentException("Cannot set all companies to null.", "companies");
            }
            
            this.companies = new ConcurrentDictionary<int, Company>(companies);        
        }
        #endregion
    }
}
