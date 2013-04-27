﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Data;

namespace Server.Business
{
    public class PriceService: Service<Price>
    {
        public PriceService(CurrentState state) : base(state, new PriceDataHelper())
        {
            // initialise current prices from DB
            if (!state.PricesInitialised)
            {
                //var prices = dataHelper.LoadAll();
                var prices = new Dictionary<int, Price>();
                state.InitialisePrices(prices);
            }
        }

        /// <summary>
        /// Creates a new price for the given [origin, destination, priority] combination.
        /// </summary>
        /// <param name="originID">id of the origin</param>
        /// <param name="destinationID">id of the destination</param>
        /// <param name="priority">priority</param>
        /// <param name="pricePerGram"></param>
        /// <param name="pricePerCm3"></param>
        /// <returns>the created object, with ID field, and LastEdited initialised</returns>
        /// <exception cref="DatabaseException">if it already exists</exception>
        /// <exception cref="InvalidObjectStateException">if the fields are invalid</exception>
        public Price Create(int originID, int destinationID, Priority priority, int pricePerGram, int pricePerCm3)
        {
            var origin = state.GetRouteNode(originID);
            var destination = state.GetRouteNode(destinationID);

            // throws an exception if invalid
            var newPrice = new Price { Origin = origin, Destination = destination, Priority = priority, PricePerGram = pricePerGram, PricePerCm3 = pricePerCm3};

            // throws a database exception if exists already
            dataHelper.Create(newPrice);

            // update state
            state.SavePrice(newPrice);
            state.IncrementNumberOfEvents();

            return newPrice;
        }

        /// <summary>
        /// Updates a new price for the given [origin, destination, priority] combination.
        /// </summary>
        /// <param name="originID">id of the origin</param>
        /// <param name="destinationID">id of the destination</param>
        /// <param name="priority">priority</param>
        /// <param name="pricePerGram"></param>
        /// <param name="pricePerCm3"></param>
        /// <returns>the created object, with ID field, and LastEdited initialised</returns>
        /// <exception cref="DatabaseException">if it doesn't exist</exception>
        /// <exception cref="InvalidObjectStateException">if the fields are invalid</exception>
        public Price Update(int originID, int destinationID, Priority priority, int pricePerGram, int pricePerCm3)
        {
            var origin = state.GetRouteNode(originID);
            var destination = state.GetRouteNode(destinationID);

            // throws an exception if invalid
            var newPrice = new Price { Origin = origin, Destination = destination, Priority = priority, PricePerGram = pricePerGram, PricePerCm3 = pricePerCm3 };

            // throws a database exception if exists already
            dataHelper.Update(newPrice);

            // update state
            state.SavePrice(newPrice);
            state.IncrementNumberOfEvents();

            return newPrice;
        }


        public override Price Get(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            return state.GetPrice(id);
        }

        public override IEnumerable<Price> GetAll()
        {
            return state.GetAllPrices();
        }

        public override bool Exists(Price price)
        {
            var prices = state.GetAllPrices().AsQueryable();
            return prices.Any(t => t.Equals(price));
        }

        public override void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            var price = state.GetPrice(id);

            // checks there aren't any routes with the same [origin, destination] combination.
            var routes = state.GetAllRoutes();
            bool isUsed = routes.AsQueryable().Any(t => t.Origin.Equals(price.Origin) && t.Destination.Equals(price.Destination));
            if (isUsed)
            {
                throw new IllegalActionException("Cannot remove a price that corresponds to existing routes.  You can either update the price, or remove the routes first.");
            }

            // remove from db
            dataHelper.Delete(id);

            // remove from state     
            state.RemovePrice(id);
            state.IncrementNumberOfEvents();
        }
    }
}
