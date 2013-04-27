//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

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
    }
}
