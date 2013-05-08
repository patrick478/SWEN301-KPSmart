using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Gui;

namespace Server.Data
{
    public class DepartureTimeDataHelper
    {
        protected string EVENT_ID = "event_id";
        protected string TABLE_NAME;
        protected string ID_COL_NAME;

        public IList<WeeklyTime> Load (int route_id)
        {
            throw new NotImplementedException();
        }

        public IList<WeeklyTime> Load (int route_id, DateTime snapshotTime)
        {
            throw new NotImplementedException();
        }

        public void Update (int route_id, int event_id, IList<WeeklyTime> departureTimes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves all the departure times for the new route.
        /// </summary>
        /// <param name="route_id">the id of the newly created route</param>
        /// <param name="event_id">the event id of creating the route</param>
        /// <param name="departureTimes">the departure times to save</param>
        public void Create (int route_id, int event_id, IList<WeeklyTime> departureTimes)
        {
            if (route_id == 0)
                throw new ArgumentException("Route_id cannot be zero", "route_id");

            if (departureTimes == null)
                throw new ArgumentException("departureTimes cannot be null", "departureTimes");

            // BEGIN LOCK HERE
            lock (Database.Instance)
            {
                // check that no entries for that route_id
                var sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "route_id", route_id.ToString(), new string[] { ID_COL_NAME });
                var rows = Database.Instance.FetchRows(sql);
                if (rows.Length != 0)
                    throw new DatabaseException("That route already has active departure time entries.  Should use Update method instead.");

                // save all the entries
                foreach (WeeklyTime departureTime in departureTimes) 
                {
                    sql = SQLQueryBuilder.CreateNewRecord(TABLE_NAME, ID_COL_NAME, new[] { EVENT_ID, "route_id", "weekly_time" }, new string[] { event_id.ToString(), route_id.ToString(), departureTime.Value.Ticks.ToString() });
                    Database.Instance.InsertQuery(sql);
                }    
            }
            // END LOCK HERE

            Logger.WriteLine("Saved {0} departure times for new route: {1}", departureTimes.Count, route_id);
        }

        /// <summary>
        /// Returns the departure_time_id of the given combination.
        /// 
        /// Returns 0 if it doesn't exist.
        /// </summary>
        /// <param name="route_id"></param>
        /// <param name="departureTime"></param>
        /// <returns></returns>
        private int GetId (int route_id, WeeklyTime departureTime)
        {
            throw new NotImplementedException();
        }
    }
}
