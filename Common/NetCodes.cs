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
        public const string CL_DELIVERY_SELECT = "ds";    // Priority (PRIORITY_ / CANCEL)

        public const string CL_ROUTE_EDIT = "re";         // Route ID (int) - Company ID (int) - Transport Type (TRANSPORT_) - Origin Location ID (int) - Destination Location ID (int) - Cost/gram (int) - Cost/cm3 (int) TODO Add Times and Max Weight/Volume per trip etc.
        public const string CL_ROUTE_DELETE = "rd";       // Route ID (int)

        public const string CL_PRICE_EDIT = "pe";         // Origin Location ID (int) - Destination Location ID (int) - Priority (PRIORITY_) - Price/gram (int) - Price/cm3 (int)
                                                            // CHECK For Price Updates, are we going to store them by ID, or like above where we just search it on the fly.

        public const string CL_LOCATION_ADD = "la";       // Location Code (3char string) - Location Name (string ...)     TODO Later if we get really pro with maps integration: - Longitude - Latititude. Unless we just pass the name along to the api and it finds it itself.
        public const string CL_LOCATION_DELETE = "ld";    // Location ID (int)

        public const string CL_COMPANY_ADD = "ca";       // Company Name (string ...)
        public const string CL_COMPANY_DELETE = "cd";    // Company ID (int)    CHECK Or just the name again? Depends if we use IDs for Company.


        // Server to Client - First token of a message sent by the Server, identifies the information the Server is sending (and the format for the rest of the message).

        public const string SV_DELIVERY_PRICES = "dp";    // Standard Price (int) - Air Price (int)

        public const string SV_CONFIRM = "ok";  // ??
        public const string SV_ERROR = "er"; // Error Message (string ...)   // CHECK unless we want to have error codes. Or have both; a code followed by a string.


        // State Updates
        public const string UPDATE_ROUTE = "ur";

        public const string PRIORITY_AIR = "a";
        public const string PRIORITY_STANDARD = "s";

        public const string TRANSPORT_AIR = "a";
        public const string TRANSPORT_LAND = "l";
        public const string TRANSPORT_SEA = "s";

        public const string CANCEL = "x";
    }
}
