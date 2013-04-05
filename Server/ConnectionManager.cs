using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public static class ConnectionManager
    {
        private static Dictionary<int, Client> connections = new Dictionary<int, Client>();

        private static int GetUnixTimestamp()
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
            return (int)Math.Floor(diff.TotalSeconds);
        }

        public static Client Get(int id)
        {
            if (connections.ContainsKey(id))
                return connections[id];

            throw new Exception("Client does not exist in ConnectionManager");
        }

        public static int Add()
        {
            int id = GetUnixTimestamp();
            connections.Add(id, new Client(id));
            return id;
        }
    }
}
