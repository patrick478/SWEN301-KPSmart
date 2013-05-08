using System;
using System.Text;

namespace Common
{
    /// <summary>
    /// Represents a country that mail can be sent to.
    /// </summary>
    public class Country: DataObject
    {


        /// <summary>
        ///  the country name
        /// </summary>
        private string name;   
        public string Name
        {
            get { return name; }
            set 
            {
                // validation       
                if (value == null)
                    throw new InvalidObjectStateException("Name","Name cannot be null");
                
                name = value; 
            }
        }

       /// <summary>
       /// 3 character code
       /// </summary>
       private string code;
       public string Code {
           get { return code; }
           set
           {
               // validate
               if(value.Length > 3)
                   throw new InvalidObjectStateException("Code", "Code cannot be more than 3 characters");

               code = value.ToUpper();
           }
       }

       public override string ToString()
       {
           return string.Format("[Name: {0}, Code: {1}, ID: {2}, LastModified: {3}]", name, code, ID, LastEdited);
       }

       public override bool Equals(object obj)
       {
           if (ReferenceEquals(null, obj)) return false;
           if (ReferenceEquals(this, obj)) return true;
           if (obj.GetType() != this.GetType()) return false;
           return Equals((Country) obj);
       }

       protected bool Equals(Country other)
       {
           return string.Equals(name, other.name);
       }

       public override int GetHashCode()
       {
           unchecked
           {
               return ((name != null ? name.GetHashCode() : 0) * 397);
           }
       }

       public override string ToNetString()
       {
           return NetCodes.BuildNetworkString(base.ToNetString(), NetCodes.OBJECT_COUNTRY, Code, Name);
       }
    }






}
