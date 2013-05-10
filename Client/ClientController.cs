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
        }

        #region Receiving
        public delegate void StateUpdatedDelegate(Type type);
        public event StateUpdatedDelegate Updated;

        /// <summary>
        /// Reads a message received from the Server and performs the appropriate actions.
        /// </summary>
        /// <param name="data">The message.</param>
        public void OnReceived(string data)
        {
            // TODO try catches for string arrays that are missing expected arguments. And can't be converted etc.
            var tokens = data.Split(NetCodes.SEPARATOR);

            switch (tokens[0])
            {
                case NetCodes.SV_OBJECT_UPDATE:
                    ObjectUpdate(tokens);
                    return;
                case NetCodes.SV_OBJECT_DELETE:
                    ObjectDelete(tokens);
                    return;
                case NetCodes.SV_ERROR:
                    return;

                // TODO once implemented, add the business figures stuff
            }
        }

        /// <summary>
        /// Updates the Client's state with an Add/Edit of a DataObject.
        /// </summary>
        /// <param name="tokens">Network Message.</param>
        private void ObjectUpdate(string[] tokens)
        {
            int count = 1;
            int id = Convert.ToInt32(tokens[count++]);
            switch (tokens[count++])
            {
                case NetCodes.OBJECT_COUNTRY:
                    string countryCode = tokens[count++];
                    string countryName = tokens[count++];
                    state.SaveCountry(new Country() { Name = countryName, Code = countryCode, ID = id });
                    if (Updated != null)
                        Updated(typeof(Country));
                    return;
                case NetCodes.OBJECT_COMPANY:
                    string companyName = tokens[count++];
                    state.SaveCompany(new Company() { Name = companyName, ID = id });
                    if (Updated != null)
                        Updated(typeof(Company));
                    return;
                case NetCodes.OBJECT_PRICE:
                    
                    return;
                case NetCodes.OBJECT_ROUTE:
                    return;
            }
        }

        /// <summary>
        /// Deletes the encoded DataObject from the Client's state.
        /// </summary>
        /// <param name="tokens">Network Message.</param>
        private void ObjectDelete(string[] tokens)
        {
            int count = 1;
            int id = Convert.ToInt32(tokens[count++]);
            switch (tokens[count])
            {
                case NetCodes.OBJECT_COUNTRY:
                    state.RemoveCountry(id);
                    return;
                case NetCodes.OBJECT_PRICE:
                    state.RemovePrice(id);
                    return;
                case NetCodes.OBJECT_ROUTE:
                    state.RemoveRoute(id);
                    return;
                case NetCodes.OBJECT_COMPANY:
                    state.RemoveCompany(id);
                    return;
            }
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
            Send(NetCodes.CL_OBJECT_EDIT, NetCodes.OBJECT_COUNTRY, code);
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
        /// <param name="type">Transport type.</param>
        /// <param name="weightCost">Cost per Gram.</param>
        /// <param name="volumeCost">Cost per cm^3.</param>
        /// <param name="weightMax">Maximum weight (g) per trip.</param>
        /// <param name="volumeMax">Maximum volume (cm^3) per trip.</param>
        /// <param name="duration">Trip duration.</param>
        /// <param name="times">Departing times.</param>
        public void AddRoute(int originId, int destinationId, TransportType type, int weightCost, int volumeCost, int weightMax, int volumeMax, int duration, IList<WeeklyTime> times)
        {
            Send(NetCodes.CL_OBJECT_ADD, NetCodes.OBJECT_ROUTE, Convert.ToString(originId), Convert.ToString(destinationId), type.ToNetString(), Convert.ToString(weightCost), Convert.ToString(volumeCost), Convert.ToString(weightMax), Convert.ToString(volumeMax), Convert.ToString(duration), WeeklyTime.BuildTimesNetString(times));
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
        public void EditRoute(int id, int weightCost, int volumeCost, int weightMax, int volumeMax, int duration, IList<WeeklyTime> times)
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
        /// <param name="first"></param>
        /// <param name="rest"></param>
        private void Send(string first, params string[] rest)
        {
            Network.Instance.WriteLine(NetCodes.BuildNetworkString(first, rest));
        }
        #endregion
    }
}
