using System;

namespace Common
{
    /// <summary>
    /// Represents a country that mail can be sent to.
    /// </summary>
    public class Country: DataObject
    {
       
        public Country(string name, string code)
        {
            this.Name = name;
            this.Code = code;
        }

        public string Name { get; set; }

        private string _code;
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                if (value.Length > 3) throw new Exception("Country code must not be more than 3 letters");
                _code = value;
            }
        }
    }
}
