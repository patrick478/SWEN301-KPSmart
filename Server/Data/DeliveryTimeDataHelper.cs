using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

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

        public void Update (int route_id, IList<WeeklyTime> departureTimes)
        {
            throw new NotImplementedException();
        }

        public void Create (int route_id, IList<WeeklyTime> departureTimes)
        {
            throw new NotImplementedException();
        }

        public int GetId (int route_id, WeeklyTime departureTime)
        {
            throw new NotImplementedException();
        }
    }
}
