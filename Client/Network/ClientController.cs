using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Client.Network
{
    /// <summary>
    /// Another class name to refactor :(
    /// </summary>
    public class ClientController
    {
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
                case NetCodes.SV_CONFIRM:
                    //stuff
                    return;
                case NetCodes.CL_DELIVERY_SELECT:
                    DeliverySelect(client, tokens);
                    return;
            }
        }
    }
}
