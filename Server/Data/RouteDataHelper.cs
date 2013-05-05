//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System;
using System.Collections.Generic;
using Common;

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

        public override void Create(Route obj)
        {
            throw new NotImplementedException();
        }

        public override int GetId(Route country)
        {
            throw new NotImplementedException();
        }
    }
}
