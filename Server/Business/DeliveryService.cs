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
        private PathFinder pathFinder;
        
        public DeliveryService(CurrentState state, PathFinder pathfinder) : base(state, new DeliveryDataHelper())
        {
            // initialise current deliveries from DB
            if (!state.DeliveriesInitialised)
            {             
                //var deliveries = dataHelper.LoadAll();
                var deliveries = new Dictionary<int, Delivery>();
                state.InitialiseDeliveries(deliveries);
            }

            // save reference to the pathfinder
            this.pathFinder = pathfinder;
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
