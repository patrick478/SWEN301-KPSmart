//////////////////////
// Original Writer: Ben Anderson.
// Reviewed by:
//
// **Example for when someone makes a significant change**
// Changed By: Ben Anderson
// Reviewed by: Bob Smith.
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
    /// This is the main networking class used by the server. All actual networking should take place in here.
    /// It's thread-safe which will make using it with WinForms much easier.
    /// </summary>
    public sealed class Network
    {
        // The singleton instance
        private static volatile Network instance;
        // Locking object for the singleton. Thread safety!
        private static object syncRoot = new Object();

        // Empty constructor is empty for a reason. All 'settings' for this, should be loaded using the config
        private Network() { }

        // The variable to fetches the instance, it's all magical. Uses C# Get. 
        public static Network Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Network();
                    }
                }


                return instance;
            }
        }

        // The actual networking class begins here.
        private Socket listenSocket = null;

        // These are the callback objects used for the asynchronous listener.
        private AsyncCallback acceptCallback;
        private AsyncCallback recieveCallback;

        /// <summary>
        /// Starts the networking system, and prepares to begin listening but doesn't actually
        /// start accepting connections.
        /// </summary>
        public void Start()
        {
            // Port to be used as the listening port
            int port = 23333; // This should be fetched from config.

            // Listening address. 
            IPAddress listenAddress = IPAddress.Any; // Should be fetched from config.

            // Setup the callbacks
            acceptCallback = new AsyncCallback(Network.Instance.onAccept);
            recieveCallback = new AsyncCallback(Network.Instance.onRecieve);

            this.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                this.listenSocket.Bind(new IPEndPoint(listenAddress, port));
            }
            catch (SocketException se)
            {
                // TODO: Check if se.ErrorCode == 10054 (I think), and then fail the bind because socket already in use.
                // This basically occurs when we try to bind to an in-use port.
                Logger.WriteLine(se.ToString());
            }
            catch (Exception e)
            {
                // This is a pretty unlikely occurence.
                Logger.WriteLine(e.ToString());
                throw e;
            }

            Logger.WriteLine("Server ready to begin listening");
        }

        /// <summary>
        /// Starts the Networking system listening for connections and recieving data from those connections
        /// </summary>
        public void Open()
        {
            this.listenSocket.Listen(100);
            this.listenSocket.BeginAccept(acceptCallback, null);
            Logger.WriteLine("Server listening for connections");
        }

        // This function is called to accept a socket connection request on the listen socket.
        public void onAccept(IAsyncResult ar)
        {
            // TODO: Error catching!
            Socket clientSocket = this.listenSocket.EndAccept(ar);
            this.listenSocket.BeginAccept(acceptCallback, null);

            int id = ConnectionManager.Add();
            Client client = ConnectionManager.Get(id);
            client.Socket = clientSocket;
            client.ConnectedTime = DateTime.Now;

            client.Socket.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, recieveCallback, id);

            IPEndPoint remoteEndPoint = client.Socket.RemoteEndPoint as IPEndPoint;
            Logger.WriteLine("Client connected from {0}:{1}", remoteEndPoint.Address, remoteEndPoint.Port);
        }

        // This function is executed when a client socket recieves data.
        public void onRecieve(IAsyncResult ar)
        {
            int id = (int)ar.AsyncState;
            Client client = ConnectionManager.Get(id);
            int rx = client.Socket.EndReceive(ar);
            byte[] recieved = client.Buffer;
            Array.Clear(client.Buffer, 0, client.Buffer.Length);
            client.Socket.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, recieveCallback, id);

            Logger.WriteLine("Recieved {0} from {1}", rx, id);
        }
    }
}
