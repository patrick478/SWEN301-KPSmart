//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

namespace Common
{
    /// <summary>
    /// This represents a RouteNode within NZ that mail can originate from, and be sent to. 
    /// Routes can only go between two DistributionCentres, or between a DistributionCentre and the NZ InternationalPort
    /// 
    /// TODO: work out if these will be hardcoded, work out if we want to be able to sent from a distribution centre to other external ports and not have a NZ InternationlPort
    /// </summary>
    public class DistributionCentre: RouteNode
    {


        private static readonly Country NEW_ZEALAND = new Country{Name="New Zealand"}; // should this go in some config file?

        public DistributionCentre(string name) : base(NEW_ZEALAND)
        {
            Name = name;
        }

        // The name of the distribution centre.
        public string Name
        {
            get; private set;
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DistributionCentre)obj);
        }

        protected bool Equals(DistributionCentre other)
        {
            bool value = string.Equals(Name, other.Name);
            return value;
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override string ToString ()
        {
            return string.Format("DistributionCentre[ID={0}, LastEdited={1}, Name={2}]", ID, LastEdited, Name);
        }

    }
}
