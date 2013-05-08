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

        #region Receiving
        public delegate void StateUpdatedDelegate(Type type);
        public event StateUpdatedDelegate Updated;

        public ClientController(ClientState state)
        {
            this.state = state;
            Network.Instance.DataReady += new Network.DataReadyDelegate(OnReceived);
        }

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

        #region Sending Triggers (Called by GUI)
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
