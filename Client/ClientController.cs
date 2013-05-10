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
        public void RequestDelivery(int originId, int destinationId, int weight, int volume)
        {
            Send(NetCodes.CL_DELIVERY_REQUEST, Convert.ToString(originId), Convert.ToString(destinationId), Convert.ToString(weight), Convert.ToString(volume));
        }

        public void ChooseDelivery(PathType type)
        {
            Send(NetCodes.CL_DELIVERY_SELECT, type.ToNetString());
        }

        public void CancelDelivery()
        {
            Send(NetCodes.CL_DELIVERY_SELECT, NetCodes.PATH_CANCEL);
        }
        #endregion

        #region Country
        public void AddCountry(string code, string name)
        {
            Send(NetCodes.CL_OBJECT_ADD, NetCodes.OBJECT_COUNTRY, code, name);
        }

        public void EditCountry(int id, string code)
        {
            Send(NetCodes.CL_OBJECT_EDIT, NetCodes.OBJECT_COUNTRY, code);
        }

        public void DeleteCountry(int id)
        {
            Send(NetCodes.CL_OBJECT_DELETE, Convert.ToString(id), NetCodes.OBJECT_COUNTRY);
        }
        #endregion

        #region Company
        public void AddCompany(string name)
        {
            Send(NetCodes.CL_OBJECT_ADD, NetCodes.OBJECT_COMPANY, name);
        }

        public void DeleteCompany(int id)
        {
            Send(NetCodes.CL_OBJECT_DELETE, Convert.ToString(id), NetCodes.OBJECT_COMPANY);
        }
        #endregion

        #region Price
        public void AddPrice(int originId, int destinationId, Priority priority, int weightPrice, int volumePrice)
        {
            Send(NetCodes.CL_OBJECT_ADD, NetCodes.OBJECT_PRICE, Convert.ToString(originId), Convert.ToString(destinationId), priority.ToNetString(), Convert.ToString(weightPrice), Convert.ToString(volumePrice));
        }

        public void EditPrice(int id, int weightPrice, int volumePrice)
        {
            Send(NetCodes.CL_OBJECT_EDIT, Convert.ToString(id), NetCodes.OBJECT_PRICE, Convert.ToString(weightPrice), Convert.ToString(volumePrice));
        }

        public void DeletePrice(int id)
        {
            Send(NetCodes.CL_OBJECT_DELETE, Convert.ToString(id), NetCodes.OBJECT_PRICE);
        }
        #endregion

        #region Route
        // TODO - INCOMPLETE need to add weekly times
        public void AddRoute(int originId, int destinationId, TransportType type, int weightCost, int volumeCost, int weightMax, int volumeMax, int duration)
        {
            Send(NetCodes.CL_OBJECT_ADD, NetCodes.OBJECT_ROUTE, Convert.ToString(originId), Convert.ToString(destinationId), type.ToNetString(), Convert.ToString(weightCost), Convert.ToString(volumeCost), Convert.ToString(weightMax), Convert.ToString(volumeMax), Convert.ToString(duration), );
        }

        // TODO - INCOMPLETE need to add weekly times
        public void EditRoute(int id, int weightCost, int volumeCost, int weightMax, int volumeMax, int duration)
        {
            Send(NetCodes.CL_OBJECT_EDIT, Convert.ToString(id), NetCodes.OBJECT_ROUTE, Convert.ToString(weightCost), Convert.ToString(volumeCost), Convert.ToString(weightMax), Convert.ToString(volumeMax), Convert.ToString(duration), );
            // *Cost/gram (int) - *Cost/cm3 (int) - *Max Weight (int) - *Max Capacity (int) - *Trip Duration (int) - *Trip Times 
        }

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
