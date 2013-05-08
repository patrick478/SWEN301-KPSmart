//////////////////////
// Original Writer: Joshua Scott
// Reviewed by: 
//
// 
//////////////////////

using System;

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
    }


}
