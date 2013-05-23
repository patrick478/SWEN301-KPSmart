using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// This class is the CurrentState of the server.  
    /// Services should refer to this class for reads, and keep it updated when there are updates.
    /// 
    /// It is also for pathfinding to use.
    /// </summary>
    public class CurrentState: State
    {
        /// <summary>
        /// Creates an uninitialised CurrentState.  Its internal state must be initialised via the Initialise...() methods.
        /// </summary>
        public CurrentState()
        {
        }

        #region events

        /// <summary>
        /// This can only be called if the number of events is zero.
        /// </summary>
        /// <param name="newNumber"></param>
        public void SetNumberOfEvents(int newNumber)
        {
            if (EventsInitialised())
                throw new IllegalActionException("Cannot reset NumberOfEvents as it has already been initialised.");

            if(newNumber == 0)
                throw new ArgumentException("Cannot set number of events to zero for current state.");
            
            NumberOfEvents = newNumber;
        }

        public void IncrementNumberOfEvents()
        {
            NumberOfEvents += 1;
        }

        public bool EventsInitialised()
        {
            return NumberOfEvents != 0;
        }

        #endregion

        #region route
        public void SaveRoute(Route route)
        {
            routes[route.ID] = route;
        }

        public void RemoveRoute(int id)
        {
            routes.Remove(id);
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

        public bool RoutesInitialised {
            get { return routes != null; }
        }
        #endregion


        #region route node
        public virtual void SaveRouteNode(RouteNode routeNode)
        {
            routeNodes[routeNode.ID] = routeNode;
        }

        public virtual void RemoveRouteNode(int id)
        {
            routeNodes.Remove(id);
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
        public virtual void InitialiseRouteNodes(IDictionary<int, RouteNode> routeNodes)
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

        public virtual bool RouteNodesInitialised
        {
            get { return routeNodes != null; }
        }
        #endregion


        #region prices
        public void SavePrice(Price price)
        {
            prices[price.ID] = price;
        }

        public void RemovePrice(int id)
        {
            prices.Remove(id);
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

            // add all domestic prices
            domesticPrices = new ConcurrentDictionary<int, DomesticPrice>();
            foreach (Price p in prices.Values) 
            {
                if (p.GetType() == typeof(DomesticPrice))
                {
                    domesticPrices[p.ID] = (p as DomesticPrice);                
                }

            }

            // remove domestic prices from prices
            foreach (Price p in domesticPrices.Values) 
            {
                prices.Remove(p.ID);
            }

            // initialise prices
            this.prices = new ConcurrentDictionary<int, Price>(prices);
        }

        public bool PricesInitialised
        {
            get { return prices != null && domesticPrices != null; }
        }
        #endregion


        #region domesticPrices
        public void SaveDomesticPrice (DomesticPrice price)
        {
            domesticPrices[price.ID] = price;
        }

        public void RemoveDomesticPrice (int id)
        {
            domesticPrices.Remove(id);
        }
        #endregion

        #region countries
        public void SaveCountry(Country country)
        {
            countries[country.ID] = country;
        }

        public void RemoveCountry(int id)
        {
            countries.Remove(id);
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

        public bool CountriesInitialised
        {
            get { return countries != null; }
        }
        #endregion


        #region deliveries
        public virtual void SaveDelivery(Delivery delivery)
        {
            deliveries[delivery.ID] = delivery;
        }

        public virtual void RemoveDelivery(int id)
        {
            deliveries.Remove(id);
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
        public virtual void InitialiseDeliveries(IDictionary<int, Delivery> deliveries)
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

        public virtual bool DeliveriesInitialised
        {
            get { return deliveries != null; }
        }
        #endregion


        #region companies
        public void SaveCompany(Company company)
        {
            companies[company.ID] = company;
        }

        public void RemoveCompany(int id)
        {
            companies.Remove(id);
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

        public bool CompaniesInitialised
        {
            get { return companies != null; }
        }
        #endregion

    }
}
