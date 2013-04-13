using System;

namespace Common
{
    /// <summary>
    /// Represents a country that mail can be sent to.
    /// </summary>
    public class Country: DataObject
    {
       // the country name
       public string Name { get; set; }

       // 3 character code
       public string Code { get; set; }
    }
}
