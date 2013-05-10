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

        public override string ToNetString()
        {
            return NetCodes.BuildNetworkString(base.ToNetString(), NetCodes.OBJECT_COMPANY, Name);
        }
    }
}
