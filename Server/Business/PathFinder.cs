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
        public Dictionary<int, IEnumerable<RouteInstance>> findRoutes(Delivery delivery)
        {

            Dictionary<int, IEnumerable<RouteInstance>> paths = new Dictionary<int, IEnumerable<RouteInstance>>();

            paths.Add((int)PathType.Express, findPath(delivery, time, allRoutes));
            paths.Add((int)PathType.Standard, findPath(delivery, cost, allRoutes));
            paths.Add((int)PathType.AirExpress, findPath(delivery, time, airOnly));
            paths.Add((int)PathType.AirStandard, findPath(delivery, cost, airOnly));

            return paths;
        }

        private IEnumerable<RouteInstance> findPath(Delivery delivery, NodeEvaluator evaluator, RouteExcluder excluder)//, Excluder excluder) 
        {
            RouteNode origin = delivery.Origin;
            RouteNode goal = delivery.Destination;

            originPath = new Dictionary<RouteNode, RouteInstance>();
            closed = new HashSet<RouteNode>();
            fringe = new SortedList<RouteNode, double>();


            //if the queue is empty return null (no path)
            while (fringe.Capacity > 0)
            {
                //take new node off the top of the stack
                RouteNode curNode = fringe.Keys[0];
                fringe.RemoveAt(0);
                closed.Add(curNode);

                //if it's the goal node exit and send path
                if (curNode.Equals(goal))
                    return reconstructPath(curNode);

                //grab a list of the next avaliable nodes
                IEnumerable<Route> routes = routeService.GetAll(curNode);

                //take each route/node and evaluate
                foreach (Route path in excluder.Omit(routes))
                {
                    RouteInstance nextInstance = evaluator.GetNextInstance(path);
                    RouteNode nextNode = path.Destination;

                    if(closed.Contains(nextNode))
                        continue;

                    double totalCost = evaluator.GetValue(curNode, nextInstance, curNode);

                    //if node not exists
                    if (fringe.ContainsKey(nextNode))
                    {
                        if (fringe[nextNode] > totalCost) //TODO
                            continue;
                    }
                    //else
                    else
                    {
                        originPath.Add(nextNode, nextInstance);
                        fringe.Add(nextNode, totalCost);
                        //fringe.Sort((System.Collections.IComparer)evaluator);
                    }
                }
            }
            return null;
        }

        private IEnumerable<RouteInstance> reconstructPath(RouteNode goal)
        {
            List<RouteInstance> path = new List<RouteInstance>();
            
            RouteNode nextNode = goal;
            RouteInstance nextInstance;

            do
            {
                nextInstance = originPath[nextNode];
                path.Add(nextInstance);
                nextNode = nextInstance.Route.Origin;
            }
            while (originPath.ContainsKey(nextNode));
            return path;
        }

        private abstract class NodeEvaluator : System.Collections.IComparer
        {
            readonly PathFinder outer;

            public NodeEvaluator(PathFinder outer)
            {
                this.outer = outer;
            }

            public int Compare(object ob1, object ob2)
            {
                int retval = 0;

                if (ob1 is RouteNode && ob2 is RouteNode)
                {
                    RouteNode r1 = (RouteNode)ob1;
                    RouteNode r2 = (RouteNode)ob2;
                    if (outer.fringe[r1] < outer.fringe[r2]) retval = 1;
                    if (outer.fringe[r2] < outer.fringe[r1]) retval = -1;
                }
                return (retval);
            }

            public abstract double GetValue(RouteNode start, RouteInstance route, RouteNode end);

            public abstract RouteInstance GetNextInstance(Route routes);
        }

        //compare based on cost (money)
        private class CostEvaluator : NodeEvaluator
        {
            public CostEvaluator(PathFinder outer) : base(outer)
            {
            }

            public override double GetValue(RouteNode start, RouteInstance route, RouteNode end)
            {
                throw new System.NotImplementedException();
            }

            public override RouteInstance GetNextInstance(Route routes)
            {
                throw new System.NotImplementedException();
            }
        }

        //compare based on time
        private class TimeEvaluator : NodeEvaluator
        {
            public TimeEvaluator(PathFinder outer) : base(outer)
            {
            }

            public override double GetValue(RouteNode start, RouteInstance route, RouteNode end)
            {
                throw new System.NotImplementedException();
            }

            public override RouteInstance GetNextInstance(Route routes)
            {
                throw new System.NotImplementedException();
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
            public AirOnlyExcluder(PathFinder outer) : base(outer)
            {
            }

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
            public NullExcluder(PathFinder outer) : base(outer) 
            {
            }

            public override IEnumerable<Route> Omit(IEnumerable<Route> original)
            {
                return new List<Route>(original);
            }
        }
    }
}
