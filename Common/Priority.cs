//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

namespace Common
    /// <summary>
    /// Describes the priority of a Delivery.
    /// 
    /// A delivery is Standard if it contains some routes with 'Land' or 'Sea' TransportType.
    /// A delivery is Air if it only contains routes with 'Air' as the TransportType.  
    /// </summary>
    public enum Priority
    {
        Standard,
        Air
    }
}
