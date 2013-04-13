using System;
using System.Data.SQLite;
using Server.Gui;
using System.Collections.Generic;

namespace Server.Data
{
    public class Database
    {
        // The singleton instance
        private static volatile Database instance;
        // Locking object for the singleton. Thread safety!
        private static object syncRoot = new Database();

        // The variable to fetches the instance, it's all magical. Uses C# Get. 
        public static Database Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Database();
                    }
                }


                return instance;
            }
        }

        private SQLiteConnection connection;
        private Dictionary<string, string> tables = new Dictionary<string, string>();

        public Database()
        {
            tables.Add("countries", "CREATE TABLE 'countries' ('id' INTEGER PRIMARY KEY AUTOINCREMENT , country_id INTEGER, 'created' TIMESTAMP DEFAULT (CURRENT_TIMESTAMP) ,'active' INT DEFAULT ('0') ,'name' TEXT,'code' VARCHAR(3))");
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

        public long InsertQuery(string sql)
        {
            Logger.WriteLine("InsertQuery: {0}", sql);
            SQLiteCommand sqlCommand = new SQLiteCommand(sql, this.connection);
            Logger.WriteLine("InsertQuery-2");
            try
            {
                int n_rows = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Exception: {0}", ex);
            }
            Logger.WriteLine("InsertQuery-3");
            return this.connection.LastInsertRowId;
        }

        public long FetchNumberQuery(string sql)
        {
            SQLiteCommand sqlCommand = new SQLiteCommand(sql, this.connection);
            Logger.WriteLine("SQL: {0}", sql);


            long returnValue = 0;

            try
            {
                object row = sqlCommand.ExecuteScalar();


                if (row == DBNull.Value) return 0;

                returnValue = (long)row;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("{0}", ex);
            }

            return returnValue;
        }
    }
}
