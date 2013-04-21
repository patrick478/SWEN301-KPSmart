//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

namespace Common
{
    /// <summary>
    /// This class represents a RouteNode that mail can be sent to.
    /// </summary>
    public abstract class RouteNode: DataObject
    {

        protected RouteNode(string country)
        {
            Country = country;
        }

        // the country that the RouteNode is in
        protected string Country { get; set; }
    }
}
