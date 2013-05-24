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
    /// Represents the transport type of a Route
    /// </summary>
    public enum TransportType
    {
        Sea,
        Land,
        Air
    }

    public static class TransportTypeExtensions
    {
        public static string ToNetString(this TransportType type)
        {
            switch (type)
            {
                case TransportType.Air:
                    return NetCodes.TRANSPORT_AIR;
                case TransportType.Land:
                    return NetCodes.TRANSPORT_LAND;
                case TransportType.Sea:
                    return NetCodes.TRANSPORT_SEA;
                default:
                    throw new ArgumentException("Unsupported enum value");
            }
        }

        public static TransportType ParseNetString(string raw)
        {
            switch (raw)
            {
                case NetCodes.TRANSPORT_AIR:
                    return TransportType.Air;
                case NetCodes.TRANSPORT_LAND:
                    return TransportType.Land;
                case NetCodes.TRANSPORT_SEA:
                    return TransportType.Sea;
                default:
                    throw new ArgumentException("Unsupported network token");
            }
        }

        public static TransportType ParseTransportTypeFromString (this string raw) 
        {
            switch (raw.ToLower()) 
            { 
                case "air":
                    return TransportType.Air;
                case "land":
                    return TransportType.Land;
                case "sea":
                    return TransportType.Sea;
                default:
                    throw new ArgumentException("Unsupported string value: " + raw);
            } 
       
        }
    }
}
