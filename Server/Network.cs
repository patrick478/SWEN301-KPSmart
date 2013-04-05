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
            acceptCallback = new AsyncCallback(this.onAccept);
            recieveCallback = new AsyncCallback(this.onRecieve);

            // This actually creates the socket object and prepares it to be bound to a port
            this.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // The bind. Binding the process of binding a socket to a specific port, and is essential
                // for listening for servers on windows.
                // It basically notifies the OS that this socket is an active socket on this port.
                // Note, that just because a socket is bound, doesn't mean it's listening for connections.
                this.listenSocket.Bind(new IPEndPoint(listenAddress, port));
            }
            catch (SocketException se)
            {
                // TODO: Check if se.ErrorCode == 10054 (I think), and then fail the bind because socket already in use.
                // This basically occurs when we try to bind to an in-use port.
                // UPDATE: It's not 10054, 10054 is disconnect during recieve. I'll have to look into it.
                // UPDATE2: The number appears to be 10048. http://msdn.microsoft.com/en-us/library/windows/desktop/ms740668(v=vs.85).aspx
                // UPDATE3: We'll probably also need to catch 10049: Socket address not available.
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
            // This actually starts the socket listening for incoming connections
            this.listenSocket.Listen(100);

            // Because we're using an asynchronous socket, the OS will handle a lot of the work of actually accepting
            // for us, using the OS level eventing. This means we just say what we want to happen on an accept-event and the
            // OS / C# will execute that for us.
            // This line starts an accept request, which will run "acceptCallback" when an accept is ready to happen!
            this.listenSocket.BeginAccept(acceptCallback, null);

            Logger.WriteLine("Server listening for connections");
        }

        // This function is called to accept a socket connection request on the listen socket.
        private void onAccept(IAsyncResult ar)
        {
            // TODO: Error catching!
            // This line ends the accept request. It returns the socket object referencing
            // the client that has just finished the connection state.
            Socket clientSocket = this.listenSocket.EndAccept(ar);

            // After accepting a client, the listener stops looking to accept new clients, so we put 
            // it back into the state by making another call to BeginAccept.
            this.listenSocket.BeginAccept(acceptCallback, null);

            // Allocates a new Client in the ConnectionManager.
            int id = ConnectionManager.Add();

            // Fetch the newly created client.
            Client client = ConnectionManager.Get(id);

            // Set the client socket, so that other code my use it.
            client.Socket = clientSocket;

            // Records the time the client connected
            client.ConnectedTime = DateTime.Now;

            // Start listening for data on the newly acquired client socket. This allows us to 
            // start recieving immediatly.
            // Arguments of note are the last one, 'id'. I'm setting the AsyncState variable to the ID, so that
            // when the callback is executed, I know which Client object this particular socket is referencing.
            client.Socket.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, recieveCallback, id);

            // Prints out remote client stats
            IPEndPoint remoteEndPoint = client.Socket.RemoteEndPoint as IPEndPoint;
            Logger.WriteLine("Client {0} connected from {1}:{2}", id, remoteEndPoint.Address, remoteEndPoint.Port);
        }

        // This function is executed when a client socket recieves data.
        private void onRecieve(IAsyncResult ar)
        {
            // I previously set the AsyncState to the ID of the client, so get that back...
            int id = (int)ar.AsyncState;

            // ...and fetch the client object
            Client client = ConnectionManager.Get(id);

            // Now we've actually got the socket, we can end the recieve.
            // 'rx' will now contain the number of bytes recieved.
            int rx = client.Socket.EndReceive(ar);

            // Clone the client buffer
            byte[] recieved = client.Buffer;

            // Clear the client buffer to prevent overwrites causing confusion
            Array.Clear(client.Buffer, 0, client.Buffer.Length);
            
            // And mark the socket as recieving so as to get more data.
            client.Socket.BeginReceive(client.Buffer, 0, client.Buffer.Length, SocketFlags.None, recieveCallback, id);

            Logger.WriteLine("Recieved {0} from {1}", rx, id);
        }
    }
}
