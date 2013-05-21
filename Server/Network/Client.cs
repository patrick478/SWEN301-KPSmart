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
using System.Text;
using Server.Gui;

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

        public void SendMessage(string p)
        {
            Logger.WriteLine("Sending '"+p+"' to client "+ID+".");
            byte[] bytes = Encoding.ASCII.GetBytes(p);
            Network.Instance.Send(this, bytes);
        }
    }
}
