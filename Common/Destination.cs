//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

namespace Common
{
    /// <summary>
    /// This class represents a destination that mail can be sent to.
    /// </summary>
    public abstract class Destination
    {

        protected Destination(Country country) 
        {
            Country = country;
        }

        // the country that the Destination is in
        public Country Country { get; private set; }
    }
}
