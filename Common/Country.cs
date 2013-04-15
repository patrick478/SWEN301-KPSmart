using System;

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
                if (name == null)
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
    }
}
