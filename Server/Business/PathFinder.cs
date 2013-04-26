//////////////////////
// Original Writer: Joshua Scott
// Reviewed by: 
//
// 
//////////////////////

using System.Collections.Generic;
using Common;
using Server.Data;

namespace Server.Business
{
    public class PathFinder
    {
        private RouteService routeService;

        private Dictionary<RouteNode, double> cost;
        private Dictionary<RouteNode, RouteNode> parent;
        private HashSet<RouteNode> closed;
        private List<RouteNode> fringe;


        public PathFinder(RouteService routeService)
        {
            this.routeService = routeService;
        }

        public IEnumerable<IEnumerable<RouteInstance>> findRoutes(Delivery delivery)
        {
            return null;
        }

        private IEnumerable<Route> findPath(Delivery delivery, NodeEvaluator evaluator)//, Excluder excluder) 
        {
            RouteNode origin = delivery.Origin;
            RouteNode goal = delivery.Destination;

            cost = new Dictionary<RouteNode, double>();
            parent = new Dictionary<RouteNode, RouteNode>();
            closed = new HashSet<RouteNode>();
            
            //TODO need to make a queue
            fringe = new List<RouteNode>();


            //if the queue is empty return null (no path)
            while (fringe.Capacity > 0)
            {
                //take new node of the top of the stack
                RouteNode curNode = fringe[0];
                fringe.RemoveAt(0);
                closed.Add(curNode);

                //if it's the goal node exit and send path
                if (curNode.Equals(goal))
                    return null; //TODO


                //grab a list of the next avaliable nodes
                IEnumerable<Route> routes = routeService.GetAll(curNode);

                //take each route/node and evaluate
                foreach (Route path in routes)
                {

                    //route has both nodes
                    //route provides routeinstance (certain time)
                    //route service provides all routes associated witha  a certain node

                    RouteInstance nextInstance = evaluator.GetNextInstance(path);


                    // Take next node from the intace route
                    RouteNode nextNode = path.RouteNode;

                    if(closed.Contains(nextNode))
                        continue;

                    //TODO
                    double currentCost = cost[curNode];//cost of the current node
                    double transitionCost = 0;
                    double pathCost = 0; //path cost
                    double totalCost = currentCost + transitionCost + pathCost;

                    //if node not exists
                    if (cost.ContainsKey(nextNode))
                    {
                        if (cost[nextNode] > totalCost) //TODO
                            continue;
                    }
                    //else
                    else
                    {
                        cost.Add(nextNode, totalCost);
                        parent.Add(nextNode, curNode);
                        fringe.Add(nextNode);
                        //fringe.Sort((System.Collections.IComparer)evaluator);
                    }
                }


            }
            return null;
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
                    if (outer.cost[r1] < outer.cost[r2]) retval = 1;
                    if (outer.cost[r2] < outer.cost[r1]) retval = -1;
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



    }
}
