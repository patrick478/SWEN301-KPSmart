using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Client
{
    /// <summary>
    /// Handles the client-specific view of the system state.
    /// TODO The CurrentState extension is a bit dirty seeing as it is basically chopping off a bunch of CurrentState-specific stuff, only taking some of it. May want to change?
    /// </summary>
    public class ClientState: CurrentState
    {
        public override void SaveRouteNode(RouteNode routeNode) { throw new NotSupportedException(); }

        public override void RemoveRouteNode(int id) { throw new NotSupportedException(); }

        public override void InitialiseRouteNodes(IDictionary<int, RouteNode> routeNodes) { throw new NotSupportedException(); }

        public override bool RouteNodesInitialised { get { return false; } }

        public override void SaveDelivery(Delivery delivery) { throw new NotSupportedException(); }

        public override void RemoveDelivery(int id) { throw new NotSupportedException(); }

        public override void InitialiseDeliveries(IDictionary<int, Delivery> deliveries) { throw new NotSupportedException(); }

        public override bool DeliveriesInitialised { get { return false; } }
    }
}
