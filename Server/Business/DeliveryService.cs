using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Data;

namespace Server.Business
{
    public class DeliveryService: Service<Delivery>
    {
        private PathFinder pathFinder;
        
        public DeliveryService(CurrentState state, PathFinder pathfinder) : base(state, new DeliveryDataHelper())
        {
            // initialise current deliveries from DB
            if (!state.DeliveriesInitialised)
            {             
                //var deliveries = dataHelper.LoadAll();
                var deliveries = new Dictionary<int, Delivery>();
                state.InitialiseDeliveries(deliveries);
            }

            // save reference to the pathfinder
            this.pathFinder = pathfinder;
        }


        public Dictionary<int, Delivery> GetBestRoutes()
        {
            return null;
        }

        public Delivery Create(int originId, int destinationId, Priority priority, IEnumerable<RouteInstance> routes,
                               int weightInGrams, int volumeInCm3)
        {
            //todo validate the list of deliveries is contiguous.
            var origin = state.GetRouteNode(originId);
            var destination = state.GetRouteNode(destinationId);


            var totalCost = GetTotalCost(routes, weightInGrams, volumeInCm3);
            var totalPrice = GetTotalPrice(routes, priority, weightInGrams, volumeInCm3);
            var scope = GetScope(routes);
            var timeOfRequest = DateTime.Now; // should this be UTC?
            var duration = GetDuration(routes);
            var timeOfDelivery = routes.AsQueryable().Last().DepartureTime.Add(duration);
            //todo really inefficient - need to put in one loop!!!

            var delivery = new Delivery
                {
                    Origin = origin,
                    Destination = destination,
                    Priority = priority,
                    TimeOfDelivery = timeOfDelivery,
                    TimeOfRequest = timeOfRequest,
                    TotalCost = totalCost,
                    TotalPrice = totalPrice,
                    Scope = scope
                };
                
            
            // save in DB
            dataHelper.Create(delivery);

            // save state
            state.SaveDelivery(delivery);
            state.IncrementNumberOfEvents();

            return delivery;
        }


        private TimeSpan GetDuration(IEnumerable<RouteInstance> routes)
        {
            
            TimeSpan total = new TimeSpan(0);
            
            foreach (RouteInstance r in routes)
            {
                var route = r.Route;
                total = total.Add(new TimeSpan(0, 0, route.Duration, 0));
            }

            return total;
        }

        private Scope GetScope(IEnumerable<RouteInstance> routes)
        {
            foreach (RouteInstance r in routes)
            {
                if (r.Route.Origin.GetType() == typeof (InternationalPort))
                {
                    return Scope.International;
                }
            }

            return Scope.Domestic;
        }

        private int GetTotalCost(IEnumerable<RouteInstance> delivery, int weightInGrams, int volumeInCm3 )
        {
            int totalCost = 0;
            
            foreach (RouteInstance r in delivery)
            {
                int costPerGram = r.Route.CostPerGram;
                int costPerCm3 = r.Route.CostPerCm3;

                totalCost += costPerGram*weightInGrams;
                totalCost += costPerCm3*volumeInCm3;
            }

            return totalCost;
        }


        private int GetTotalPrice(IEnumerable<RouteInstance> delivery, Priority priority, int weightInGrams,
                                  int volumeInCm3)
        {
            int totalPrice = 0;

            var prices = state.GetAllPrices().AsQueryable();

            foreach (RouteInstance r in delivery)
            {
                var route = r.Route;
                var price =
                    prices.First(t =>
                        t.Origin.Equals(route.Origin) && t.Destination.Equals(route.Destination) &&
                        t.Priority.Equals(priority));

                totalPrice += price.PricePerCm3*volumeInCm3;
                totalPrice += price.PricePerGram*weightInGrams;
            }

            return totalPrice;
        }

        public override Delivery Get(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            return state.GetDelivery(id);
        }

        public override IEnumerable<Delivery> GetAll()
        {
            return state.GetAllDeliveries();
        }

        public override bool Exists(Delivery obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
