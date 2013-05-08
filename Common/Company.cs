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

        public override string ToNetString()
        {
            return NetCodes.BuildNetworkString(base.ToNetString(), NetCodes.OBJECT_COMPANY, Name);
        }
    }
}
