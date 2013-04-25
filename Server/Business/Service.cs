using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Data;

namespace Server.Business
{
    public abstract class Service<T> where T:DataObject
    {
        protected CurrentState state;
        protected DataHelper<T> dataHelper;

        protected Service (CurrentState state, DataHelper<T> dataHelper)
        {
            this.state = state;
            this.dataHelper = dataHelper;
        }

        public abstract T Get(int id);

        public abstract IEnumerable<T> GetAll();

        public abstract bool Exists(T obj);

        public abstract void Delete(int id);

    }
}
