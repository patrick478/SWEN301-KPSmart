//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System;

namespace Common
{
    /// <summary>
    /// Represents the price charged to a customer between the given Origin, Destination, and Priority.  
    /// 
    /// There should be only one Price defined for any combination of Origin, Destination, and Priority.
    /// 
    /// 
    /// </summary>
    public class Price: DataObject
    {
        private RouteNode origin;
        public RouteNode Origin 
        {
            get { return origin; }      
            set 
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("Origin", "Origin cannot be set to null.");

                if (value.Equals(Destination))
                    throw new InvalidObjectStateException("Origin", "Origin cannot be the same as the Destination.");

                this.origin = value;
            }
        }

        private RouteNode destination;
        public RouteNode Destination 
        {

            get { return destination; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("Destination", "Destination cannot be set to null.");

                if (value.Equals(Origin))
                    throw new InvalidObjectStateException("Destination", "Destination cannot be the same as the Origin.");

                this.destination = value;
            }
        }

        private Priority priority;
        public Priority Priority 
        {
            get { return priority; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("Priority", "Priority cannot be set to null.");

                this.priority = value;
            }
        } 

        // in cents
        private int pricePerGram;
        public int PricePerGram 
        {
            get { return pricePerGram; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("PricePerGram", "PricePerGram cannot be less than or equal to 0.");

                this.pricePerGram = value;
            }
        }

        // in cents
        private int pricePerCm3;
        public int PricePerCm3 {

            get { return pricePerCm3; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("PricePerCm3", "PricePerCm3 cannot be less than or equal to 0.");

                this.pricePerCm3 = value;
            }  
        }







        public override string ToNetString()
        {
            return NetCodes.BuildObjectNetString(base.ToNetString(), Convert.ToString(Origin.ID), Convert.ToString(Destination.ID), Priority.ToNetString(), Convert.ToString(PricePerGram), Convert.ToString(PricePerCm3));
        }

        public static Price ParseNetString(string objectDef, State state)
        {
            string[] tokens = objectDef.Split(NetCodes.SEPARATOR_FIELD);
            int count = 0;
            int id = Convert.ToInt32(tokens[count++]);
            int originId = Convert.ToInt32(tokens[count++]);
            int destinationId = Convert.ToInt32(tokens[count++]);
            Priority priority = PriorityExtensions.ParseNetString(tokens[count++]);
            int weightPrice = Convert.ToInt32(tokens[count++]);
            int volumePrice = Convert.ToInt32(tokens[count++]);
            return new Price() { ID = id, Origin = state.GetRouteNode(originId), Destination = state.GetRouteNode(destinationId), Priority = priority, PricePerGram = weightPrice, PricePerCm3 = volumePrice };
        }



        public override bool Equals (object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Price)obj);
        }

        protected bool Equals (Price other)
        {
            return this.Origin.Equals(other.Origin) && this.Destination.Equals(other.Destination) && this.Priority == other.Priority;
        }

        public override int GetHashCode ()
        {
            unchecked
            {
                var hashCode = (int)Priority;
                hashCode = (hashCode * 397) ^ (Origin != null ? Origin.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Destination != null ? Destination.GetHashCode() : 0);
                return hashCode;
            }
        }



    }
}
