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
        protected readonly IDictionary<int, Route> routes;
        protected readonly IDictionary<int, Price> prices;
        protected readonly IDictionary<int, Delivery> deliveries;
        protected readonly IDictionary<int, RouteNode> routeNodes;
        protected readonly IDictionary<int, Company> companies;
        protected readonly IDictionary<int, Country> countries;

        protected State(IDictionary<int, Route> routes, 
            IDictionary<int, Price> prices, 
            IDictionary<int, Delivery> deliveries,
            IDictionary<int, RouteNode> routeNodes,
            IDictionary<int, Company> companies,
            IDictionary<int, Country> countries )
        {
            this.routes = new ConcurrentDictionary<int, Route>(routes);
            this.prices = new ConcurrentDictionary<int, Price>(prices);
            this.deliveries = new ConcurrentDictionary<int, Delivery>(deliveries);
            this.routeNodes = new ConcurrentDictionary<int, RouteNode>(routeNodes);
            this.countries = new ConcurrentDictionary<int, Country>(countries);
            this.companies = new ConcurrentDictionary<int, Company>(companies);
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
        #endregion
    }
}
