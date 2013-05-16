﻿//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

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
            return string.Format("InternationalPort[ID={0}, LastEdited={1}, Country={2}]", ID, LastEdited, Country);
        }

        public override string ToShortString ()
        {
            return Country.Name;
        }

    }
}
