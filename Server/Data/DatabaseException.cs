using System;

namespace Server.Data
{
    public class DatabaseException: Exception
    {

        public DatabaseException(string message) : base(message)
        {
        }
    }
}
