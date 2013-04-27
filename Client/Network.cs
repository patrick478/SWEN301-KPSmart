using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public enum NetworkError
    {
        NoError,
        ConnectionError,
        UnknownError
    }

    public class NetworkException : Exception
    {
        public NetworkException(string msg) : base(msg)
        {
        }
    }

    public class Network
    {
        private Socket clientSocket;

        public delegate void OnConnectDelegate();
        public delegate void DataReadyDelegate(string msg);
        public event OnConnectDelegate OnConnectComplete;
        public event DataReadyDelegate DataReady;

        public bool ErrorOccured = false;
        public NetworkError Error = NetworkError.NoError;
        public string ErrorMessage = "";

        public bool Connected = false;

        private AsyncCallback onConnectCompleteCallback;
        private AsyncCallback onSentCallback;
        private AsyncCallback onRecieveCallback;

        private byte[] buffer;

        public Network()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            onConnectCompleteCallback = new AsyncCallback(onConnectComplete);
            onSentCallback = new AsyncCallback(onSent);
            onRecieveCallback = new AsyncCallback(onRecieve);

            buffer = new byte[1024];
        }

        private void onConnectComplete(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndConnect(ar);
                this.Connected = true;
            }
            catch (SocketException se)
            {
                ErrorOccured = true;
                Error = NetworkError.ConnectionError;
                ErrorMessage = se.Message;
                return;
            }
            catch (Exception e)
            {
                ErrorOccured = true;
                Error = NetworkError.UnknownError;
                ErrorMessage = e.Message;
                return;
            }

            clientSocket.BeginReceive(buffer, 0, 1024, SocketFlags.None, onRecieveCallback, null);


            OnConnectComplete();
        }

        private void onSent(IAsyncResult ar)
        {
            int tx = clientSocket.EndSend(ar);
        }

        private void onRecieve(IAsyncResult ar)
        {
            int rx = -1;
            try
            {
                rx = clientSocket.EndReceive(ar);
            }
            catch (SocketException se)
            {
                if (se.ErrorCode == 10054)
                {
                    // Shit - server went away!
                    return;
                }
            }

            byte[] recieved = new byte[rx];
            Array.Copy(buffer, recieved, rx);

            string message = Encoding.ASCII.GetString(recieved);

            clientSocket.BeginReceive(buffer, 0, 1024, SocketFlags.None, onRecieveCallback, null);

            if(DataReady != null)
                DataReady(message);
        }

        public void BeginConnect(string address, int port)
        {
            clientSocket.BeginConnect(address, port, onConnectCompleteCallback, null);
        }

        public void WriteLine(string line)
        {
            byte[] data = Encoding.ASCII.GetBytes(line);
            clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, onSentCallback, null);
        }
    }
}
