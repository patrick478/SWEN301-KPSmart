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

        public void GetStatistics(){

            IList<Delivery> deliveries = state.GetAllDeliveries();

            //Total Revenue - Total revenue from Customer charges.
            //Total Expenditure - Total cost of transport charges.
            //Total Events - The number of events used to generate these figures.
            long totalRevenue = 0;
            long totalExpenditure = 0;
            long totalEvents = deliveries.Count;

            foreach (Delivery delivery in deliveries)
            {
                totalRevenue += delivery.TotalPrice;
                totalExpenditure += delivery.TotalCost;


                //create eideitefier
                //delivery.Origin, delivery.Duration, delivery.Priority

                Triple triple = Triple.GetTriple(delivery.Origin, delivery.Destination, delivery.Priority);


            }

            //Amount of Mail - Total number of mail, total weight, total volume. For each path.

            //Average Delivery Times - Average delivery time for each Triple*.

            //Critical Routes - List of Triples* where the average transport cost is more than the average customer price.




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

            public List<Triple> AllInstances
            {
                get
                {
                    return instances;
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
