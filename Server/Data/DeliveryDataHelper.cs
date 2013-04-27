using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Server.Data
{
    public class DeliveryDataHelper: DataHelper<Delivery>
    {
        public override Delivery Load(int id)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<int, Delivery> LoadAll()
        {
            throw new NotImplementedException();
        }

        public override IDictionary<int, Delivery> LoadAll(DateTime snapshotTime)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Delivery obj)
        {
            throw new NotImplementedException();
        }

        public override void Update(Delivery obj)
        {
            throw new NotImplementedException();
        }

        public override void Create(Delivery obj)
        {
            throw new NotImplementedException();
        }

        public override int GetId(Delivery country)
        {
            throw new NotImplementedException();
        }
    }
}
