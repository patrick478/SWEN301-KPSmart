//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// This holds all the current information for Routes, Destinations, Prices, and Deliveries.
    /// 
    /// todo: I'm not sure if we can just make it a repository for dictionaries, and we edit them directly? Will finish the class when we are more sure
    /// 
    /// </summary>
    public class StateSnapshot
    {
        private IDictionary<int, Route> routes;
        private IDictionary<int, Price> prices;
        private IDictionary<int, Delivery> deliveries;
        private IDictionary<int, RouteNode> destinations;
        private IDictionary<int, Company> companies;
        private IDictionary<int, Country> countries; 

        // the time that the stateSnapshot is from. still not sure about this field
        // my idea is that this is the latest LastEdited date from any DataObject in all the collections.
        private DateTime timeRepresented;

        public StateSnapshot()
        {
            routes = new Dictionary<int, Route>();
            prices = new Dictionary<int, Price>();
            deliveries = new Dictionary<int, Delivery>();
            destinations = new Dictionary<int, RouteNode>();
            countries = new Dictionary<int, Country>();
            companies = new Dictionary<int, Company>();
        }

        #region routes
        public void SaveRoute(Route route)
        {
            routes.Add(route.ID, route);
        }

        public void RemoveRoute(Route route)
        {
            routes.Remove(route.ID);
        }

        public IEnumerable<Route> GetAllRoutes()
        {
            return routes.Values;
        }

        public void ResetRoutes(IDictionary<int, Route> allRoutes)
        {
            routes = allRoutes; // TODO - is this safe? should we copy values across perhaps?
        }
        #endregion

        #region prices
        public void SavePrice(Price price)
        {
            prices.Add(price.ID, price);
            UpdateTime(price);
        }

        public void RemovePrice(Price price)
        {
            prices.Remove(price.ID);
            UpdateTime(price);
        }

        public IEnumerable<Price> GetAllPrices()
        {
            return prices.Values;
        }

        public void ResetPrices(IDictionary<int, Price> allPrices)
        {
            prices = allPrices;
            //UpdateTime(allPrices);
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

        public IEnumerable<Country> GetAllCountries()
        {
            return countries.Values;
        }

        public void ResetCountries(IDictionary<int, Country> allCountries)
        {
            countries = allCountries;
            //UpdateTime(allPrices);
        }
        #endregion

        #region deliveries
        #endregion

        #region companies
        #endregion

        #region destinations
        #endregion

        #region time updates

        public DateTime GetTimeRepresented()
        {
            return timeRepresented;
        }

        public void SetTimeRepresented(DateTime newTime)
        {
            this.timeRepresented = newTime;
        }

        /// <summary>
        /// This method updates the timeRepresented field to be the lastEdited date of the object (if it is later than timeRepresented) 
        /// </summary>
        /// <param name="editedObject"></param>
        private void UpdateTime(DataObject editedObject)
        {
            if (timeRepresented.CompareTo(editedObject.LastEdited) < 0)
            {
                timeRepresented = editedObject.LastEdited;
            }
        }

        /// <summary>
        /// This method scans the given datastructure and finds the latest LastEdited date.
        /// It updates the timeRepresented field to be this date. 
        /// 
        /// I'm imagining this will be used when we rollback, or view a different time period.  
        /// 
        /// TODO: when/if we want this method, work out how to pass a collection of a specific instance of DataObject.
        ///  
        /// </summary>
        private void UpdateTime<T>(IDictionary<int, T> newDataStructure ) where T: DataObject
        {
            foreach(var d in newDataStructure.Values)
            {
                UpdateTime(d);
            }
        }
        #endregion

    }
}
