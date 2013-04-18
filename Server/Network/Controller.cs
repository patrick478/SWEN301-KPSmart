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

        /// <summary>
        /// Reads a message received from the Client and performs the appropriate actions.
        /// </summary>
        /// <param name="client">Client the message was sent from.</param>
        /// <param name="data">The message.</param>
        public static void OnReceived(Client client, string data)
        {
            // TODO try catches for string arrays that are missing expected arguments. And can't be converted etc.
            var tokens = data.Split();

            switch (tokens[0])
            {
                case Common.NetCodes.CL_DELIVERY_REQUEST:
                    DeliveryRequest(client, tokens);
                    return;
                case Common.NetCodes.CL_DELIVERY_SELECT:
                    DeliverySelect(client, tokens);
                    return;

                case Common.NetCodes.CL_ROUTE_EDIT:
                    RouteEdit(client, tokens);
                    return;
                case Common.NetCodes.CL_ROUTE_DELETE:
                    RouteDelete(client, tokens);
                    return;

                case Common.NetCodes.CL_PRICE_EDIT:
                    PriceEdit(client, tokens);
                    return;

                case Common.NetCodes.CL_LOCATION_ADD:
                    LocationAdd(client, tokens);
                    return;
                case Common.NetCodes.CL_LOCATION_DELETE:
                    LocationDelete(client, tokens);
                    return;

                case Common.NetCodes.CL_COMPANY_ADD:
                    CompanyAdd(client, tokens);
                    return;
                case Common.NetCodes.CL_COMPANY_DELETE:
                    CompanyDelete(client, tokens);
                    return;
            }

        }

        private static void DeliveryRequest(Client client, string[] tokens)
        {
            int count = 1;
            int originID = Convert.ToInt32(tokens[count++]);
            int destinationID = Convert.ToInt32(tokens[count++]);
            int weight = Convert.ToInt32(tokens[count++]);
            int volume = Convert.ToInt32(tokens[count++]);
            //Common.Delivery air = DeliveryService.Build(originID, destinationID, Common.NetCodes.PRIORITY_AIR, weight, volume);
            //Common.Delivery standard = DeliveryService.Build(originID, destinationID, Common.NetCodes.PRIORITY_STANDARD, weight, volume);
            //client.StorePendingDelivery(air, standard);
            //Transmit(client,BuildTransmissionString(Common.NetCode.SV_DELIVERY_PRICES,air.TotalPrice,standard.TotalPrice));
        }

        private static void DeliverySelect(Client client, string[] tokens)
        {
            int count = 1;
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
        }

        private static void RouteEdit(Client client, string[] tokens)
        {
            int count = 1;
            int routeID = Convert.ToInt32(tokens[count++]);
            int companyID = Convert.ToInt32(tokens[count++]);
            Common.TransportType type;
            if (tokens[count] == Common.NetCodes.TRANSPORT_AIR)
                type = Common.TransportType.Air;
            else if (tokens[count] == Common.NetCodes.TRANSPORT_SEA)
                type = Common.TransportType.Sea;
            else
                type = Common.TransportType.Land;   // TODO Just go with the flow and avoid errors by assuming anything else is Land? Or manually check for Land and else is a error?
            ++count;
            int originID = Convert.ToInt32(tokens[count++]);
            int destinationID = Convert.ToInt32(tokens[count++]);
            int weightCost = Convert.ToInt32(tokens[count++]);
            int volumeCost = Convert.ToInt32(tokens[count++]);

            //Common.Route r = Server.Business.RouteService.GetRoute(routeID);
        }

        private static void RouteDelete(Client client, string[] tokens)
        {
            int count = 1;
            int routeID = Convert.ToInt32(tokens[count++]);
            //Server.Business.RouteService.GetRoute/DeleteRoute(routeID);
        }

        private static void PriceEdit(Client client, string[] tokens)
        {
            int count = 1;
        }

        private static void LocationAdd(Client client, string[] tokens)
        {
            int count = 1;
        }

        private static void LocationDelete(Client client, string[] tokens)
        {
            int count = 1;
        }

        private static void CompanyAdd(Client client, string[] tokens)
        {
            int count = 1;
        }

        private static void CompanyDelete(Client client, string[] tokens)
        {
            int count = 1;
        }

        /*
         * TODO Maybe change the token seperator for network messages to something other than space, 
         * as some of the string params like Location Name can contain spaces, which restricts to only 
         * having one Space-able string per transmission, which is a lame limitation.
         * \t instead maybe? Using UTF-8, where ASCII 0-127 characters only cost one byte, so should use one of them.
         */


        /// <summary>
        /// Creates a string out of all the string parameters, seperated by spaces. Ensures at least one string is given.
        /// </summary>
        /// <param name="first">First string</param>
        /// <param name="rest">Remaining strings</param>
        /// <returns></returns>
        private static string BuildTransmissionString(string first, params string[] rest)
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