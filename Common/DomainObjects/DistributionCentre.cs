//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

namespace Common.DomainObjects
{
    /// <summary>
    /// This represents a destination within NZ that mail can originate from, and be sent to. 
    /// Routes can only go between two DistributionCentres, or between a DistributionCentre and the NZ InternationalPort
    /// 
    /// TODO: work out if these will be hardcoded, work out if we want to be able to sent from a distribution centre to other external ports and not have a NZ InternationlPort
    /// </summary>
    public class DistributionCentre: Destination
    {
        // TODO: need to work out how to access countries - this should set country as NZ, but not sure where it gets it from yet, so put it in constructor for now
        public DistributionCentre(string name, Country country) : base(country)
        {
            Name = name;
        }

        // The name of the distribution centre.
        public string Name
        {
            get; private set;
        }
    }
}
