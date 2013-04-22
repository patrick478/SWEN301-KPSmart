using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Business
{
    public class IllegalActionException: Exception
    {
        public IllegalActionException(string message) : base(message)
        {
        }
    }
}
