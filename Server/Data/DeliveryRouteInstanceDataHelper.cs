using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Gui;

namespace Server.Data
{
    public class DeliveryRouteInstanceDataHelper
    {
        protected string EVENT_ID = "event_id";
        protected string TABLE_NAME = "delivery_route_instances";
        protected string ID_COL_NAME = "delivery_id";
        private RouteDataHelper routeDataHelper;

        public DeliveryRouteInstanceDataHelper() 
        {
            this.routeDataHelper = new RouteDataHelper();
        }

        public List<RouteInstance> Load(int delivery_id)
        {
            // check values
            if (delivery_id == 0)
                throw new ArgumentException("Delivery_id cannot be zero", "delivery_id");

            var list = new List<RouteInstance>();

            object[] rows;
            lock (Database.Instance)
            {
                var sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "delivery_id", delivery_id.ToString(), new[] { "route_id", "departure_time" });
                rows = Database.Instance.FetchRows(sql);
            }

            foreach (object[] row in rows)
            {
                int route_id = row[0].ToInt();
                DateTime departureTime = (DateTime)row[1];
                Route route = routeDataHelper.Load(route_id, true);

                list.Add(new RouteInstance(route, departureTime));
            }

            list.Sort();
            return list;
        }

        public List<RouteInstance> Load(int delivery_id, DateTime snapshotTime)
        {
            // check values
            if (delivery_id == 0)
                throw new ArgumentException("Delivery_id cannot be zero", "delivery_id");

            var list = new List<RouteInstance>();

            object[] rows;
            lock (Database.Instance)
            {
                var sql = SQLQueryBuilder.SelectFieldsWhereFieldsEqualAtTimeStamp(TABLE_NAME, new string[] {"delivery_id"}, new string[] {delivery_id.ToString()}, new[] { "route_id", "departure_time" }, ID_COL_NAME, snapshotTime);
                rows = Database.Instance.FetchRows(sql);
            }

            foreach (object[] row in rows)
            {
                int route_id = row[0].ToInt();
                DateTime departureTime = (DateTime)row[1];
                Route route = routeDataHelper.Load(route_id, true);

                list.Add(new RouteInstance(route, departureTime));
            }

            list.Sort();
            return list;
        }


        /// <summary>
        /// Saves all the departure times for the new route.
        /// </summary>
        /// <param name="route_id">the id of the newly created route</param>
        /// <param name="event_id">the event id of creating the route</param>
        /// <param name="departureTimes">the departure times to save</param>
        public void Create(int delivery_id, int event_id, List<RouteInstance> routeInstances)
        {
            // check values
            if (delivery_id == 0)
                throw new ArgumentException("delivery_id cannot be zero", "delivery_id");

            if (event_id == 0)
                throw new ArgumentException("event_id cannot be zero", "event_id");

            if (routeInstances == null)
                throw new ArgumentException("routeInstances cannot be null", "routeInstances");

            // BEGIN LOCK HERE
            lock (Database.Instance)
            {
                // check that no entries for that delivery_id
                var sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "delivery_id", delivery_id.ToString(), new string[] { ID_COL_NAME });
                var rows = Database.Instance.FetchRows(sql);
                if (rows.Length != 0)
                    throw new DatabaseException("That delivery already has route instance entries.");

                // save all the entries
                foreach (RouteInstance routeInstance in routeInstances)
                {
                    sql = SQLQueryBuilder.InsertFields(TABLE_NAME,  new[] { EVENT_ID, "active", ID_COL_NAME, "route_id", "departure_time" }, new string[] { event_id.ToString(), "1", delivery_id.ToString(), routeInstance.Route.ID.ToString(), routeInstance.DepartureTime.ToString("yyyy-MM-dd HH:mm:ss") });
                    Database.Instance.InsertQuery(sql);
                }

            }
            // END LOCK HERE

            Logger.WriteLine("Saved {0} routeInstances for new delivery: {1}", routeInstances.Count, delivery_id);
        }

    }
}
