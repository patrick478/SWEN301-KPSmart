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
                    throw new ArgumentException("This method doesn't know about that PathType");
            }
        }
    }


}
