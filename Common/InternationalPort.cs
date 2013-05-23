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
    /// Represents an International Port.  
    /// 
    /// Routes can go between all International ports.  
    /// There cannot be a route between a DistributionCentre and an International port, unless the InternationalPort is in NZ.
    /// 
    /// It is assumed that there is only one InternationalPort per country.
    /// TODO: decide if NZ has one.
    /// </summary>
    public class InternationalPort: RouteNode 
    {
        public InternationalPort(Country country) : base(country)
        {
         
        }

        public override string ToString ()
        {
            return Country.ToString();
        }

        public override string ToShortString ()
        {
            return Country.Name;
        }

        public string ToDebugString () 
        {
            return string.Format("InternationalPort[ID={0}, Country={2}, LastEdited={1}]", ID, LastEdited, Country.ToShortString());
        }

        public override string ToNetString()
        {
            return NetCodes.BuildObjectNetString(base.ToNetString(), NetCodes.NODE_INTERNATIONAL, Convert.ToString(Country.ID));
        }

    }
}
