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
            return new Price() { Origin = state.GetRouteNode(originId), Destination = state.GetRouteNode(destinationId), Priority = priority, PricePerGram = weightPrice, PricePerCm3 = volumePrice };
        }
    }
}
