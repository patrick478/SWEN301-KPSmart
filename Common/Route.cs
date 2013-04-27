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

        public Route(Company company, TransportType transportType, RouteNode origin, RouteNode routeNode)
        {
            Company = company;
            TransportType = transportType;
            Origin = origin;
            Destination = routeNode;

            this.departureTimes = new List<WeeklyTime>();
        }

        // fields that determine uniqueness of the Route
        //----------------------------------------------
        public Company Company { get; private set; }
        public TransportType TransportType { get; private set; }
        public RouteNode Origin { get; private set; }
        public RouteNode Destination { get; private set; }


        // other fields
        //-------------

        // all the departure times this route offers
        private List<WeeklyTime> departureTimes;
        public IEnumerable<WeeklyTime> DepartureTimes
        {
            get { return departureTimes; }
            set { this.departureTimes = new List<WeeklyTime>(value); }
        }

        // domestic or international
        public Scope Scope { get; set; }

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

        // in cents
        public int PricePerGram { get; set; }

        // in cents
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
            var other = obj as Route;

            // false if not Route
            if (other == null)
                return false;

            // false if both ID's are initialised and they aren't the same
            if (this.ID != 0 && other.ID != 0 && this.ID != other.ID)
                return false;

            // false if all these not the same
            bool sameCompany = this.Company == other.Company;
            bool sameOrigin = this.Origin == other.Origin;
            bool sameDestination = this.Destination == other.Destination;
            bool sameTransType = this.TransportType == other.TransportType;

            return sameCompany && sameOrigin && sameDestination && sameTransType;
        }


    }
}
