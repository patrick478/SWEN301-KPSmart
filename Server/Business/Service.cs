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


        /// <summary>
        /// Gets the object from the system.
        /// </summary>
        /// <param name="id">the id of the object to get</param>
        /// <exception cref="ArgumentException">if id is <= 0</exception>>
        public abstract T Get(int id);

        public abstract IEnumerable<T> GetAll();

        public abstract bool Exists(T obj);

        /// <summary>
        /// Deletes the object from the system.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="IllegalActionException">If the object being deleted is used by another object</exception>>
        /// <exception cref="ArgumentException">if id is <= 0</exception>>
        public abstract void Delete(int id);

    }
}
