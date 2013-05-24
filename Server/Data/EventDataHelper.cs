using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Data
{
    public class EventDataHelper
    {

        /// <summary>
        /// Returns the total number of events.
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfEvents()
        {
            lock (Database.Instance)
            {
                var sql = "SELECT * FROM `events`";
                var result = Database.Instance.FetchRows(sql);
                return result.Length;
            }
        }

        /// <summary>
        /// Returns the total number of events that happened before the given date.
        /// </summary>
        /// <param name="time">date up to which you are interested in</param>
        /// <returns></returns>
        public int GetNumberOfEvents(DateTime time)
        {
            lock (Database.Instance) 
            {
                var sql = SQLQueryBuilder.SelectFieldsAtDateTime("events", new string[] { "id"}, "id", time);
                var rows = Database.Instance.FetchRows(sql);
                return rows.Length;      
            }
        }

        public DateTime GetDateTimeOfFirstEvent()
        {
            var sql = "SELECT MIN(created) FROM 'events'";
            var result = Database.Instance.FetchRow(sql);

            if (result[0] is System.DBNull) return DateTime.Now;
            DateTime retValue;
            try
            {
                if (result[0] is string)
                    retValue = DateTime.Parse((string)result[0]);
                else
                    retValue = (DateTime)result[0];
            }
            catch(Exception ex)
            {
                throw new DatabaseException("Unable to fetch first event data: " + ex.ToString());
            }

            return retValue;
        }
    }
}
