using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Data;
using Common;

namespace Server.Business
{
    public class StatisticsService
    {
        private RouteDataHelper routeDataHelper;
        private PriceDataHelper priceDataHelper;
        private EventDataHelper eventDataHelper;
        private RouteNodeDataHelper routeNodeDataHelper;
        private CountryDataHelper countryDataHelper;
        private CompanyDataHelper companyDataHelper;
        private DeliveryDataHelper deliveryDataHelper;

        public StatisticsService () 
        {
            this.routeDataHelper = new RouteDataHelper();
            this.priceDataHelper = new PriceDataHelper();
            this.eventDataHelper = new EventDataHelper();
            this.routeNodeDataHelper = new RouteNodeDataHelper();
            this.countryDataHelper = new CountryDataHelper();
            this.companyDataHelper = new CompanyDataHelper();
            this.deliveryDataHelper = new DeliveryDataHelper();    
        }

        /// <summary>
        /// Returns the statistics for the given point in time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public Statistics GetStatisticsFromPoint (DateTime time) 
        {

            var routes = routeDataHelper.LoadAll(time);
            var prices = priceDataHelper.LoadAll(time);
            var routeNodes = routeNodeDataHelper.LoadAll(time);
            var countries = countryDataHelper.LoadAll(time);
            var companies = companyDataHelper.LoadAll(time);
            var deliveries = deliveryDataHelper.LoadAll(time);
            var events = eventDataHelper.GetNumberOfEvents(time);

            var stateSnapshot = new StateSnapshot(time, routes, prices, deliveries, routeNodes, companies, countries, events);

            var statistics = new Statistics(stateSnapshot);

            return statistics;
        }



    }
}
