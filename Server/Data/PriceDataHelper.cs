using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Server.Data
{
    public class PriceDataHelper: DataHelper<Price>
    {
        public override Price Load(int id)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<int, Price> LoadAll()
        {
            throw new NotImplementedException();
        }

        public override IDictionary<int, Price> LoadAll(DateTime snapshotTime)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Price obj)
        {
            throw new NotImplementedException();
        }

        public override void Update(Price obj)
        {
            throw new NotImplementedException();
        }

        public override void Create(Price obj)
        {
            throw new NotImplementedException();
        }

        public override int GetId(Price country)
        {
            throw new NotImplementedException();
        }
    }
}
