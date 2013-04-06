using System.Data.SQLite;
using Server.Gui;

namespace Server.Data
{
    public class Database
    {
        private SQLiteConnection connection;

        public static void CreateDatabase()
        {
            // TODO: Use a config value for the name of the database
            // TODO: Check if the file already exists.
            SQLiteConnection.CreateFile("kpsmart.db");
        }

        public void Connect()
        {
            // TODO: Use a config value for database to be opened.
            this.connection = new SQLiteConnection("Data Source=kpsmart.db;Version=3;");
            this.connection.Open();

            Logger.WriteLine("Connected to database");
        }
    }
}
