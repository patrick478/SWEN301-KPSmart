//////////////////////
// Original Writer: Joshua Scott
// Reviewed by: 
//
// 
//////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum PathType
    {
        Standard,
        Express,
        AirStandard,
        AirExpress
    }


    public static class PathTypeExtensions
    {
        public static Priority GetPriority(this PathType pathType)
        {
            switch (pathType)
            {
                case PathType.Standard:
                    return Priority.Standard;
                case PathType.Express:
                    return Priority.Standard;
                case PathType.AirStandard:
                    return Priority.Air;
                case PathType.AirExpress:
                    return Priority.Air;
                default:
                    throw new ArgumentException("Unsupported enum value");
            }
        }

        public static string ToNetString(this PathType type)
        {
            switch (type)
            {
                case PathType.AirStandard:
                    return NetCodes.PATH_AIR;
                case PathType.AirExpress:
                    return NetCodes.PATH_AIRXPRESS;
                case PathType.Standard:
                    return NetCodes.PATH_STANDARD;
                case PathType.Express:
                    return NetCodes.PATH_STANDARDXPRESS;
                default:
                    throw new ArgumentException("Unsupported enum value");
            }
        }

        public static PathType ParseNetString(string raw)
        {
            switch (raw)
            {
                case NetCodes.PATH_AIR:
                    return PathType.AirStandard;
                case NetCodes.PATH_AIRXPRESS:
                    return PathType.AirExpress;
                case NetCodes.PATH_STANDARD:
                    return PathType.Standard;
                case NetCodes.PATH_STANDARDXPRESS:
                    return PathType.Express;
                case NetCodes.PATH_CANCEL:
                    throw new ArgumentException("Cannot retrieve a PathType from the CANCEL token");
                default:
                    throw new ArgumentException("Unsupported network token");
            }
        }

        /// <summary>
        /// Creates a network string containing mulitple PathTypes mapped to Prices (integers). Used when sending Delivery Options to a client.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string BuildOptionsNetString(IDictionary<PathType,Delivery> options)
        {
            StringBuilder builder = new StringBuilder();
            bool first = true;
            foreach (KeyValuePair<PathType, Delivery> entry in options)
            {
                if (first)
                    first = false;
                else
                    builder.Append(NetCodes.SUBSEPARATOR);
                builder.Append(entry.Key.ToNetString());
                builder.Append(NetCodes.SUBSEPARATOR);
                builder.Append(entry.Value.TotalPrice);
            }
            return builder.ToString();
        }

        //TODO - Add a sub-seperator token or just use normal seperator? (seeing as this SHOULD always be the last part of a net message)
        /// <summary>
        /// Builds a dictonary mapping PathTypes to prices (integers) from a network string (that was generated via the BuildOptionsNetString method). Used when receiving Delivery Options from the server.
        /// </summary>
        /// <returns>Dictonary mapping PathType to price (integer)</returns>
        public static IDictionary<PathType, int> ParseOptionsNetString(string options)
        {
            string[] tokens = options.Split(NetCodes.SUBSEPARATOR);
            IDictionary<PathType, int> prices = new Dictionary<PathType,int>();
            for (int i = 0; i < tokens.Length; )
            {
                PathType type = PathTypeExtensions.ParseNetString(tokens[i++]);
                int price = Convert.ToInt32(tokens[i++]);
                prices.Add(type, price);
            }
            return prices;
        }
    }


}
