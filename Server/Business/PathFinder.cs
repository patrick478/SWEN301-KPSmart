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
        public static IEnumerable<Route> findPath(Delivery delivery) 
        {

            //Minimize Time
            //Minimize Price


            RouteNode origin = delivery.Origin;
            RouteNode goal = delivery.Destination;

            Dictionary<RouteNode, double> cost = new Dictionary<RouteNode, double>();
            Dictionary<RouteNode, RouteNode> parent = new Dictionary<RouteNode, RouteNode>();

            //HashSet<RouteNode> closed = new HashSet<RouteNode>();


                 //function Dijkstra(Graph, source):
                 //     for each vertex v in Graph:                                // Initializations
                 //         dist[v] := infinity ;                                  // Unknown distance function from 
                 //                                                                // source to v
                 //         previous[v] := undefined ;                             // Previous node in optimal path
                 //     end for                                                    // from source
      
                 //     dist[source] := 0 ;                                        // Distance from source to source
                 //     Q := the set of all nodes in Graph ;                       // All nodes in the graph are
                 //                                                                // unoptimized - thus are in Q
                 //     while Q is not empty:                                      // The main loop
                 //         u := vertex in Q with smallest distance in dist[] ;    // Source node in first case
                 //         remove u from Q ;
                 //         if dist[u] = infinity:
                 //             break ;                                            // all remaining vertices are
                 //         end if                                                 // inaccessible from source
          
                 //         for each neighbor v of u:                              // where v has not yet been 
                 //                                                                // removed from Q.
                 //             alt := dist[u] + dist_between(u, v) ;
                 //             if alt < dist[v]:                                  // Relax (u,v,a)
                 //                 dist[v] := alt ;
                 //                 previous[v] := u ;
                 //                 decrease-key v in Q;                           // Reorder v in the Queue
                 //             end if
                 //         end for
                 //     end while
                 // return dist;

            return null;
        }
    }
}
