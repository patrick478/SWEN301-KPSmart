using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Server.Data
{
    public abstract class DataHelper<T> where T: DataObject
    {
        public abstract T Load(int id);
        public abstract IDictionary<int, T> LoadAll();
        public abstract IDictionary<int, T> LoadAll(DateTime snapshotTime);

        public abstract void Delete(int id);
        public abstract void Delete(T obj);

        public abstract void Update(T obj);
        public abstract void Create(T obj);

        public abstract int GetId(T country);

        protected string EVENT_ID = "event_id";

    }
}
