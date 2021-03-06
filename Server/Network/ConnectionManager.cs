﻿//////////////////////
// Original Writer: Ben Anderson.
// Reviewed by:  Isabel B-N 06/04/13
//////////////////////

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Server.Gui;

namespace Server.Network
{
    /// <summary>
    /// This is a class designed to allow for the mangement of all connected clients.
    /// </summary>
    public static class ConnectionManager
    {
        // Tracks the actual connections.
        // I used a Dictionary instead of List because *i think* that a list doesnt keep it's
        // index's upon removal. For further investigation. It's a minor issue, anyway.
        private static Dictionary<int, Client> connections = new Dictionary<int, Client>();

        // Gets the current Unix timestamp. It's quite possibly incorrect, but at least it's consistent.
        private static int GetUnixTimestamp()
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;
            return (int)Math.Floor(diff.TotalSeconds);
        }

        /// <summary>
        /// Fetches a client with the given ID
        /// </summary>
        /// <param name="id">The ID of the client</param>
        /// <returns>The client with the ID requested</returns>
        public static Client Get(int id)
        {
            if (connections.ContainsKey(id))
                return connections[id];

            throw new Exception("Client does not exist in ConnectionManager");
        }

        public static List<Client> GetAll()
        {
            List<Client> clients = new List<Client>();
            foreach (KeyValuePair<int, Client> kvp in connections)
            {
                clients.Add(kvp.Value);
            }

            return clients;
        }

        /// <summary>
        /// Creates a new Client and returns the newly allocated ID number.
        /// This doesn't return the Client for synchronisation reasons.
        /// </summary>
        /// <returns>The new client ID</returns>
        public static int Add(Socket clientSocket)
        {
            int id = GetUnixTimestamp();
            connections.Add(id, new Client(id, clientSocket));
            return id;
        }

        public static void Remove(Client client)
        {
            int id = client.ID;
            connections.Remove(id);
            Logger.WriteLine("Client {0} removed (probably disconnected).", id);
        }
    }
}
