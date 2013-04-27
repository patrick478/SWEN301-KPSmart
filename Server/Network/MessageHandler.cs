using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Network
{
    public static class MessageHandler
    {
        public static void PassMessage(string message, Client sendingClient)
        {
            sendingClient.SendMessage("Hello!");
        }
    }
}
