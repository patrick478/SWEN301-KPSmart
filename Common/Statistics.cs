using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    class Statistics
    {

        private State state;

        public Statistics(State state) {
            this.state = state;
        }

        public int TotalRevenue
        {
            get;
            private set;
        }

        public int TotalExpenditure
        {
            get;
            private set;
        }

        public int TotalEvents
        {
            get;
            private set;
        }

        public List<Triple> Triples
        {
            get;
            private set;
        }


        public void GetStatistics(){

            IList<Delivery> deliveries = state.GetAllDeliveries();
            Triple.Clear();

            //Total Events - The number of events used to generate these figures.
            TotalEvents = deliveries.Count;

            foreach (Delivery delivery in deliveries)
            {

                //Total Revenue - Total revenue from Customer charges.
                //Total Expenditure - Total cost of transport charges.
                int revenue = delivery.TotalPrice - delivery.TotalCost;
                TotalRevenue += delivery.TotalPrice - delivery.TotalCost;
                TotalExpenditure += delivery.TotalCost;

                Triple triple = Triple.GetTriple(delivery.Origin, delivery.Destination, delivery.Priority);

                //Amount of Mail - Total number of mail, total weight, total volume. For each path.
                triple.TotalMail++;
                triple.TotalVolume += delivery.VolumeInCm3;
                triple.TotalWeight += delivery.WeightInGrams;

                //Average Delivery Times - Average delivery time for each Triple*.
                triple.TotalDeliveryTimes.Add(delivery.Duration);

                //Critical Routes - List of Triples* where the average transport cost is more than the average customer price.
                triple.TotalRevenue += revenue;
            }
        }


        public class Triple
        {

            public static List<Triple> instances = new List<Triple>();

            private Triple(RouteNode origin, RouteNode destination, Priority priority) 
            {
                Origin = origin;
                Destination = destination;
                Priority = priority;
            }

            public static Triple GetTriple(RouteNode origin, RouteNode destination, Priority priority)
            {
                try{
                    return instances.First(triple => 
                                triple.Origin.Equals(origin) && 
                                triple.Destination.Equals(destination) &&
                                triple.Priority.Equals(priority));
                }catch{}

                Triple newTriple = new Triple(origin, destination, priority);
                instances.Add(newTriple);
                return newTriple;
            }

            public static void Clear()
            {
                instances = new List<Triple>();
            }

            public int TotalMail { get; set; }
            public int TotalWeight { get; set; }
            public int TotalVolume { get; set; }

            public int AverageDeliveryTimes
            {
                get
                {
                    double time = TotalDeliveryTimes.TotalMinutes;
                    double num = TotalMail;
                    return (int)(time / ((num > 0) ? num: 0));
                }
            }
            public bool CriticalRoutes
            {
                get
                {
                    return TotalRevenue > 0;
                }
            }

            public TimeSpan TotalDeliveryTimes { get; set; }
            public int TotalRevenue { get; set; }

            public List<Triple> AllInstances
            {
                get
                {
                    return new List<Triple>(instances);
                }
            }

            public RouteNode Origin
            {
                get;
                private set;
            }

            public RouteNode Destination
            {
                get;
                private set;
            }

            public Priority Priority
            {
                get;
                private set;
            }
        }
    }
}
