using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Business;

namespace Server.Network
{
    /// <summary>
    /// Passes information between the Server's Business components and the connected Clients.
    /// TODO Come up with a more logical name for the class.
    /// </summary>
    public class Controller
    {
        private RouteService routeService;
        private PriceService priceService;
        private LocationService locationService;
        private DeliveryService deliveryService;
        private CountryService countryService;
        private CompanyService companyService;

        public Controller(CountryService countryService, CompanyService companyService, DeliveryService deliveryService, PriceService priceService, RouteService routeService, LocationService locationService)
        {
            this.countryService = countryService;
            this.companyService = companyService;
            this.deliveryService = deliveryService;
            this.priceService = priceService;
            this.routeService = routeService;
            this.locationService = locationService;
        }

        /// <summary>
        /// Actions to be performed when a Client first connects.
        /// </summary>
        /// <param name="client">Client that connected.</param>
        public void OnConnected(Client client)
        {

        }

        /// <summary>
        /// Reads a message received from the Client and performs the appropriate actions.
        /// </summary>
        /// <param name="client">Client the message was sent from.</param>
        /// <param name="data">The message.</param>
        public void OnReceived(Client client, string data)
        {
            // TODO try catches for string arrays that are missing expected arguments. And can't be converted etc.
            var tokens = data.Split(NetCodes.SEPARATOR);

            switch (tokens[0])
            {
                case NetCodes.CL_DELIVERY_REQUEST:
                    DeliveryRequest(client, tokens);
                    return;
                case NetCodes.CL_DELIVERY_SELECT:
                    DeliverySelect(client, tokens);
                    return;

                case NetCodes.CL_OBJECT_ADD:
                    ObjectAdd(client, tokens);
                    return;

                case NetCodes.CL_OBJECT_UPDATE:
                    ObjectUpdate(client, tokens);
                    return;

                case NetCodes.CL_OBJECT_DELETE:
                    ObjectDelete(client, tokens);
                    return;
            }
        }

        private void ObjectAdd(Client client, string[] tokens)
        {
            int count = 1;
            switch (tokens[count++])
            {
                case NetCodes.OBJECT_COUNTRY:
                    LocationAdd(client, tokens);
                    return;
            } 
        }

        private void ObjectUpdate(Client client, string[] tokens)
        {
            int count = 1;
            int id = Convert.ToInt32(tokens[count++]);
            switch (tokens[count++])
            {
                case NetCodes.OBJECT_COUNTRY:

                    return;
            }
        }

        private void ObjectDelete(Client client, string[] tokens)
        {
            int count = 1;
            int id = Convert.ToInt32(tokens[count++]);
            switch (tokens[count++])
            {
                case NetCodes.OBJECT_COUNTRY:
                    countryService.Delete(id);
                    return;
                case NetCodes.OBJECT_COMPANY:
                    companyService.Delete(id);
                    return;
                case NetCodes.OBJECT_PRICE:
                    priceService.Delete(id);
                    return;
                case NetCodes.OBJECT_ROUTE:
                    routeService.Delete(id);
                    return;
            }
        }

        private void DeliveryRequest(Client client, string[] tokens)
        {
            int count = 1;
            int originID = Convert.ToInt32(tokens[count++]);
            int destinationID = Convert.ToInt32(tokens[count++]);
            int weight = Convert.ToInt32(tokens[count++]);
            int volume = Convert.ToInt32(tokens[count++]);
            //Delivery air = deliveryService.Build(originID, destinationID, NetCodes.PRIORITY_AIR, weight, volume);
            //Delivery standard = deliveryService.Build(originID, destinationID, NetCodes.PRIORITY_STANDARD, weight, volume);
            //client.StorePendingDelivery(air, standard);
            //client.SendMessage(NetCodes.BuildNetworkString(NetCodes.SV_DELIVERY_PRICES,Convert.ToString(air.TotalPrice),Convert.ToString(standard.TotalPrice)));
        }

        private void DeliverySelect(Client client, string[] tokens)
        {
            int count = 1;
            // TODO Implement the timeout stuff
            Priority prio;
            if (tokens[count] == NetCodes.PRIORITY_AIR)
                prio = Priority.Air;
            else if (tokens[count] == NetCodes.PRIORITY_STANDARD)
                prio = Priority.Standard;
            //else
            //prio = null;
            //Delivery delivery = client.GetPendingDelivery(prio);
            //if (delivery != null)
            //DeliveryManager.Commit(delivery);
            //Transmit(client, NetCodes.BuildNetworkString(NetCodes.SV_DELIVERY_CONFIRM));
        }

        // Just does a full update at the moment, can do delta update later.
        private void RouteEdit(Client client, string[] tokens)
        {
            int count = 1;
            int routeID = Convert.ToInt32(tokens[count++]);
            int companyID = Convert.ToInt32(tokens[count++]);
            TransportType type;
            if (tokens[count] == NetCodes.TRANSPORT_AIR)
                type = TransportType.Air;
            else if (tokens[count] == NetCodes.TRANSPORT_SEA)
                type = TransportType.Sea;
            else
                type = TransportType.Land;   // TODO Just go with the flow and avoid errors by assuming anything else is Land? Or manually check for Land and else is a error?
            ++count;
            int originID = Convert.ToInt32(tokens[count++]);
            int destinationID = Convert.ToInt32(tokens[count++]);
            int weightCost = Convert.ToInt32(tokens[count++]);
            int volumeCost = Convert.ToInt32(tokens[count++]);

            //Route r = routeService.GetRoute(routeID);
        }

        private void PriceEdit(Client client, string[] tokens)
        {
            int count = 1;
        }

        private void LocationAdd(Client client, string[] tokens)
        {
            int count = 1;
            string code = tokens[count++];
            string name = tokens[count++];
            Country country = countryService.Create(name, code);
            Network.Instance.SendMessageToAll(country.Transmit());
        }

        private void CompanyAdd(Client client, string[] tokens)
        {
            string name = tokens[1];
            companyService.Create(name);
        }
    }
}