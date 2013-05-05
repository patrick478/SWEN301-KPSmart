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
        Network network;
        ClientState state;

        public ClientController(Network network)
        {
            this.network = network;
        }

        #region Receiving

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
            Send(NetCodes.CL_LOCATION_ADD, code, name);
        }

        public void DeleteCountry(int id)
        {
            Send(NetCodes.CL_LOCATION_DELETE, Convert.ToString(id));
        }

        /// <summary>
        /// Alias method for sending message to server.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="rest"></param>
        private void Send(string first, params string[] rest)
        {
            network.WriteLine(NetCodes.BuildNetworkString(first, rest));
        }

        #endregion
    }
}
