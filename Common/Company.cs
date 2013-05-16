//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

namespace Common
{
    /// <summary>
    /// Represents a company that offers Route services.
    /// </summary>
    public class Company: DataObject
    {

        public string Name { get; set; }

        public override string ToString ()
        {
            return string.Format("Company[ID={0}, LastEdited={1}, Name={2}]", ID, LastEdited, Name);
        }


        public override bool Equals (object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Company)obj);
        }

        protected bool Equals (Company other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode ()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397);
            }
        }


        public string ToShortString ()
        {
            return Name;
        }

        public override string ToNetString()
        {
            return NetCodes.BuildNetworkString(base.ToNetString(), NetCodes.OBJECT_COMPANY, Name);
        }
    }
}
