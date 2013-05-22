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

        // whether the route has been deleted or not
        public bool Active { get; set; }


        private Company company;
        public Company Company
        {
            get { return company; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("Company", "Company cannot be set to null.");

                this.company = value;
            }
        }

        private TransportType transportType;
        public TransportType TransportType
        {
            get { return transportType; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("TransportType", "TransportType cannot be set to null.");

                this.transportType = value;
            }
        }


        private RouteNode origin;
        public RouteNode Origin
        {
            get { return origin; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("Origin", "Origin cannot be set to null.");

                if (value.Equals(Destination))
                    throw new InvalidObjectStateException("Origin", "Origin cannot be the same as the Destination.");

                this.origin = value;
            }
        }

        private RouteNode destination;
        public RouteNode Destination
        {

            get { return destination; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("Destination", "Destination cannot be set to null.");

                if (value.Equals(Origin))
                    throw new InvalidObjectStateException("Destination", "Destination cannot be the same as the Origin.");

                this.destination = value;
            }
        }

        // other fields
        //-------------

        // all the departure times this route offers
        private List<WeeklyTime> departureTimes;
        public List<WeeklyTime> DepartureTimes
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
        private int duration;
        public int Duration
        {
            get { return duration; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("Duration", "Duration cannot be less than or equal to 0.");

                this.duration = value;
            }
        }

        // in grams
        private int maxWeight;
        public int MaxWeight
        {
            get { return maxWeight; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("MaxWeight", "MaxWeight cannot be less than or equal to 0.");

                this.maxWeight = value;
            }
        }

        // In cubic cm
        private int maxVolume;
        public int MaxVolume
        {
            get { return maxVolume; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("MaxVolume", "MaxVolume cannot be less than or equal to 0.");

                this.maxVolume = value;
            }
        }


        // in cents 
        private int costPerGram;
        public int CostPerGram
        {
            get { return costPerGram; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("CostPerGram", "CostPerGram cannot be less than or equal to 0.");

                this.costPerGram = value;
            }
        }

        // in cents  
        private int costPerCm3;
        public int CostPerCm3
        {
            get { return costPerCm3; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("CostPerCm3", "CostPerCm3 cannot be less than or equal to 0.");

                this.costPerCm3 = value;
            }
        }

        // in cents - this isn't a property of Route, just for reference
        private int pricePerGram;
        public int PricePerGram
        {
            get { return pricePerGram; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("PricePerGram", "PricePerGram cannot be less than or equal to 0.");

                this.pricePerGram = value;
            }
        }

        // in cents - this isn't a property of Route, just for reference
        private int pricePerCm3;
        public int PricePerCm3
        {

            get { return pricePerCm3; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("PricePerCm3", "PricePerCm3 cannot be less than or equal to 0.");

                this.pricePerCm3 = value;
            }
        }
        
        public void AddDepartureTime(WeeklyTime weeklyTime)
        {
            // validation
            if (this.departureTimes.Contains(weeklyTime))
                throw new IllegalActionException("That time is already part of the route.");

            this.departureTimes.Add(weeklyTime);
            this.departureTimes.Sort();
        }

        /// <summary>
        /// Removes the given weeklyTime if it is contained.
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

        public override string ToString ()
        {
            var weeklyTimes = "";

            foreach (WeeklyTime t in departureTimes) 
            {
                weeklyTimes += t.ToString() + ", ";
            }
            
            return String.Format("Route[ID={0}, Origin={1}, Destination={2}, Company={3}, TransportType={4}, Duration={5}, MaxWeight={6}, MaxVolume={7}, CostPerCm3={8}, CostPerGram={9}, DepartureTimes={10}, LastEdited={11}]", ID, Origin.ToShortString(), Destination.ToShortString(), Company.Name, TransportType, Duration, MaxWeight, MaxVolume, CostPerCm3, CostPerGram, weeklyTimes, LastEdited);
        }

        public override string ToNetString()
        {
            return NetCodes.BuildObjectNetString(base.ToNetString(), Convert.ToString(Origin.ID), Convert.ToString(Destination.ID), Convert.ToString(Company.ID), TransportType.ToNetString(), Convert.ToString(PricePerGram), Convert.ToString(PricePerCm3), Convert.ToString(MaxWeight), Convert.ToString(MaxVolume), Convert.ToString(Duration), WeeklyTime.BuildTimesNetString(DepartureTimes));
        }

        public static Route ParseNetString(string objectDef, State state)
        {
            string[] tokens = objectDef.Split(NetCodes.SEPARATOR_FIELD);
            int count = 0;
            int id = Convert.ToInt32(tokens[count++]);
            int originId = Convert.ToInt32(tokens[count++]);
            int destinationId = Convert.ToInt32(tokens[count++]);
            int companyId = Convert.ToInt32(tokens[count++]);
            TransportType type = TransportTypeExtensions.ParseNetString(tokens[count++]);
            int weightCost = Convert.ToInt32(tokens[count++]);
            int volumeCost = Convert.ToInt32(tokens[count++]);
            int weightMax = Convert.ToInt32(tokens[count++]);
            int volumeMax = Convert.ToInt32(tokens[count++]);
            int duration = Convert.ToInt32(tokens[count++]);
            List<WeeklyTime> routeTimes = WeeklyTime.ParseTimesNetString(tokens[count++]);
            return new Route() { Origin = state.GetRouteNode(originId), Destination = state.GetRouteNode(destinationId), Company = state.GetCompany(companyId), TransportType = type, CostPerGram = weightCost, CostPerCm3 = volumeCost, MaxWeight = weightMax, MaxVolume = volumeMax, Duration = duration, DepartureTimes = routeTimes };
        }

    }
}
