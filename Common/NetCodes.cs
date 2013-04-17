using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 
    /// </summary>
    static public class NetCodes
    {
        /* TODO So at the moment just using a OK confirm, but due to unpredictability of the network, may need to match a confirm to the action,
         * in case an OK gets sent for an old action after the client has made a new action or w/e. So when the Client sends any request, 
         * it'll need to preceed all the other prams (or maybe even the CL_ itself) with some kind of id, that the server will take and include 
         * in the confirmed/error responses.
         * Given the non-terrible environment that we and the markers will probably be running the client and server in, this problem situation 
         * realistically wouldn't ever occur but should probably include it for completeness sakes.
         */

        // The values must be unique within a prefix.

        // Client to Server - First token of a message sent by a Client, identifies the action the Client wants to take.

        public const String CL_DELIVERY_REQUEST = "dr";   // Origin Location ID (int) - Destination Location ID (int) - Weight (int) - Volume (int)
        public const String CL_DELIVERY_SELECT = "ds";    // Priority (PRIORITY_ / CANCEL)

        public const String CL_ROUTE_EDIT = "re";         // Route ID (int) - Company ID (int) - Transport Type (TRANSPORT_) - Origin Location ID (int) - Destination Location ID (int) - Cost/gram (int) - Cost/cm3 (int)
        public const String CL_ROUTE_DELETE = "rd";       // Route ID (int)

        public const String CL_PRICE_EDIT = "pe";         // Origin Location ID (int) - Destination Location ID (int) - Priority (PRIORITY_) - Price/gram (int) - Price/cm3 (int)
                                                            // CHECK For Price Updates, are we going to store them by ID, or like above where we just search it on the fly.

        public const String CL_LOCATION_ADD = "la";       // Location Name (String ...)     TODO Later if we get really pro with maps integration: - Longitude - Latititude. Unless we just pass the name along to the api and it finds it itself.
        public const String CL_LOCATION_DELETE = "ld";    // Location ID (int)

        public const String CL_COMPANY_ADD = "ca";       // Company Name (String ...)
        public const String CL_COMPANY_DELETE = "cd";    // Company ID (int)    CHECK Or just the name again? Depends if we use IDs for Company.


        // Server to Client - First token of a message sent by the Server, identifies the information the Server is sending.

        public const String SV_DELIVERY_PRICES = "dp";    // Standard Price (int) - Air Price (int)

        public const String SV_CONFIRM = "ok";  // ??
        public const String SV_ERROR = "er"; // Error Message (String ...)   // CHECK unless we want to have error codes. Or have both; a code followed by a string.


        // State Updates
        public const String UPDATE_ROUTE = "ur";

        public const String PRIORITY_AIR = "a";
        public const String PRIORITY_STANDARD = "s";

        public const String TRANSPORT_AIR = "a";
        public const String TRANSPORT_LAND = "l";
        public const String TRANSPORT_SEA = "s";

        public const String CANCEL = "x";
    }
}
