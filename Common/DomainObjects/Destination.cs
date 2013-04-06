//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

namespace Common.DomainObjects
{
    /// <summary>
    /// This class represents a destination that mail can be sent to.
    /// </summary>
    public abstract class Destination
    {

        public Destination(Country country) 
        {
            Country = country;
        }

        // the country that the Destination is in
        public Country Country { get; private set; }
    }
}
