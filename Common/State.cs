﻿using System;
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
        protected IDictionary<int, DomesticPrice> domesticPrices;
        protected IDictionary<int, Delivery> deliveries;
        protected IDictionary<int, RouteNode> routeNodes;
        protected IDictionary<int, Company> companies;
        protected IDictionary<int, Country> countries;

        protected State()
        {
        }

        protected State(IDictionary<int, Route> routes, IDictionary<int, Price> prices, IDictionary<int, Delivery> deliveries, IDictionary<int, RouteNode> routeNodes, IDictionary<int, Company> companies, IDictionary<int, Country> countries, int numberOfEvents)
        {
            this.routes = routes;
            this.prices = prices;
            this.domesticPrices = new Dictionary<int, DomesticPrice>();
            foreach (var kvp in prices)
                if (kvp.Value is DomesticPrice)
                    this.domesticPrices.Add(kvp.Key, kvp.Value as DomesticPrice);

            foreach(var kvp in domesticPrices)
            {
                this.prices.Remove(kvp.Key);
            }

            this.deliveries = deliveries;
            this.routeNodes = routeNodes;
            this.companies = companies;
            this.countries = countries;
            this.NumberOfEvents = numberOfEvents;
        }

        #region events
        public int NumberOfEvents { get; protected set; }
        #endregion

        #region routes
        public Route GetRoute(int id)
        {  
            var route = routes.ContainsKey(id)? routes[id]:null;
            return route;
        }

        public IList<Route> GetAllRoutes()
        {
            return new List<Route>(routes.Values);
        }

        public Price GetRoutePrice (Route route, Priority priority) 
        {
            if (route.Scope == Scope.Domestic)
            {
                return GetAllDomesticPrices().AsQueryable().Where(t => t.Priority == priority).FirstOrDefault<DomesticPrice>();
            }
            else {
                var value = GetAllInternationalPrices().AsQueryable().Where(t => t.Origin.Equals(route.Origin) && t.Destination.Equals(route.Destination) && t.Priority.Equals(priority)).FirstOrDefault<Price>();
                var attempt = (priority == Priority.Air ? Priority.Standard : Priority.Air);
                if(value == null)
                    value = GetAllInternationalPrices().AsQueryable().Where(t => t.Origin.Equals(route.Origin) && t.Destination.Equals(route.Destination) && t.Priority.Equals(attempt)).FirstOrDefault<Price>();
                return value;
            }   
        }
        #endregion


        #region route nodes
        public virtual RouteNode GetRouteNode(int id)
        {
            return routeNodes.ContainsKey(id) ? routeNodes[id] : null;
        }

        public virtual IList<RouteNode> GetAllRouteNodes()
        {
            return new List<RouteNode>(routeNodes.Values);
        }
        #endregion


        #region prices
        public Price GetPrice(int id)
        {
            return prices.ContainsKey(id)? prices[id]:null;
        }

        public IList<Price> GetAllPrices()
        {
            return new List<Price>(prices.Values);
        }

        public IList<Price> GetAllInternationalPrices()
        {
            return new List<Price>(prices.Values.Where(t => !(t is DomesticPrice)));
        }
        #endregion

        #region domesticPrices
        public DomesticPrice GetDomesticPrice (int id)
        {
            return domesticPrices.ContainsKey(id) ? domesticPrices[id] : null;
        }

        public IList<DomesticPrice> GetAllDomesticPrices ()
        {
            return new List<DomesticPrice>(domesticPrices.Values);
        }
        #endregion


        #region countries
        public Country GetCountry(int id)
        {
            return countries.ContainsKey(id) ? countries[id] : null;
        }

        public IList<Country> GetAllCountries()
        {
            return new List<Country>(countries.Values);
        }
        #endregion


        #region deliveries
        public virtual Delivery GetDelivery(int id)
        {
            return deliveries.ContainsKey(id) ? deliveries[id] : null;
        }

        public virtual IList<Delivery> GetAllDeliveries()
        {
            return new List<Delivery>(deliveries.Values);
        }
        #endregion


        #region companies
        public Company GetCompany(int id)
        {
            return companies.ContainsKey(id) ? companies[id] : null;
        }

        public IList<Company> GetAllCompanies()
        {
            return new List<Company>(companies.Values);
        }
        #endregion
    }
}
