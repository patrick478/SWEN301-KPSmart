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
    /// Represents a company that offers Route services.
    /// </summary>
    public class Company: DataObject
    {
        private string name;
        public string Name
        {
            get { return name; } 
            set 
            {
                // validation
                if (value == null || value == String.Empty)
                    throw new InvalidObjectStateException("Name", "Cannot give the Company an empty or null name");

                name = value; 
            } 
        }

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
            return NetCodes.BuildObjectNetString(base.ToNetString(), Name);
        }

        public static Company ParseNetString(string objectDef)
        {
            string[] tokens = objectDef.Split(NetCodes.SEPARATOR_FIELD);
            int count = 0;
            int id = Convert.ToInt32(tokens[count++]);
            string name = tokens[count++];
            return new Company() { Name = name, ID = id };
        }
        
    }
}
