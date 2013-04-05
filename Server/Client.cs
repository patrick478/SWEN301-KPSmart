//////////////////////
// Original Writer: Ben Anderson.
// Reviewed by:
//////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    /// <summary>
    /// A object which represnts all known information about a connected client.
    /// </summary>
    public class Client
    {
        // This constructor is simple because it's called by the connection manager.
        public Client(int id)
        {
            this.id = id;
        }

        // This allows for the ID to be fetched, but not set.
        private int id = -1;
        public int ID
        {
            get { return id; }
        }

        // The remote client socket.
        public Socket Socket;
        // The time they connected
        public DateTime ConnectedTime;

        // The recieve buffer.
        public byte[] Buffer = new byte[1024]; // TODO: Get the recieved_buffer_size from configs.
    }
}
