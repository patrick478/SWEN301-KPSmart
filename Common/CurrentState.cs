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

        public CurrentState()
        {
        }

        #region route
        public void SaveRoute(Route route)
        {
            routes.Add(route.ID, route);
        }

        public void RemoveRoute(int id)
        {
            routes.Remove(id);
        }
        #endregion


        #region route node
        public void SaveRouteNode(RouteNode routeNode)
        {
            routeNodes.Add(routeNode.ID, routeNode);
        }

        public void RemoveRouteNode(int id)
        {
            routeNodes.Remove(id);
        }
        #endregion


        #region prices
        public void SavePrice(Price price)
        {
            prices.Add(price.ID, price);
        }

        public void RemovePrice(int id)
        {
            prices.Remove(id);

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
        #endregion


        #region deliveries
        public void SaveDelivery(Delivery delivery)
        {
            deliveries.Add(delivery.ID, delivery);
        }

        public void RemoveDelivery(int id)
        {
            deliveries.Remove(id);
        }
        #endregion


        #region companies
        public void SaveCompany(Company company)
        {
            companies.Add(company.ID, company);
        }

        public void RemoveCompany(int id)
        {
            companies.Remove(id);
        }
        #endregion

    }
}
