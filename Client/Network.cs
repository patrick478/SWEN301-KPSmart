using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace Client
{
    public enum NetworkError
    {
        NoError,
        ConnectionError,
        TransmissionError,
        Disconnect,
        UnknownError
    }

    public class NetworkException : Exception
    {
        public NetworkException(string msg)
            : base(msg)
        {
        }
    }

    public class Network
    {
        // The singleton instance
        private static volatile Network instance;
        // Locking object for the singleton. Thread safety!
        private static object syncRoot = new Object();

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

        private Socket clientSocket;

        public delegate void OnConnectDelegate();
        public delegate void DataReadyDelegate(string msg);
        public delegate void NetworkErrorDelegate();
        public delegate void LoginCompleteDelegate(bool success);

        public event OnConnectDelegate OnConnectComplete;
        public event DataReadyDelegate DataReady;
        public event NetworkErrorDelegate NetworkErrorOccured;
        public event LoginCompleteDelegate LoginComplete;

        public bool ErrorOccured = false;
        public NetworkError Error = NetworkError.NoError;
        public string ErrorMessage = "";

        public bool Connected = false;
        public bool Usable = false;

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
                if (NetworkErrorOccured != null)
                    NetworkErrorOccured();

                return;
            }
            catch (Exception e)
            {
                ErrorOccured = true;
                Error = NetworkError.UnknownError;
                ErrorMessage = e.Message;
                if (NetworkErrorOccured != null)
                    NetworkErrorOccured();

                return;
            }

            clientSocket.BeginReceive(buffer, 0, 1024, SocketFlags.None, onRecieveCallback, null);


            if (OnConnectComplete != null)
                OnConnectComplete();
        }

        private void onSent(IAsyncResult ar)
        {
            int tx = -1;
            try
            {

                tx = clientSocket.EndSend(ar);
            }
            catch (SocketException se)
            {
                ErrorOccured = true;
                Error = NetworkError.TransmissionError;
                ErrorMessage = se.Message;

                if (NetworkErrorOccured != null)
                    NetworkErrorOccured();

                return;
            }

            if (tx < 0)
            {
                ErrorOccured = true;
                Error = NetworkError.TransmissionError;
                ErrorMessage = "Transmission recieve error, data recieved was zero bytes";

                if (NetworkErrorOccured != null)
                    NetworkErrorOccured();

                return;
            }
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
                    ErrorOccured = true;
                    Error = NetworkError.Disconnect;
                    ErrorMessage = se.Message;
                    return;
                }
            }

            byte[] recieved = new byte[rx];
            Array.Copy(buffer, recieved, rx);

            string message = Encoding.ASCII.GetString(recieved);

            clientSocket.BeginReceive(buffer, 0, 1024, SocketFlags.None, onRecieveCallback, null);

            if (message.StartsWith("#LOGIN"))
            {
                var success = (message.Split('|')[1] == "true" ? true : false);
                if (success)
                {
                    var isAdmin = (message.Split('|')[2] == "true" ? true : false);
                    Usable = true;
                    if (LoginComplete != null)
                        LoginComplete(true);
                }
                else if (LoginComplete != null)
                    LoginComplete(false);

            }
            else if (DataReady != null)
                DataReady(message);
        }

        public void BeginConnect(string address, int port)
        {
            clientSocket.BeginConnect(address, port, onConnectCompleteCallback, null);
        }

        public void BeginLogin(string username, string password)
        {
            string loginRequest = String.Format("#LOGIN|{0}|{1}", username, password);
            this.WriteLine(loginRequest);
        }

        public void WriteLine(string line)
        {
            if (!Usable)
            {
                byte[] data = Encoding.ASCII.GetBytes(line);
                clientSocket.BeginSend(data, 0, data.Length, SocketFlags.None, onSentCallback, null);
            }
        }
    }
}
