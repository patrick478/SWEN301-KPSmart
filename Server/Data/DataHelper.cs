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

        /// <summary>
        /// Returns 0 if it doesn't exist in the Database.
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public abstract int GetId(T country);

        protected string EVENT_ID = "event_id";
        protected string TABLE_NAME;
        protected string ID_COL_NAME;

    }

    public static class DatabaseExtensionMethods
    {
        public static int ToInt(this Object obj)
        {
            long asLong = (long) obj;
            return (int) asLong;
        }
    }
}
