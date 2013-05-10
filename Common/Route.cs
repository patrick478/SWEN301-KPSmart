//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System.Collections.Generic;
using System;

namespace Common
{
    /// <summary>
    /// This represents a Route offered by a Company.
    /// </summary>
    public class Route: DataObject
    {


        public Route()
        {
            this.departureTimes = new List<WeeklyTime>();
        }

        // fields that determine uniqueness of the Route
        //----------------------------------------------
        public Company Company { get; set; }
        public TransportType TransportType { get; set; }
        public RouteNode Origin { get; set; }
        public RouteNode Destination { get; set; }


        // other fields
        //-------------

        // all the departure times this route offers
        private List<WeeklyTime> departureTimes;
        public IList<WeeklyTime> DepartureTimes
        {
            get { return departureTimes; }
            set { this.departureTimes = new List<WeeklyTime>(value); }
        }

        // domestic or international
        public Scope Scope 
        {
            get
            {
                if (Origin.GetType() == typeof(InternationalPort) || Destination.GetType() == typeof(InternationalPort))
                    return Scope.International;
                else
                    return Scope.Domestic;
            }
        }

        //in minutes  
        public int Duration { get; set; }

        // in grams
        public int MaxWeight { get; set; }

        // In cubic cm
        public int MaxVolume { get; set; }

        // (should this be a double?)  Maybe make a class "MoneyValue"?
        // in cents  
        public int CostPerGram { get; set; }

        // in cents  
        public int CostPerCm3 { get; set; }

        // in cents - this isn't a property belonging to the Route itself.
        public int PricePerGram { get; set; }

        // in cents - this isn't a property belonging to the Route itself.
        public int PricePerCm3 {get; set; }
        
        public void AddDepartureTime(WeeklyTime weeklyTime)
        {
            this.departureTimes.Add(weeklyTime);
            this.departureTimes.Sort();
        }

        /// <summary>
        /// TODO: not sure if this will work - need to make WeeklyTime comparible so if it has the same value, it is equal?  
        /// Not sure if Remove listens to this, or looks at specific instances?
        /// </summary>
        /// <param name="weeklyTime"></param>
        public void RemoveDepartureTime(WeeklyTime weeklyTime)
        {
            
            this.departureTimes.Remove(weeklyTime);
        }

        /// <summary>
        /// This method returns the next RouteInstance to depart from the given the date time.
        /// 
        /// If the Route has no deliveryTimes, the method will return null.  Otherwise, it will return a RouteInstance with
        /// the DateTime of the next delivery time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public RouteInstance GetNextDeparture(DateTime time){

            if (departureTimes.Count == 0) {
                return null;
            }
            
            var asWeeklyTime = new WeeklyTime(time);

            // find next departure time
            WeeklyTime nextDeparture = null;
            foreach (WeeklyTime departureTime in departureTimes)
            {

                var value = asWeeklyTime.CompareTo(departureTime);
                if ( value <= 0) 
                {
                    nextDeparture = departureTime;
                    break;
                }
            }

            int extraMinutes = 0; // extra time to add if the departureTime is next week.

            // if no deliveries this week, get first one next week.
            if (nextDeparture == null) {
                nextDeparture = departureTimes[0];
                extraMinutes = WeeklyTime.MINUTES_IN_A_WEEK;
            }

            // work out datetime
            int minutesDifference = (int)(nextDeparture.Value.TotalMinutes - asWeeklyTime.Value.TotalMinutes) + extraMinutes;
            var departureDateTime = time.AddMinutes(minutesDifference);

            // create and return route instance
            return new RouteInstance(this, departureDateTime);
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Route) obj);
        }

        protected bool Equals(Route other)
        {
            return Equals(Company, other.Company) && TransportType == other.TransportType && Equals(Origin, other.Origin) && Equals(Destination, other.Destination);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Company != null ? Company.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)TransportType;
                hashCode = (hashCode * 397) ^ (Origin != null ? Origin.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Destination != null ? Destination.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToNetString()
        {
            return NetCodes.BuildNetworkString(base.ToNetString(), NetCodes.OBJECT_ROUTE, Convert.ToString(Origin.ID), Convert.ToString(Destination.ID), TransportType.ToNetString(), Convert.ToString(PricePerGram), Convert.ToString(PricePerCm3), Convert.ToString(MaxWeight), Convert.ToString(MaxVolume), Convert.ToString(Duration), WeeklyTime.BuildTimesNetString(DepartureTimes));
        }

    }
}
