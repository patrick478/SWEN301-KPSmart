//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// This represents a Route offered by a Company.
    /// </summary>
    public class Route
    {

        public Route(Company company, TransportType transportType, Destination origin, Destination destination)
        {
            Company = company;
            TransportType = transportType;
            Origin = origin;
            Destination = destination;

            this.departureTimes = new List<WeeklyTime>();
        }

        // fields that determine uniqueness of the Route
        //----------------------------------------------
        public Company Company { get; private set; }
        public TransportType TransportType { get; private set; }
        public Destination Origin { get; private set; }
        public Destination Destination { get; private set; }


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

        // in cents  (should this be a double?)  Maybe make a class "MoneyValue"?
        public int CostPerGram { get; set; }

        // in cents  (should this be a double?)
        public int CostPerCm3 { get; set; }


        
        public void AddDepartureTime(WeeklyTime weeklyTime)
        {
            this.departureTimes.Add(weeklyTime);
        }

        public void RemoveDepartureTime(WeeklyTime weeklyTime)
        {
            // TODO: not sure if this will work - need to make WeeklyTime comparible so if it has the same value, it is equal?  Not sure if Remove listens to this, or looks at specific instances?
            this.departureTimes.Remove(weeklyTime);
        }
    }
}
