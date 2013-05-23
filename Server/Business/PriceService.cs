using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Data;

namespace Server.Business
{
    public class PriceService: Service<Price>
    {

        private PriceDataHelper dataHelper = new PriceDataHelper();

        public PriceService(CurrentState state) : base(state, new PriceDataHelper())
        {
            // initialise current prices from DB
            if (!state.PricesInitialised)
            {
                var prices = dataHelper.LoadAll();
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

        public DomesticPrice GetDomesticPrice (int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            return state.GetDomesticPrice(id);
        }

        public DomesticPrice CreateDomesticPrice (Priority priority, int pricePerGram, int pricePerCm3) 
        {
            // throws an exception if invalid
            var newPrice = new DomesticPrice(priority) { PricePerCm3 = pricePerCm3, PricePerGram = pricePerGram };

            // throws a database exception if exists already
            dataHelper.Create(newPrice);

            // update state
            state.SaveDomesticPrice(newPrice);
            state.IncrementNumberOfEvents();

            return newPrice; 
        }

        public DomesticPrice UpdateDomesticPrice (int priceId, int pricePerGram, int pricePerCm3) 
        {
            var domesticPrice = state.GetDomesticPrice(priceId);
            if (domesticPrice == null)
                throw new ArgumentException("No price was found with id: " + priceId, "priceId");

            // throws an exception if invalid
            var newPrice = new DomesticPrice(domesticPrice.Priority) { ID = priceId, PricePerCm3 = pricePerCm3, PricePerGram = pricePerGram };

            // throws a database exception if doens't exist already
            dataHelper.Update(newPrice);

            // update state
            state.SaveDomesticPrice(newPrice);
            state.IncrementNumberOfEvents();

            return newPrice; 
        }

        public IList<DomesticPrice> GetAllDomesticPrices () 
        {
            return state.GetAllDomesticPrices();
        }

        /// <summary>
        /// Updates the given price.
        /// </summary>
        /// <param name="priceId">id of the price to update</param>
        /// <param name="pricePerGram"></param>
        /// <param name="pricePerCm3"></param>
        /// <returns>the created object, with ID field, and LastEdited initialised</returns>
        /// <exception cref="DatabaseException">if it doesn't exist</exception>
        /// <exception cref="InvalidObjectStateException">if the fields are invalid</exception>
        public Price Update(int priceId, int pricePerGram, int pricePerCm3)
        {
            var price = state.GetPrice(priceId);
            if (price == null)
                throw new ArgumentException("No price was found with id: " + priceId, "priceId");
            
            // throws an exception if invalid
            var newPrice = new Price { Origin = price.Origin, Destination = price.Destination, Priority = price.Priority, PricePerGram = pricePerGram, PricePerCm3 = pricePerCm3 };
            newPrice.ID = priceId;

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

        public override IList<Price> GetAll()
        {
            return state.GetAllPrices();
        }

        public override bool Exists(Price price)
        {
            var prices = state.GetAllPrices().AsQueryable();
            var domesticPrices = state.GetAllDomesticPrices().AsQueryable();
            return prices.Any(t => t.Equals(price)) || domesticPrices.Any(t => t.Equals(price));
        }

        public override void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            var price = state.GetPrice(id);

            if (price == null)
                throw new ArgumentException("No price with that id was found: " + id);

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
