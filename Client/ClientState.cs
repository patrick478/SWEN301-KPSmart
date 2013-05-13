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
        public ClientState()
        {
            this.countries = new Dictionary<int, Country>();
            this.routes = new Dictionary<int, Route>();
            this.prices = new Dictionary<int, Price>();
            this.companies = new Dictionary<int, Company>();
        }

        // State method overrides

        public override RouteNode GetRouteNode(int id) { throw new NotSupportedException("Client state does not store RouteNodes."); }

        public override IList<RouteNode> GetAllRouteNodes() { throw new NotSupportedException("Client state does not store RouteNodes."); }

        public override Delivery GetDelivery(int id) { throw new NotSupportedException("Client state does not store Deliveries."); }

        public override IList<Delivery> GetAllDeliveries() { throw new NotSupportedException("Client state does not store Deliveries."); }

        // Current State method overrides

        public override void SaveRouteNode(RouteNode routeNode) { throw new NotSupportedException("Client state does not store RouteNodes."); }

        public override void RemoveRouteNode(int id) { throw new NotSupportedException("Client state does not store RouteNodes."); }

        public override void InitialiseRouteNodes(IDictionary<int, RouteNode> routeNodes) { throw new NotSupportedException("Client state does not store RouteNodes."); }

        public override bool RouteNodesInitialised { get { return false; } }

        public override void SaveDelivery(Delivery delivery) { throw new NotSupportedException("Client state does not store Deliveries."); }

        public override void RemoveDelivery(int id) { throw new NotSupportedException("Client state does not store Deliveries."); }

        public override void InitialiseDeliveries(IDictionary<int, Delivery> deliveries) { throw new NotSupportedException("Client state does not store Deliveries."); }

        public override bool DeliveriesInitialised { get { return false; } }
    }
}
