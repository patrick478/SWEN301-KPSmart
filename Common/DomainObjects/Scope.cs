//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

namespace Common.DomainObjects
{
    /// <summary>
    /// Represents the scope of a Route or Delivery.  
    /// 
    /// A Route is International if its origin or destination is an InternationalPort.
    /// A Route is Domestic if both origin and destination are within NZ.
    /// 
    /// A Delivery is International if an of its routes go to an InternationalPort.
    /// A Delivery is Domestic if all routes go between destinations in NZ.
    /// </summary>
    public enum Scope
    {
        International,
        Domestic
    }
}
