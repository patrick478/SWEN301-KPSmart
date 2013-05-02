using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Server.Business
{
    public class DeliveryOption
    {

        public DeliveryOption(DateTime timestamp)
        {
            this.Timestamp = timestamp;
            this.options = new Dictionary<PathType, Delivery>();
        }

        public DateTime Timestamp { get; private set; }

        private readonly Dictionary<PathType, Delivery> options; 

        public Delivery GetOption(PathType type)
        {
            return options[type];
        }

        public void AddOption(PathType type, Delivery delivery)
        {
            if (delivery == null)
                throw new ArgumentException("Delivery cannot be null", "delivery");

            options[type] = delivery;
        }

        public IDictionary<PathType, Delivery> GetOptions()
        {
            return new Dictionary<PathType, Delivery>(options);
        }
    }
}
