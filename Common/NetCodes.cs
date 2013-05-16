using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// Static class containing the codes used in the messages sent between the client and server. Also contains documentation for formats of the messages etc.
    /// </summary>
    static public class NetCodes
    {
        /* TODO So at the moment just using an OK confirm, but due to unpredictability of the network, may need to match a confirm to the action,
         * in case an OK gets sent for an old action after the client has made a new action or w/e. So when the Client sends any request, 
         * it'll need to preceed all the other prams (or maybe even the CL_ itself) with some kind of id, that the server will take and include 
         * in the confirmed/error responses.
         * Given the non-terrible environment that we and the markers will probably be running the client and server in, this problem situation 
         * realistically wouldn't ever occur but should probably include it for completeness sakes.
         */

        // The values must be unique within a prefix.

        // Client to Server - First token of a message sent by a Client, identifies the action the Client wants to take (and the format for the rest of the message).

        public const string CL_DELIVERY_REQUEST = "dr";   // Origin Location ID (int) - Destination Location ID (int) - Weight (int) - Volume (int)
        public const string CL_DELIVERY_SELECT = "ds";    // Type (PATH_)

        public const string CL_OBJECT_ADD = "oa";       // Type (OBJECT_) - See specific object for rest of protocol...
        public const string CL_OBJECT_EDIT = "oe";    // ID (int) - Type (OBJECT_) - See specific object for rest of protocol...
        public const string CL_OBJECT_DELETE = "od";    // ID (int) - Type (OBJECT_)

        // Server to Client - First token of a message sent by the Server, identifies the information the Server is sending (and the format for the rest of the message).

        public const string SV_DELIVERY_PRICES = "dp";    // PATH_CANCEL or PATH_ 
        public const string SV_DELIVERY_CONFIRMED = "dc";    // 
        public const string SV_ERROR = "er"; // Error Message (string ...)   // CHECK unless we want to have error codes. Or have both; a code followed by a string.
        public const string SV_OBJECT_UPDATE = "ou";    // DateTime String - ID (int) - Type (OBJECT_) - See specific object for rest of protocol...
        public const string SV_OBJECT_DELETE = "od";    // DateTime String - ID (int) - Type (OBJECT_)

        // State Updates - Fields marked with astericks are for fields also used in EDITs
        public const string OBJECT_COUNTRY = "l";   // ... - *Location Code (3char string) - Location Name (string)     TODO Later if we get really pro with maps integration: - Longitude - Latititude. Unless we just pass the name along to the api and it finds it itself.
        public const string OBJECT_PRICE = "p";     // ... - Origin Location ID (int) - Destination Location ID (int) - Priority (PRIORITY_) - *Price/gram (int) - *Price/cm3 (int)
        public const string OBJECT_ROUTE = "r";     // ... - Origin Location ID (int) - Destination Location ID (int) - Company ID (int) - Transport Type (TRANSPORT_) - *Cost/gram (int) - *Cost/cm3 (int) - *Max Weight (int) - *Max Capacity (int) - *Trip Duration (int) - *Trip Times 
        public const string OBJECT_COMPANY = "c";   // ... - Company Name (string)

        public const string PATH_AIR = "a";
        public const string PATH_STANDARD = "s";
        public const string PATH_AIRXPRESS = "A";
        public const string PATH_STANDARDXPRESS = "S";
        public const string PATH_CANCEL = "c";

        public const string PRIORITY_AIR = "a";
        public const string PRIORITY_STANDARD = "s";

        public const string TRANSPORT_AIR = "a";
        public const string TRANSPORT_LAND = "l";
        public const string TRANSPORT_SEA = "s";

        /// <summary>Character used to seperate tokens in a network message.</summary>
        public const char SEPARATOR = '|';
        public const char SEPARATOR_ELEMENT = '\t';
        public const char SEPERATOR_TIME = ':';

        /// <summary>
        /// Creates a string out of all the string parameters, seperated by the seperator character. Ensures at least one string is given.
        /// </summary>
        /// <param name="first">First string</param>
        /// <param name="rest">Remaining strings</param>
        /// <returns></returns>
        public static string BuildNetworkString(string first, params string[] rest)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(first);
            for (int i = 0; i < rest.Length; ++i)
            {
                builder.Append(NetCodes.SEPARATOR);
                builder.Append(rest[i]);
            }
            return builder.ToString();
        }

    }
}
