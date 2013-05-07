﻿//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System;
using System.Collections.Generic;
using Common;
using Server.Gui;

namespace Server.Data
{
    /// <summary>
    /// The idea is this class is used to extract Routes from the DB, and save routes to the DB.  
    /// It should be used by the RouteService class.
    /// 
    /// Maybe it returns the DateTime instead of void so StateSnapshot can be updated???
    /// </summary>
    public class RouteDataHelper: DataHelper<Route>
    {

        private PriceDataHelper priceDataHelper;
        private DepartureTimeDataHelper deliveryTimeDataHelper;
        private RouteNodeDataHelper routeNodeDataHelper;
        private CompanyDataHelper companyDataHelper;

        public RouteDataHelper()
        {
            TABLE_NAME = "routes";
            ID_COL_NAME = "route_id";

            priceDataHelper = new PriceDataHelper();
            deliveryTimeDataHelper = new DepartureTimeDataHelper();
            routeNodeDataHelper = new RouteNodeDataHelper();
            companyDataHelper = new CompanyDataHelper();
        }

        public override Route Load(int id)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<int, Route> LoadAll()
        {
            throw new NotImplementedException();
        }

        public override IDictionary<int, Route> LoadAll(DateTime snapshotTime)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Route obj)
        {
            throw new NotImplementedException();
        }

        public override void Update(Route obj)
        {
            throw new NotImplementedException();
        }

        public void AddDeliveryTime(Route route, WeeklyTime deliveryTime)
        {
            throw new NotImplementedException();
        }

        public void DeleteDeliveryTime(Route route, WeeklyTime deliveryTime)
        {
            throw new NotImplementedException();
        }

        public override void Create(Route route)
        {
            object[] row;

            // check it is legal
            int routeNodeId = GetId(route);
            if (routeNodeId != 0)
                throw new DatabaseException(String.Format("That route already exists: {0}", routeNodeId));

            // load ids of fields
            int origin_id = routeNodeDataHelper.GetId(route.Origin);
            int destination_id = routeNodeDataHelper.GetId(route.Destination);
            int company_id = companyDataHelper.GetId(route.Company);
            string transport_type = route.TransportType == null ? "" : Enum.GetName(typeof(TransportType), route.TransportType);

            // check all the fields
            if (origin_id == 0)
                throw new DatabaseException(String.Format("Origin could not be found: {0}", route.Origin));

            if (destination_id == 0)
                throw new DatabaseException(String.Format("Destination could not be found: {0}", route.Destination));

            if (origin_id == destination_id)
                throw new DatabaseException("Origin and destination cannot be the same");

            if (company_id == 0)
                throw new DatabaseException(String.Format("Company could not be found: {0}", route.Company));

            if (transport_type == String.Empty)
                throw new DatabaseException(String.Format("Route must have a transport type: {0}", route));

            if (route.Duration == 0)
                throw new DatabaseException(String.Format("Route duration cannot be 0: {0}", route));

            if (route.MaxWeight <= 0)
                throw new DatabaseException(String.Format("Route maxWeight cannot be less than or equal to 0: {0}", route));
            
            if (route.MaxVolume <= 0)
                throw new DatabaseException(String.Format("Route maxVolume cannot be less than or equal to 0: {0}", route));

            if (route.CostPerGram <= 0)
                throw new DatabaseException(String.Format("Route costPerGram cannot be less than or equal to 0: {0}", route));

            if (route.CostPerCm3 <= 0)
                throw new DatabaseException(String.Format("Route costPerCm3 cannot be less than or equal to 0: {0}", route));

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get event number
                var sql = SQLQueryBuilder.SaveEvent(ObjectType.Route, EventType.Create);
                long eventId = Database.Instance.InsertQuery(sql);

                // insert the route record
                sql = SQLQueryBuilder.CreateNewRecord(    TABLE_NAME,
                                                          ID_COL_NAME,
                                                          new string[] 
                                                              { 
                                                                  EVENT_ID, 
                                                                  "origin_id", 
                                                                  "destination_id", 
                                                                  "company_id", 
                                                                  "transport_type", 
                                                                  "duration", 
                                                                  "max_weight", 
                                                                  "max_volume", 
                                                                  "cost_per_gram",
                                                                  "cost_per_cm3"                                                            
                                                              },
                                                          new string[] 
                                                              { 
                                                                  eventId.ToString(), 
                                                                  origin_id.ToString(), 
                                                                  destination_id.ToString(),
                                                                  company_id.ToString(),
                                                                  transport_type,
                                                                  route.Duration.ToString(),
                                                                  route.MaxWeight.ToString(),
                                                                  route.MaxVolume.ToString(),
                                                                  route.CostPerGram.ToString(),
                                                                  route.CostPerCm3.ToString()
                                                              });
                long inserted_id = Database.Instance.InsertQuery(sql);

                // get id and LastEdited
                var fields = new string[] { ID_COL_NAME, "created" };
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "id", inserted_id.ToString(), fields);
                row = Database.Instance.FetchRow(sql);
            }
            // LOCK ENDS HERE

            // set id and lastedited
            route.ID = row[0].ToInt();
            route.LastEdited = (DateTime)row[1];

            // save all the departure times
            deliveryTimeDataHelper.Create(route.ID, route.DepartureTimes);
                        
            Logger.WriteLine("Created route: " + route);
        }

        public override int GetId(Route country)
        {
            throw new NotImplementedException();
        }
    }
}
