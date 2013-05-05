using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Timers;
using Common;
using Server.Data;
using Server.Gui;

namespace Server.Business
{
    public class DeliveryService: Service<Delivery>
    {
        private PathFinder pathFinder;
        private static Dictionary<int, DeliveryOption> temporaryOptions;
        
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
            temporaryOptions = new Dictionary<int, DeliveryOption>();

            // set up to clear cache every hour
            var timerItem = new Timer(3600000);
            timerItem.Elapsed += ClearTemporaryOptionsCacheOnTimedEvent;
            timerItem.AutoReset = true;
            timerItem.Start();
        }


        public IDictionary<PathType, Delivery> GetBestRoutes(int clientId, int originId, int destinationId, int weightInGrams, int volumeInCm3)
        {
            // prep result
            var result = new Dictionary<PathType, Delivery>();   
            
            // get origin and destination
            var origin = state.GetRouteNode(originId);
            var destination = state.GetRouteNode(destinationId);
            var timeOfRequest = DateTime.UtcNow;
            var prices = state.GetAllPrices();

            // check arguments
            if(origin == null)
                throw new ArgumentException("Could not find that origin", "originId");

            if (destination == null)
                throw new ArgumentException("Could not find that destination", "destinationId");

            if (weightInGrams <= 0)
                throw new ArgumentException("Weight cannot be less than or equal to zero", "weightInGrams");

            if (volumeInCm3 <= 0)
                throw new ArgumentException("Volume cannot be less than or equal to zero", "volumeInCm3");

            // do pathfinding and get results
            var deliveries =
                pathFinder.findRoutes(new Delivery
                    {
                        Origin = origin,
                        Destination = destination,
                        WeightInGrams = weightInGrams,
                        VolumeInCm3 = volumeInCm3
                    });

            // return empty result if none found
            if (deliveries.Count == 0)
            {
                return result;
            }

            // make deliveries of the paths found
            var deliveryOptions = new DeliveryOption(timeOfRequest);
            foreach (var delivery in deliveries)
            {
                var pathType = delivery.Key;
                var path = delivery.Value;

                // make the delivery (will throw an exception if fields are incorrect)
                var newDelivery = GetDeliveryFromRouteInstances(path, weightInGrams, volumeInCm3,
                                                                timeOfRequest, pathType);
                
                // add delivery to deliveryOptions
                deliveryOptions.AddOption(pathType, newDelivery);
            }

            // save options
            temporaryOptions[clientId] = deliveryOptions;

            // return result
            return deliveryOptions.GetOptions();
        }

        private Delivery GetDeliveryFromRouteInstances(IList<RouteInstance> path, int weightInGrams, int volumeInCm3, DateTime timeOfRequest, PathType pathType)
        {      
            // check arguments
            if (path.Count == 0)
                throw new ArgumentException("Cannot have an empty path");
            if (weightInGrams <= 0)
                throw new ArgumentException("Weight cannot be less than or equal to zero", "weightInGrams");
            if (volumeInCm3 <= 0)
                throw new ArgumentException("Volume cannot be less than or equal to zero", "volumeInCm3");
            
            // initialise interesting variables
            RouteNode origin = path.First().Route.Origin;
            RouteNode destination = path.Last().Route.Destination;

            Priority priority = pathType.GetPriority();
            int totalCost = 0;
            int totalPrice = 0;
            Scope scope = Scope.Domestic;
            DateTime timeOfDelivery;

            // iterate through all routeInstances and calculate all the variables
            RouteNode previousDestination = null;
            foreach (RouteInstance routeInstance in path)
            {
                Route route = routeInstance.Route;

                // check contiguous
                if (previousDestination != null && route.Origin.Equals(previousDestination))
                    throw new ArgumentException("Path isn't contiguous: " + path);

                // add to cost
                totalCost += route.CostPerGram * weightInGrams;
                totalCost += route.CostPerCm3 * volumeInCm3;

                // add to price
                var prices = state.GetAllPrices();
                var price =
                prices.First(t =>
                    t.Origin.Equals(route.Origin) && t.Destination.Equals(route.Destination) &&
                    t.Priority.Equals(priority));
                totalPrice += price.PricePerCm3 * volumeInCm3;
                totalPrice += price.PricePerGram * weightInGrams;

                // check scope
                if (scope != Scope.International && route.Scope == Scope.International)
                    scope = Scope.International;

                previousDestination = route.Destination;
            }

            // calculate timeOfDelivery
            var lastRouteInstance = path.Last();
            timeOfDelivery = lastRouteInstance.DepartureTime.AddMinutes(lastRouteInstance.Route.Duration);

            // calculate duration
            var duration = timeOfDelivery.Subtract(timeOfRequest);

            // make the delivery (will throw an exception if fields are incorrect)
            var newDelivery = new Delivery
                {
                    Origin = origin, 
                    Destination = destination, 
                    Routes = path, 
                    VolumeInCm3 = volumeInCm3, 
                    WeightInGrams = weightInGrams, 
                    TotalCost = totalCost, 
                    TotalPrice = totalPrice, 
                    TimeOfRequest = timeOfRequest, 
                    TimeOfDelivery = timeOfDelivery, 
                    Priority = priority, 
                    Scope = scope
                };

            return newDelivery;
        }

        public Delivery SelectDeliveryOption(int clientId, PathType selection)
        {
            // get delivery
            var delivery = temporaryOptions[clientId].GetOption(selection);
            if (delivery == null)
                throw new IllegalActionException("Cannot find your selection. You haven't requested us to find the best paths yet.");
  
            // save in DB
            dataHelper.Create(delivery);

            // save state
            state.SaveDelivery(delivery);
            state.IncrementNumberOfEvents();

            return delivery;
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

        /// <summary>
        /// Not implemented for this Service. There are no fields that are unique to a particular Delivery except ID.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool Exists(Delivery obj)
        {
            throw new NotImplementedException("There are no fields that are unique to a particular Delivery except ID.");
        }

        /// <summary>
        /// Not implemented for this Service. Cannot delete a delivery.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override void Delete(int id)
        {
            throw new NotImplementedException("Cannot delete a delivery.");
        }


        private static void ClearTemporaryOptionsCacheOnTimedEvent(object source, ElapsedEventArgs e)
        {
            Logger.WriteLine("Removing deliveryOptions older than one hour.");

            var listOfKeysToRemove = new List<int>();
            
            foreach (var option in temporaryOptions)
            {
                if (option.Value.Timestamp.AddHours(1) < DateTime.UtcNow)
                {
                    listOfKeysToRemove.Add(option.Key);
                }
            }

            Logger.WriteLine(String.Format("Removed {0} options",listOfKeysToRemove.Count));

            foreach (int key in listOfKeysToRemove)
            {
                temporaryOptions.Remove(key);
            }
        }


    }
}
