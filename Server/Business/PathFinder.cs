//////////////////////
// Original Writer: Joshua Scott
// Reviewed by: 
//
// 
//////////////////////

using System;
using System.Collections.Generic;
using Common;
using Server.Data;
using System.Collections;

namespace Server.Business
{
    public class PathFinder
    {
        private RouteService routeService;

        private NodeEvaluator time;
        private NodeEvaluator cost;
        private RouteExcluder airOnly;
        private RouteExcluder allRoutes;

        private Dictionary<RouteNode, RouteInstance> originPath;
        private Dictionary<RouteNode, double> nodeCost;
        private HashSet<RouteNode> closed;
        private SortedList<RouteNode, double> fringe;


        public PathFinder(RouteService routeService)
        {
            this.routeService = routeService;
            time = new TimeEvaluator(this);
            cost = new CostEvaluator(this);
            airOnly = new AirOnlyExcluder(this);
            allRoutes = new NullExcluder(this);
        }

        //indexed by the ordinal of the Common.PathType
        public Dictionary<PathType, List<RouteInstance>> findRoutes(RouteNode origin, RouteNode destination, int weight, int volume)
        {
            //get the DateTime to the nearest minute as the requested Date Time
            DateTime requestTime = DateTime.Today;
            requestTime = requestTime.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);

            var paths = new Dictionary<PathType, List<RouteInstance>>
                {
                    {PathType.Express, findPath(requestTime, origin, destination, weight, volume, time, allRoutes)},
                    {PathType.Standard, findPath(requestTime, origin, destination, weight, volume, cost, allRoutes)},
                    {PathType.AirExpress, findPath(requestTime, origin, destination, weight, volume, time, airOnly)},
                    {PathType.AirStandard, findPath(requestTime, origin, destination, weight, volume, cost, airOnly)}
                };

            return paths;
        }

        private List<RouteInstance> findPath(DateTime requestTime, RouteNode origin, RouteNode goal, int weight, int volume, NodeEvaluator evaluator, RouteExcluder excluder)//, Excluder excluder) 
        {
            Delivery delivery = new Delivery();
            delivery.Origin = origin;
            delivery.Destination = goal;
            delivery.WeightInGrams = weight;
            delivery.VolumeInCm3 = volume;
            delivery.TimeOfRequest = requestTime;

            originPath = new Dictionary<RouteNode, RouteInstance>();
            nodeCost = new Dictionary<RouteNode, double>();
            closed = new HashSet<RouteNode>();
            fringe = new SortedList<RouteNode, double>();

            fringe.Add(origin, 0);
            originPath.Add(origin, new OriginRouteInstance(requestTime));

            //if the queue is empty return null (no path)
            while (fringe.Count > 0)
            {
                //take new node off the top of the stack
                //this is guaranteed to be the best way to the node
                RouteNode curNode = fringe.Keys[0];

                if (closed.Contains(curNode))
                    continue;

                nodeCost.Add(curNode, fringe.Values[0]);
                closed.Add(curNode);
                fringe.RemoveAt(0);

                //if it's the goal node exit and return path
                if (curNode.Equals(goal))
                    return completeDelivery(curNode);

                //grab a list of all of the routes where the given node is the origin
                IEnumerable<Route> routes = routeService.GetAll(curNode);

                //take each route that hasn't been ommited and evaluate
                foreach (Route path in excluder.Omit(routes))
                {
                    RouteInstance nextInstance = evaluator.GetNextInstance(path);
                    RouteNode nextNode = path.Destination;

                    double totalCost = evaluator.GetValue(nextInstance, delivery);

                    //if the node is not in the fringe
                    //or the current value is lower than
                    //the new cost then set the new parent
                    if (!fringe.ContainsKey(nextNode))
                    {
                        originPath.Add(nextNode, nextInstance);
                        fringe.Add(nextNode, totalCost);
                    }
                    else if (fringe[nextNode] > totalCost)
                    {
                        originPath.Remove(nextNode);
                        fringe.Remove(nextNode);

                        originPath.Add(nextNode, nextInstance);
                        fringe.Add(nextNode, totalCost);
                    }
                }
            }
            return null;
        }

        private class OriginRouteInstance : RouteInstance
        {
            public OriginRouteInstance (DateTime originTime) : base(null, originTime) { }

            public override DateTime ArrivalTime
            {
                get
                {
                    return DepartureTime;
                }
            }
        }

        private List<RouteInstance> completeDelivery(RouteNode goal)
        {
            List<RouteInstance> path = new List<RouteInstance>();
            RouteNode nextNode = goal;
            RouteInstance nextInstance;

            //int totalCost = 0;
            //int totalPrice = 0;

            do
            {
                nextInstance = originPath[nextNode];
                path.Add(nextInstance);
                nextNode = nextInstance.Route.Origin;
                /*
                    totalCost += nextInstance.Route.CostPerCm3 * delivery.VolumeInCm3;
                    totalCost += nextInstance.Route.CostPerGram * delivery.WeightInGrams;

                    totalPrice += nextInstance.Route.PricePerCm3 * delivery.VolumeInCm3;
                    totalPrice += nextInstance.Route.PricePerGram * delivery.WeightInGrams;
                */
            }
            while (originPath[nextNode].Route != null);
            /*
                delivery.TotalCost = totalCost;
                delivery.TotalPrice = totalPrice;
                delivery.TimeOfDelivery = originPath[delivery.Destination].ArrivalTime;
            */
            path.Reverse(0, path.Count);
            return path;
        }

        private abstract class NodeEvaluator
        {
            readonly protected PathFinder outer;

            public NodeEvaluator(PathFinder outer)
            {
                this.outer = outer;
            }
            
            public abstract double GetValue(RouteInstance route, Delivery delivery);
            
            public RouteInstance GetNextInstance(Route routes)
            {
                return routes.GetNextDeparture(outer.originPath[routes.Origin].ArrivalTime);
            }
        }

        //compare based on cost (money)
        private class CostEvaluator : NodeEvaluator
        {
            public CostEvaluator(PathFinder outer) : base(outer) { }

            public override double GetValue(RouteInstance route, Delivery delivery)
            {
                double routeCost = route.Route.CostPerCm3 * delivery.VolumeInCm3;
                routeCost += route.Route.CostPerGram * delivery.WeightInGrams;
                routeCost += outer.nodeCost[route.Route.Origin];
                return routeCost + (1.0 - (1.0 / route.ArrivalTime.Ticks));
            }
        }

        //compare based on time
        private class TimeEvaluator : NodeEvaluator
        {
            public TimeEvaluator(PathFinder outer) : base(outer) { }

            public override double GetValue(RouteInstance route, Delivery delivery)
            {
                double routeCost = route.Route.CostPerCm3 * delivery.VolumeInCm3;
                routeCost += route.Route.CostPerGram * delivery.WeightInGrams;
                routeCost += outer.nodeCost[route.Route.Origin];
                return route.ArrivalTime.Ticks + (1.0 - (1.0 / routeCost));//TODO
            }
        }


        private abstract class RouteExcluder
        {
            readonly PathFinder outer;

            public RouteExcluder(PathFinder outer)
            {
                this.outer = outer;
            }

            public abstract IEnumerable<Route> Omit(IEnumerable<Route> original);
        }

        //class to omit all non air routes in a list
        private class AirOnlyExcluder : RouteExcluder
        {
            public AirOnlyExcluder(PathFinder outer) : base(outer) { }

            public override IEnumerable<Route> Omit(IEnumerable<Route> original)
            {
                List<Route> result = new List<Route>();

                foreach (Route route in original)
                {
                    if (route.TransportType.Equals(TransportType.Air))
                        result.Add(route);
                }
                return result;
            }
        }

        //class to omit no routes
        private class NullExcluder : RouteExcluder
        {
            public NullExcluder(PathFinder outer) : base(outer) { }

            public override IEnumerable<Route> Omit(IEnumerable<Route> original)
            {
                return new List<Route>(original);
            }
        }
    }
}
