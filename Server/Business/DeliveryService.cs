using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Data;

namespace Server.Business
{
    public class DeliveryService: Service<Delivery>
    {
        public DeliveryService(CurrentState state) : base(state, new DeliveryDataHelper())
        {
            // initialise current deliveries
            //var deliveries = dataHelper.LoadAll();
            var deliveries = new Dictionary<int, Delivery>();
            state.InitialiseDeliveries(deliveries);
        }

        public override Delivery Get(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Delivery> GetAll()
        {
            throw new NotImplementedException();
        }

        public override bool Exists(Delivery obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
