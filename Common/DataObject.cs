using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// The class that all objects loaded from DB should inherit from.
    /// </summary>
    public abstract class DataObject
    {
        // the id from the DB
        public int ID { get; set; }

        // the time it was last changed
        public DateTime LastEdited { get; set; }

        public virtual string ToNetString()
        {
            return ID.ToString();
        }

        public virtual string ToShortString () 
        {
            return ID.ToString();
        }
    }
}
