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
        public RouteNode Origin { get; set; }
        public RouteNode Destination { get; set; }
        public Priority Priority { get; set; } 

        // in cents
        public int PricePerGram { get; set; }

        // in cents
        public int PricePerCm3 { get; set; }

        public override string ToNetString()
        {
            return NetCodes.BuildNetworkString(base.ToNetString(), NetCodes.OBJECT_PRICE, Convert.ToString(Origin.ID), Convert.ToString(Destination.ID), Priority.ToNetString(), Convert.ToString(PricePerGram), Convert.ToString(PricePerCm3));
        }
    }
}
