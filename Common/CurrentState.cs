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
        //todo - should this be singleton? Should only be one instance


        public CurrentState(IDictionary<int, Route> routes,
                            IDictionary<int, Price> prices,
                            IDictionary<int, Delivery> deliveries,
                            IDictionary<int, RouteNode> routeNodes,
                            IDictionary<int, Company> companies,
                            IDictionary<int, Country> countries): base(routes, prices, deliveries, routeNodes, companies, countries)
        {
        }

        #region route
        public void SaveRoute(Route route)
        {
            routes.Add(route.ID, route);
        }

        public void RemoveRoute(Route route)
        {
            routes.Remove(route.ID);
        }
        #endregion


        #region route node
        public void SaveRouteNode(RouteNode routeNode)
        {
            routeNodes.Add(routeNode.ID, routeNode);
        }

        public void RemoveRouteNode(RouteNode routeNode)
        {
            routeNodes.Remove(routeNode.ID);
        }
        #endregion


        #region prices
        public void SavePrice(Price price)
        {
            prices.Add(price.ID, price);
        }

        public void RemovePrice(Price price)
        {
            prices.Remove(price.ID);

        }
        #endregion


        #region countries
        public void SaveCountry(Country country)
        {
            countries.Add(country.ID, country);
        }

        public void RemoveCountry(Country country)
        {
            prices.Remove(country.ID);
        }
        #endregion


        #region deliveries
        public void SaveDelivery(Delivery delivery)
        {
            deliveries.Add(delivery.ID, delivery);
        }

        public void RemoveDelivery(Delivery delivery)
        {
            deliveries.Remove(delivery.ID);
        }
        #endregion


        #region companies
        public void SaveCompany(Company company)
        {
            companies.Add(company.ID, company);
        }

        public void RemoveCompany(Company company)
        {
            companies.Remove(company.ID);
        }
        #endregion

    }
}
