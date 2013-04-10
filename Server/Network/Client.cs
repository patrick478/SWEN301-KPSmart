//////////////////////
// Original Writer: Ben Anderson.
// Reviewed by: Isabel B-N 06/04/13
//
// Edited by Isabel: added socket to constructor, made it a private field. 
// Reviewed by: Adam 07/04/13
//
// Edited by Adam: Added initial fields for Pending Delivery.
// Reviewed by: 
//////////////////////

using System;
using System.Net.Sockets;

namespace Server.Network
{
    /// <summary>
    /// A object which represnts all known information about a connected client.
    /// </summary>
    public class Client
    {
              
        // This constructor is simple because it's called by the connection manager.
        public Client(int id, Socket socket)
        {
            this.id = id;
            this.socket = socket;
        }

        // This allows for the ID to be fetched, but not set.
        private int id = -1;
        public int ID
        {
            get { return id; }
        }

        // The remote client socket.
        private Socket socket;
        public Socket Socket 
        {
            get { return socket; }
        }

        // The time they connected
        public DateTime ConnectedTime;

        // The recieve buffer.
        public byte[] Buffer = new byte[1024]; // TODO: Get the recieved_buffer_size from configs.


        // Pending Delivery:
        // When a Client requests a Delivery, the server builds Delivery objects for both Air and Standard priority and holds on to them here.
        // Once the Client picks one (also we check the state hasn't changed in a way that'll effect these since) we send the selected Delivery on into the real state.
        // TODO - Support for timeouts (because the Delivery objects have an expiry date due to the RouteInstances having real-time deadlines).

        /// <summary>
        /// Holds the Deliverys we generated for the Client's outstanding request.
        /// </summary>
        private Common.Delivery pendingAirDelivery, pendingStdDelivery;

        /// <summary>
        /// Stores the Delivery objects for the Clients pending delievery request.
        /// </summary>
        /// <param name="air">Air-Priority Delivery to store.</param>
        /// <param name="standard">Standard-Priority Delivery to store.</param>
        public void storePendingDelivery(Common.Delivery air, Common.Delivery standard)
        {
            pendingAirDelivery = air;
            pendingStdDelivery = standard;
        }

        /// <summary>
        /// Retrieves the pending Delivery corresponding to the Priority selected and then clears both fields.
        /// </summary>
        /// <param name="priority">The Priority the Client selected. null to cancel.</param>
        /// <returns>Delivery corresponding to the Priority selected.</returns>
        public Common.Delivery getPendingDelivery(Common.Priority priority)
        {
            Common.Delivery selected;
            if (priority == Common.Priority.Air)
                selected = pendingAirDelivery;
            else if (priority == Common.Priority.Standard)
                selected = pendingStdDelivery;
            else
                selected = null;

            pendingAirDelivery = null;
            pendingStdDelivery = null;

            return selected;
        }
    }
}
