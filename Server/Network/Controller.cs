using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Convert;

namespace Server.Network
{
    /// <summary>
    /// Passes information between the Server's Business components and the connected Clients.
    /// TODO Come up with a more logical name for the class.
    /// </summary>
    public static class Controller
    {
        /// <summary>
        /// Actions to be performed when a Client first connects.
        /// </summary>
        /// <param name="client">Client that connected.</param>
        public static void OnConnected(Client client)
        {

        }

        public static void OnReceived(Client client, String data)
        {
            var tokens = data.Split();
            int count = 0;

            switch (tokens[count++])
            {
                case Common.NetCodes.CL_DELIVERY_REQUEST:
                    int originID = Convert.ToInt32(tokens[count++]);
                    int destinationID = Convert.ToInt32(tokens[count++]);
                    int weight = Convert.ToInt32(tokens[count++]);
                    int volume = Convert.ToInt32(tokens[count++]);
                    Common.Delivery air; // TODO Generate Delivery for Standard and Air priorities
                    Common.Delivery standard; // TODO
                    //client.StorePendingDelivery(air, standard);
                    //Transmit(client,BuildTransmissionString(Common.NetCode.SV_DELIVERY_PRICES,air.TotalPrice,standard.TotalPrice));
                    return;
                case Common.NetCodes.CL_DELIVERY_SELECT:
                    // TODO Implement the timeout stuff
                    Common.Priority prio;
                    if (tokens[count] == Common.NetCodes.PRIORITY_AIR)
                        prio = Common.Priority.Air;
                    else if (tokens[count] == Common.NetCodes.PRIORITY_STANDARD)
                        prio = Common.Priority.Standard;
                    //else
                        //prio = null;
                    //Common.Delivery delivery = client.GetPendingDelivery(prio);
                    //if (delivery != null)
                        //DeliveryManager.Commit(delivery);
                    //Transmit(client, BuildTransmissionString(Common.NetCode.SV_DELIVERY_CONFIRM));
                    return;

                case Common.NetCodes.CL_ROUTE_EDIT:
                    return;
                case Common.NetCodes.CL_ROUTE_DELETE:
                    return;

                case Common.NetCodes.CL_PRICE_EDIT:
                    return;

                case Common.NetCodes.CL_LOCATION_ADD:
                    return;
                case Common.NetCodes.CL_LOCATION_DELETE:
                    return;

                case Common.NetCodes.CL_COMPANY_ADD:
                    return;
                case Common.NetCodes.CL_COMPANY_DELETE:
                    return;
            }

        }

        /*
         * TODO Maybe change the token seperator for network messages to something other than space, 
         * as some of the String params like Location Name can contain spaces, which restricts to only 
         * having one Space-able string per transmission, which is a lame limitation.
         * \t instead maybe? Using UTF-8, where ASCII 0-127 characters only cost one byte, so should use one of them.
         */


        /// <summary>
        /// Creates a String out of all the String parameters, seperated by spaces. Ensures at least one String is given.
        /// </summary>
        /// <param name="first">First String</param>
        /// <param name="rest">Remaining Strings</param>
        /// <returns></returns>
        private static String BuildTransmissionString(String first, params String[] rest)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(first);
            for (int i = 0; i < rest.Length; ++i)
            {
                builder.Append(" ");
                builder.Append(rest[i]);
            }
            return builder.ToString();
        }
    }
}