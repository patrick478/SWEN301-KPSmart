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
        protected string TABLE_NAME = "departure_times";
        protected string ID_COL_NAME = "departure_time_id";

        public IList<WeeklyTime> Load (int route_id)
        {
            // check values
            if (route_id == 0)
                throw new ArgumentException("Route_id cannot be zero", "route_id");

            var list = new List<WeeklyTime>();
            lock (Database.Instance) 
            {
                var sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "route_id", route_id.ToString(), new[]{"weekly_time"});

                //sql = "SELECT 'weekly_time' FROM `departure_times` WHERE active=1 AND route_id=1";

                var rows = Database.Instance.FetchRows(sql);

                foreach (object[] row in rows) 
                {
                    long ticks = (long)row[0];
                    list.Add(new WeeklyTime(ticks));
                }
            }

            list.Sort();
            return list;
        }

        public IList<WeeklyTime> Load (int route_id, DateTime snapshotTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// You are allowed a route with no departure times, so this method doesn't check that departure times exist already.
        /// </summary>
        /// <param name="route_id"></param>
        /// <param name="event_id"></param>
        /// <param name="departureTimes"></param>
        public void Update (int route_id, int event_id, List<WeeklyTime> departureTimes)
        {
            // check values
            if (route_id == 0)
                throw new ArgumentException("Route_id cannot be zero", "route_id");

            if (event_id == 0)
                throw new ArgumentException("Event_id cannot be zero", "event_id");

            if (departureTimes == null)
                throw new ArgumentException("departureTimes cannot be null", "departureTimes");


            var currentList = Load(route_id);
            var difference = currentList.Except(departureTimes);

            if (difference.Count() == 0)
                return; // you are allowed to call update even if it doesn't make a difference.


            // get times to add and remove
            var toDelete = difference.Intersect(currentList);
            var toAdd = difference.Intersect(departureTimes);


            lock (Database.Instance) 
            {

                // save all new entries
                foreach (WeeklyTime time in toAdd)
                {
                    var sql = SQLQueryBuilder.CreateNewRecord(TABLE_NAME, ID_COL_NAME, new[] { EVENT_ID, "route_id", "weekly_time" }, new string[] { event_id.ToString(), route_id.ToString(), time.Value.Ticks.ToString() });
                    Database.Instance.InsertQuery(sql);
                }  

                // delete all non existing entries
                foreach (WeeklyTime time in toDelete)
                {
                    // get the id
                    int departure_time_id = GetId(route_id, time);

                    // set entry to inactive
                    var sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME, departure_time_id);
                    Database.Instance.InsertQuery(sql);

                    // insert new 'deleted' row
                    sql = SQLQueryBuilder.InsertFields(TABLE_NAME,
                                                       new string[] { EVENT_ID, ID_COL_NAME, "active", "route_id", "weekly_time" },
                                                       new string[] { event_id.ToString(), departure_time_id.ToString(), "-1", route_id.ToString(), "" });
                    Database.Instance.InsertQuery(sql);
                } 
            
            }
            
        }

        /// <summary>
        /// Saves all the departure times for the new route.
        /// </summary>
        /// <param name="route_id">the id of the newly created route</param>
        /// <param name="event_id">the event id of creating the route</param>
        /// <param name="departureTimes">the departure times to save</param>
        public void Create (int route_id, int event_id, IList<WeeklyTime> departureTimes)
        {
            // check values
            if (route_id == 0)
                throw new ArgumentException("Route_id cannot be zero", "route_id");

            if (event_id == 0)
                throw new ArgumentException("Event_id cannot be zero", "event_id");

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
            // check arguments
            if (route_id <= 0)
                throw new ArgumentException("Route_id cannot be less than or equal to zero.", "route_id");

            if (departureTime == null)
                throw new ArgumentException("departureTime cannot be null", "departureTime");

            // get the id
            int id = 0;
            lock (Database.Instance) 
            {
                var sql = string.Format("SELECT {1} FROM `{0}` WHERE active=1 AND route_id='{2}' AND weekly_time='{3}'", TABLE_NAME, ID_COL_NAME, route_id, departureTime.Value.Ticks.ToString());
                var row = Database.Instance.FetchRow(sql);
                if (row.Length != 0)
                    id =  row[0].ToInt();
            }

            // return
            return id;
        }
    }
}
