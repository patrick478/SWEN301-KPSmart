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

            // TODO NETWORK EVENT SUBSCRIPTIONS HERE
            Network.Instance.MessageReceived += new Network.MessageReceivedDelegate(OnReceived);
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
                case NetCodes.CL_OBJECT_EDIT:
                    ObjectEdit(client, tokens);
                    return;
                case NetCodes.CL_OBJECT_DELETE:
                    ObjectDelete(client, tokens);
                    return;

                case NetCodes.CL_SYNC_STATE:
                    SyncState(client, tokens);
                    return;

                // TODO once implemented, add the business figures stuff
            }
        }

        private void ObjectAdd(Client client, string[] tokens)
        {
            int count = 1;
            switch (tokens[count++])
            {
                case NetCodes.OBJECT_COUNTRY:
                    string countryCode = tokens[count++];
                    string countryName = tokens[count++];
                    Country country = countryService.Create(countryName, countryCode);
                    SendObjectUpdate(NetCodes.OBJECT_COUNTRY, country.ToNetString());
                    return;
                case NetCodes.OBJECT_COMPANY:
                    string companyName = tokens[count++];
                    Company company = companyService.Create(companyName);
                    SendObjectUpdate(NetCodes.OBJECT_COMPANY, company.ToNetString());
                    return;
                case NetCodes.OBJECT_PRICE:
                    int priceOriginId = Convert.ToInt32(tokens[count++]);
                    int priceDestinationId = Convert.ToInt32(tokens[count++]);
                    Priority pricePrio = PriorityExtensions.ParseNetString(tokens[count++]);
                    int priceWeight = Convert.ToInt32(tokens[count++]);
                    int priceVolume = Convert.ToInt32(tokens[count++]);
                    Price price = priceService.Create(priceOriginId, priceDestinationId, pricePrio, priceWeight, priceVolume);
                    SendObjectUpdate(NetCodes.OBJECT_PRICE, price.ToNetString());
                    return;
                case NetCodes.OBJECT_ROUTE:
                    int routeOriginId = Convert.ToInt32(tokens[count++]);
                    int routeDestinationId = Convert.ToInt32(tokens[count++]);
                    int routeCompany = Convert.ToInt32(tokens[count++]);
                    TransportType routeTransport = TransportTypeExtensions.ParseNetString(tokens[count++]);
                    int routeWeightCost = Convert.ToInt32(tokens[count++]);
                    int routeVolumeCost = Convert.ToInt32(tokens[count++]);
                    int routeWeightMax = Convert.ToInt32(tokens[count++]);
                    int routeVolumeMax = Convert.ToInt32(tokens[count++]);
                    int routeDuration = Convert.ToInt32(tokens[count++]);
                    List<WeeklyTime> routeTimes = WeeklyTime.ParseTimesNetString(tokens[count++]);
                    Route route = routeService.Create(routeTransport, routeCompany, routeOriginId, routeDestinationId, routeTimes, routeDuration, routeWeightMax, routeVolumeMax, routeWeightCost, routeVolumeCost);
                    SendObjectUpdate(NetCodes.OBJECT_ROUTE, route.ToNetString());
                    return;
            } 
        }

        private void ObjectEdit(Client client, string[] tokens)
        {
            int count = 1;
            int id = Convert.ToInt32(tokens[count++]);
            switch (tokens[count++])
            {
                case NetCodes.OBJECT_COUNTRY:
                    string countryCode = tokens[count++];
                    Country country = countryService.Update(id, countryCode);
                    SendObjectUpdate(NetCodes.OBJECT_COUNTRY, country.ToNetString());
                    return;
                case NetCodes.OBJECT_COMPANY:
                    // send back error saying you can't edit companies. maybe?
                    return;
                case NetCodes.OBJECT_PRICE:
                    int priceWeight = Convert.ToInt32(tokens[count++]);
                    int priceVolume = Convert.ToInt32(tokens[count++]);
                    Price price = priceService.Update(id, priceWeight, priceVolume);
                    SendObjectUpdate(NetCodes.OBJECT_PRICE, price.ToNetString());
                    return;
                case NetCodes.OBJECT_ROUTE:
                    int routeWeightCost = Convert.ToInt32(tokens[count++]);
                    int routeVolumeCost = Convert.ToInt32(tokens[count++]);
                    int routeWeightMax = Convert.ToInt32(tokens[count++]);
                    int routeVolumeMax = Convert.ToInt32(tokens[count++]);
                    int routeDuration = Convert.ToInt32(tokens[count++]);
                    List<WeeklyTime> routeTimes = WeeklyTime.ParseTimesNetString(tokens[count++]);
                    Route route = routeService.Update(id, routeTimes, routeDuration, routeWeightMax, routeVolumeMax, routeWeightCost, routeVolumeCost);
                    SendObjectUpdate(NetCodes.OBJECT_ROUTE, route.ToNetString());
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
                    SendObjectDelete(NetCodes.OBJECT_COUNTRY, id);
                    return;
                case NetCodes.OBJECT_COMPANY:
                    companyService.Delete(id);
                    SendObjectDelete(NetCodes.OBJECT_COMPANY, id);
                    return;
                case NetCodes.OBJECT_PRICE:
                    priceService.Delete(id);
                    SendObjectDelete(NetCodes.OBJECT_PRICE, id);
                    return;
                case NetCodes.OBJECT_ROUTE:
                    routeService.Delete(id);
                    SendObjectDelete(NetCodes.OBJECT_ROUTE, id);
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
            IDictionary<PathType,Delivery> options = deliveryService.GetBestRoutes(client.ID, originID, destinationID, weight, volume);
            if (options.Count <= 0)
                client.SendMessage(NetCodes.BuildNetworkString(NetCodes.SV_DELIVERY_PRICES,NetCodes.PATH_CANCEL));
            else
                client.SendMessage(NetCodes.BuildNetworkString(NetCodes.SV_DELIVERY_PRICES,PathTypeExtensions.BuildOptionsNetString(options)));
        }

        private void DeliverySelect(Client client, string[] tokens)
        {
            int count = 1;
            // TODO Implement the timeout stuff
            if (tokens[count] == NetCodes.PATH_CANCEL)
            {
                // client cancelled request TODO
                return;
            }
            PathType type = PathTypeExtensions.ParseNetString(tokens[count]);
            deliveryService.SelectDeliveryOption(client.ID, type);
            client.SendMessage(NetCodes.SV_DELIVERY_CONFIRMED);
        }

        private void SyncState(Client client, string[] tokens)
        {
            DateTime clientTime = DateTime.Parse(tokens[1]);
            //TODO
            foreach (Company c in companyService.GetAll())
                SendUpdateForSync(client, NetCodes.OBJECT_COMPANY, c.ToNetString());
            foreach (Country l in countryService.GetAll())
                SendUpdateForSync(client, NetCodes.OBJECT_COUNTRY, l.ToNetString());
            foreach (Price p in priceService.GetAll())
                SendUpdateForSync(client, NetCodes.OBJECT_PRICE, p.ToNetString());
            foreach (Route r in routeService.GetAll())
                SendUpdateForSync(client, NetCodes.OBJECT_ROUTE, r.ToNetString());
        }

        private void SendUpdateForSync(Client client, string objectType, string objectDef)
        {
            client.SendMessage(NetCodes.BuildNetworkString(NetCodes.SV_OBJECT_UPDATE, DateTime.UtcNow.ToString(), objectType, objectDef));
        }

        private void SendObjectUpdate(string objectType, string objectDef)
        {
            Network.Instance.SendMessageToAll(NetCodes.BuildNetworkString(NetCodes.SV_OBJECT_UPDATE, DateTime.UtcNow.ToString(), objectType, objectDef));
        }

        private void SendObjectDelete(string objectType, int id)
        {
            Network.Instance.SendMessageToAll(NetCodes.BuildNetworkString(NetCodes.SV_OBJECT_DELETE, DateTime.UtcNow.ToString(), objectType, Convert.ToString(id)));
        }
    }
}