﻿using System;
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
            throw new NotImplementedException();
        }

        public DateTime GetDateTimeOfFirstEvent()
        {
            var sql = "SELECT MIN(created) FROM 'events'";
            var result = Database.Instance.FetchRow(sql);
            if (result.Length == 0) throw new Exception();
            DateTime retValue;
            try
            {
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
