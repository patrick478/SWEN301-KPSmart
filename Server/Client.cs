using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Client
    {
        public Client(int id)
        {
            this.id = id;
        }

        private int id = -1;
        public int ID
        {
            get { return id; }
        }

        public Socket Socket;
        public DateTime ConnectedTime;

        public byte[] Buffer = new byte[1024]; // TODO: Get the recieved_buffer_size from configs.
    }
}
