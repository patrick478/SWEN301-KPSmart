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
    /// This class represents a RouteNode that mail can be sent to.
    /// </summary>
    public abstract class RouteNode : DataObject
    {
        protected RouteNode(Country country)
        {           
            Country = country;
        }

        // the country that the RouteNode is in
        private Country country;
        public Country Country
        {
            get { return country; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("Country", "Country cannot be set to null.");

                this.country = value;
            }
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RouteNode)obj);
        }

        protected bool Equals(RouteNode other)
        {
            return Equals(Country, other.Country);
        }

        public override int GetHashCode()
        {
            return (Country != null ? Country.GetHashCode() : 0);
        }

        public static RouteNode ParseNetString(string objectDef, State state)
        {
            string[] tokens = objectDef.Split(NetCodes.SEPARATOR_FIELD);
            int count = 0;
            string type = tokens[count++];
            if (type == NetCodes.NODE_INTERNATIONAL)
            {
                int countryId = Convert.ToInt32(tokens[count++]);
                return new InternationalPort(state.GetCountry(countryId));
            }
            else
            {
                string name = tokens[count++];
                return new DistributionCentre(name);
            }
        }
    }
}
