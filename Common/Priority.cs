//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System;

namespace Common{
    /// <summary>
    /// Describes the priority of a Delivery.  Air or Standard.
    /// 
    /// A delivery is Standard if it contains some routes with 'Land' or 'Sea' TransportType.
    /// A delivery is Air if it only contains routes with 'Air' as the TransportType.  
    /// </summary>
    public enum Priority
    {
        Standard,
        Air
    }

    public static class PriorityExtensions
    {
        public static string ToNetString(this Priority priority)
        {
            switch (priority)
            {
                case Priority.Air:
                    return NetCodes.PRIORITY_AIR;
                case Priority.Standard:
                    return NetCodes.PRIORITY_STANDARD;
                default:
                    throw new ArgumentException("Unsupported enum value");
            }
        }

        public static Priority ParseNetString(string raw)
        {
            switch (raw)
            {
                case NetCodes.PRIORITY_AIR:
                    return Priority.Air;
                case NetCodes.PRIORITY_STANDARD:
                    return Priority.Standard;
                default:
                    throw new ArgumentException("Unsupported network token");
            }
        }
    }
}
