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
        public PriceService(CurrentState state) : base(state, new PriceDataHelper())
        {
            // initialise current prices
            //var prices = dataHelper.LoadAll();
            var prices = new Dictionary<int, Price>();
            state.InitialisePrices(prices);
        }

        public override Price Get(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Price> GetAll()
        {
            throw new NotImplementedException();
        }

        public override bool Exists(Price obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
