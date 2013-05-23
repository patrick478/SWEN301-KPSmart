using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Client
{
    /// <summary>
    /// Another class name to refactor :(
    /// </summary>
    public class ClientController
    {
        ClientState state;

        public ClientController(ClientState state)
        {
            this.state = state;
            Network.Instance.DataReady += new Network.DataReadyDelegate(OnReceived);
            Network.Instance.LoginComplete += new Network.LoginCompleteDelegate(OnLogin);
        }

        #region Receiving
        public delegate void StateUpdatedDelegate(string type);
        public event StateUpdatedDelegate Updated;

        public delegate void DeliveryOptionsDelegate(IDictionary<PathType, int> prices);
        public event DeliveryOptionsDelegate OptionsReceived;

        public delegate void DeliveryConfirmedDelegate();
        public event DeliveryConfirmedDelegate DeliveryOK;

        public delegate void StatisticsReceivedDelegate(Statistics stats);
        public event StatisticsReceivedDelegate StatsReceived;

        public delegate void ErrorMessageDelegate(string error);
        public event ErrorMessageDelegate Error;

        /// <summary>
        /// Reads a message received from the Server and performs the appropriate actions.
        /// </summary>
        /// <param name="data">The message.</param>
        public void OnReceived(string data)
        {
            var tokens = data.Split(NetCodes.SEPARATOR);

            switch (tokens[0])
            {
                case NetCodes.SV_OBJECT_UPDATE:
                    ObjectUpdate(tokens, true);
                    return;
                case NetCodes.SV_OBJECT_DELETE:
                    ObjectDelete(tokens, true);
                    return;
                case NetCodes.SV_DELIVERY_PRICES:
                    DeliveryOptions(tokens);
                    return;
                case NetCodes.SV_DELIVERY_CONFIRMED:
                    DeliveryConfirmed(tokens);
                    return;
                case NetCodes.SV_ERROR:
                    ErrorMessage(tokens);
                    return;
                case NetCodes.SV_STATS_ANSWER:
                    StatsAnswer(tokens);
                    return;
                case NetCodes.SV_SYNC_UPDATE:
                    ObjectUpdate(tokens, false);;
                    return;
                case NetCodes.SV_SYNC_DONE:
                    Updated(NetCodes.OBJECT_ALL);
                    return;
            }
        }

        /// <summary>
        /// Actions to perform on a Login.
        /// </summary>
        /// <param name="success">If the login succeeded or not.</param>
        private void OnLogin(bool success)
        {
            if (success)
                Send(NetCodes.CL_SYNC_STATE, state.GetUpdateTime().ToString());
        }

        /// <summary>
        /// Updates the Client's state with an Add/Edit of a DataObject.
        /// </summary>
        /// <param name="tokens">Network Message.</param>
        private void ObjectUpdate(string[] tokens, bool notify)
        {
            
            int count = 1;
            DateTime timestamp = DateTime.Parse(tokens[count++]);
            string objectType = tokens[count++];
            switch (objectType)
            {
                case NetCodes.OBJECT_COUNTRY:
                    state.SaveCountry(Country.ParseNetString(tokens[count]));
                    break;
                case NetCodes.OBJECT_COMPANY:
                    state.SaveCompany(Company.ParseNetString(tokens[count]));
                    break;
                case NetCodes.OBJECT_PRICE:
                    state.SavePrice(Price.ParseNetString(tokens[count], state));
                    break;
                case NetCodes.OBJECT_ROUTE:
                    state.SaveRoute(Route.ParseNetString(tokens[count], state));
                    break;
                case NetCodes.OBJECT_ROUTENODE:
                    state.SaveRouteNode(RouteNode.ParseNetString(tokens[count], state));
                    break;
                default:
                    return;
            }
            if (notify && Updated != null)
                Updated(objectType);
            state.SetUpdateTime(timestamp);
        }

        /// <summary>
        /// Deletes the encoded DataObject from the Client's state.
        /// </summary>
        /// <param name="tokens">Network Message.</param>
        private void ObjectDelete(string[] tokens, bool notify)
        {
            int count = 1;
            DateTime timestamp = DateTime.Parse(tokens[count++]);
            int id = Convert.ToInt32(tokens[count+1]);  // Note the different order here.
            string objectType = tokens[count];
            switch (objectType)
            {
                case NetCodes.OBJECT_COUNTRY:
                    state.RemoveCountry(id);
                    break;
                case NetCodes.OBJECT_PRICE:
                    state.RemovePrice(id);
                    break;
                case NetCodes.OBJECT_ROUTE:
                    state.RemoveRoute(id);
                    break;
                case NetCodes.OBJECT_COMPANY:
                    state.RemoveCompany(id);
                    break;
                case NetCodes.OBJECT_ROUTENODE:
                    state.RemoveRouteNode(id);
                    break;
                default:
                    return;
            }
            if (notify && Updated != null)
                Updated(objectType);
            state.SetUpdateTime(timestamp);
        }

        /// <summary>
        /// Performs the actions for receiving a Delivery Options network message from the Server.
        /// </summary>
        /// <param name="tokens">Network Message.</param>
        private void DeliveryOptions(string[] tokens)
        {
            int count = 1;
            if (OptionsReceived != null)
            {
                if (tokens[count] == NetCodes.PATH_CANCEL)
                    OptionsReceived(null);
                else
                {
                    IDictionary<PathType, int> options = PathTypeExtensions.ParseOptionsNetString(tokens[count]);
                    OptionsReceived(options);
                }
            }

        }

        /// <summary>
        /// Performs the actions for receiving a Delivery Confirmation network message from the Server.
        /// </summary>
        /// <param name="tokens">Network Message.</param>
        private void DeliveryConfirmed(string[] tokens)
        {
            if (DeliveryOK != null)
                DeliveryOK();
        }

        private void StatsAnswer(string[] tokens)
        {
            Statistics stats = Statistics.ParseNetString(tokens, 1, state);
            if (StatsReceived != null)
                StatsReceived(stats);
        }

        private void ErrorMessage(string[] tokens)
        {
            string message = tokens[1];
            if (Error != null)
                Error(message);
        }
        #endregion

        #region Sending (Triggers Called by GUI)

        #region Delivery
        /// <summary>
        /// Requests the server to provide options for a new Delivery to be made.
        /// </summary>
        /// <param name="originId">ID of Origin Location.</param>
        /// <param name="destinationId">ID of Destination Location.</param>
        /// <param name="weight">Weight (in grams) of the package.</param>
        /// <param name="volume">Volume (in cm^3) of the package.</param>
        public void RequestDelivery(int originId, int destinationId, int weight, int volume)
        {
            Send(NetCodes.CL_DELIVERY_REQUEST, Convert.ToString(originId), Convert.ToString(destinationId), Convert.ToString(weight), Convert.ToString(volume));
        }

        /// <summary>
        /// Selects a Delivery option from the provided Types of the outstanding request.
        /// </summary>
        /// <param name="type">PathType to use.</param>
        public void ChooseDelivery(PathType type)
        {
            Send(NetCodes.CL_DELIVERY_SELECT, type.ToNetString());
        }

        /// <summary>
        /// Cancels an outstanding Delivery Request.
        /// </summary>
        public void CancelDelivery()
        {
            Send(NetCodes.CL_DELIVERY_SELECT, NetCodes.PATH_CANCEL);
        }
        #endregion

        #region Country
        /// <summary>
        /// Add a new Country.
        /// </summary>
        /// <param name="code">3-character country code.</param>
        /// <param name="name">Country name.</param>
        public void AddCountry(string code, string name)
        {
            Send(NetCodes.CL_OBJECT_ADD, NetCodes.OBJECT_COUNTRY, code, name);
        }

        /// <summary>
        /// Edit the code of an existing Country.
        /// </summary>
        /// <param name="id">ID of Country to edit.</param>
        /// <param name="code">New 3-character country code to use.</param>
        public void EditCountry(int id, string code)
        {
            Send(NetCodes.CL_OBJECT_EDIT, Convert.ToString(id), NetCodes.OBJECT_COUNTRY, code);
        }

        /// <summary>
        /// Delete an existing Country.
        /// </summary>
        /// <param name="id">ID of Country to delete.</param>
        public void DeleteCountry(int id)
        {
            Send(NetCodes.CL_OBJECT_DELETE, Convert.ToString(id), NetCodes.OBJECT_COUNTRY);
        }
        #endregion

        #region Company
        /// <summary>
        /// Add a new Company.
        /// </summary>
        /// <param name="name">Company name.</param>
        public void AddCompany(string name)
        {
            Send(NetCodes.CL_OBJECT_ADD, NetCodes.OBJECT_COMPANY, name);
        }

        /// <summary>
        /// Delete an existing Company.
        /// </summary>
        /// <param name="id">ID of Company to delete.</param>
        public void DeleteCompany(int id)
        {
            Send(NetCodes.CL_OBJECT_DELETE, Convert.ToString(id), NetCodes.OBJECT_COMPANY);
        }
        #endregion

        #region RouteNodes
        /// <summary>
        /// Add a new Distribution Centre.
        /// </summary>
        /// <param name="name">Name of the Distribution Centre.</param>
        public void AddDistributionCentre(string name)
        {
            Send(NetCodes.CL_OBJECT_ADD, NetCodes.OBJECT_ROUTENODE, NetCodes.NODE_DISTRIBUTION, name);
        }

        /// <summary>
        /// Add a new International Port.
        /// </summary>
        /// <param name="countryId">ID of Country this port is for.</param>
        public void AddInternationalPort(int countryId)
        {
            Send(NetCodes.CL_OBJECT_ADD, NetCodes.OBJECT_ROUTENODE, NetCodes.NODE_INTERNATIONAL, Convert.ToString(countryId));
        }

        /// <summary>
        /// Delete an existing RouteNode.
        /// </summary>
        /// <param name="id">ID of RouteNode to delete.</param>
        public void DeleteRouteNode(int id)
        {
            Send(NetCodes.CL_OBJECT_DELETE, Convert.ToString(id), NetCodes.OBJECT_ROUTENODE);
        }
        #endregion

        #region Price
        /// <summary>
        /// Add a new Customer Price.
        /// </summary>
        /// <param name="originId">ID of Origin Location.</param>
        /// <param name="destinationId">ID of Destination Location.</param>
        /// <param name="priority">Priority this Price applies to.</param>
        /// <param name="weightPrice">Cost per Gram.</param>
        /// <param name="volumePrice">Cost per cm^3.</param>
        public void AddPrice(int originId, int destinationId, Priority priority, int weightPrice, int volumePrice)
        {
            Send(NetCodes.CL_OBJECT_ADD, NetCodes.OBJECT_PRICE, Convert.ToString(originId), Convert.ToString(destinationId), priority.ToNetString(), Convert.ToString(weightPrice), Convert.ToString(volumePrice));
        }

        /// <summary>
        /// Edit an existing Customer Price.
        /// </summary>
        /// <param name="id">ID of Price to edit.</param>
        /// <param name="weightPrice">New Cost per Gram.</param>
        /// <param name="volumePrice">New Cost per cm^3.</param>
        public void EditPrice(int id, int weightPrice, int volumePrice)
        {
            Send(NetCodes.CL_OBJECT_EDIT, Convert.ToString(id), NetCodes.OBJECT_PRICE, Convert.ToString(weightPrice), Convert.ToString(volumePrice));
        }

        /// <summary>
        /// Delete an existing Customer Price.
        /// </summary>
        /// <param name="id">ID of Price to delete.</param>
        public void DeletePrice(int id)
        {
            Send(NetCodes.CL_OBJECT_DELETE, Convert.ToString(id), NetCodes.OBJECT_PRICE);
        }
        #endregion

        #region Route
        /// <summary>
        /// Add a new Route.
        /// </summary>
        /// <param name="originId">ID of Origin Location.</param>
        /// <param name="destinationId">ID of Destination Location.</param>
        /// <param name="companyId">ID of the Company providing this route.</param>
        /// <param name="type">Transport type.</param>
        /// <param name="weightCost">Cost per Gram.</param>
        /// <param name="volumeCost">Cost per cm^3.</param>
        /// <param name="weightMax">Maximum weight (g) per trip.</param>
        /// <param name="volumeMax">Maximum volume (cm^3) per trip.</param>
        /// <param name="duration">Trip duration.</param>
        /// <param name="times">Departing times.</param>
        public void AddRoute(int originId, int destinationId, int companyId, TransportType type, int weightCost, int volumeCost, int weightMax, int volumeMax, int duration, List<WeeklyTime> times)
        {
            Send(NetCodes.CL_OBJECT_ADD, NetCodes.OBJECT_ROUTE, Convert.ToString(originId), Convert.ToString(destinationId), Convert.ToString(companyId), type.ToNetString(), Convert.ToString(weightCost), Convert.ToString(volumeCost), Convert.ToString(weightMax), Convert.ToString(volumeMax), Convert.ToString(duration), WeeklyTime.BuildTimesNetString(times));
        }

        /// <summary>
        /// Edit an existing Route.
        /// </summary>
        /// <param name="id">ID of Route to edit.</param>
        /// <param name="weightCost">Cost per Gram.</param>
        /// <param name="volumeCost">Cost per cm^3.</param>
        /// <param name="weightMax">Maximum weight (g) per trip.</param>
        /// <param name="volumeMax">Maximum volume (cm^3) per trip.</param>
        /// <param name="duration">Trip duration.</param>
        /// <param name="times">Departing times.</param>
        public void EditRoute(int id, int weightCost, int volumeCost, int weightMax, int volumeMax, int duration, List<WeeklyTime> times)
        {
            Send(NetCodes.CL_OBJECT_EDIT, Convert.ToString(id), NetCodes.OBJECT_ROUTE, Convert.ToString(weightCost), Convert.ToString(volumeCost), Convert.ToString(weightMax), Convert.ToString(volumeMax), Convert.ToString(duration), WeeklyTime.BuildTimesNetString(times));
        }

        /// <summary>
        /// Delete an existing Route.
        /// </summary>
        /// <param name="id">ID of Route to delete.</param>
        public void DeleteRoute(int id)
        {
            Send(NetCodes.CL_OBJECT_DELETE, Convert.ToString(id), NetCodes.OBJECT_ROUTE);
        }
        #endregion

        /// <summary>
        /// Alias method for sending message to server.
        /// </summary>
        private void Send(string first, params string[] rest)
        {
            Network.Instance.WriteLine(NetCodes.BuildNetworkString(first, rest));
        }
        #endregion
    }
}
