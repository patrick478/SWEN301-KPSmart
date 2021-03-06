﻿using System;
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
        /// <summary>
        /// Server timestamp on the latest state update received.
        /// </summary>
        private DateTime lastUpdate;

        public ClientState()
        {
            this.countries = new Dictionary<int, Country>();
            this.routes = new Dictionary<int, Route>();
            this.prices = new Dictionary<int, Price>();
            this.companies = new Dictionary<int, Company>();
            this.routeNodes = new Dictionary<int, RouteNode>();
            this.domesticPrices = new Dictionary<int, DomesticPrice>();
        }

        public void SetUpdateTime(DateTime newTime)
        {
            lastUpdate = newTime;
        }

        public DateTime GetUpdateTime()
        {
            return lastUpdate;
        }

        #region Functionality Removal Overrides
        // State method overrides

        public override Delivery GetDelivery(int id) { throw new NotSupportedException("Client state does not store Deliveries."); }

        public override IList<Delivery> GetAllDeliveries() { throw new NotSupportedException("Client state does not store Deliveries."); }

        // Current State method overrides

        public override void SaveDelivery(Delivery delivery) { throw new NotSupportedException("Client state does not store Deliveries."); }

        public override void RemoveDelivery(int id) { throw new NotSupportedException("Client state does not store Deliveries."); }

        public override void InitialiseDeliveries(IDictionary<int, Delivery> deliveries) { throw new NotSupportedException("Client state does not store Deliveries."); }

        public override bool DeliveriesInitialised { get { return false; } }
        #endregion

        public DateTime FirstEvent { get; set; }
    }
}
