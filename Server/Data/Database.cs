using System.Data.SQLite;
using Server.Gui;
using System.Collections.Generic;

namespace Server.Data
{
    public class Database
    {
        private SQLiteConnection connection;
        private Dictionary<string, string> tables = new Dictionary<string, string>();

        public Database()
        {
            tables.Add("syslog", "CREATE TABLE syslog (id int, body text, time timestamp)");
        }

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

            // Perform table existance checks.
            foreach(KeyValuePair<string, string> kvp in tables)
            {
                var sql = string.Format("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{0}';", kvp.Key);
                SQLiteCommand command = new SQLiteCommand(sql, this.connection);
                object returnedValue = command.ExecuteScalar();

                bool exists = false;
                if (returnedValue == null)
                    exists = false;
                else
                    if ((long)returnedValue > 0)
                        exists = true;

                if (!exists)
                {
                    Logger.WriteLine("Table {0} does not exist.. creating..", kvp.Key);
                    command = new SQLiteCommand(kvp.Value, this.connection);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
