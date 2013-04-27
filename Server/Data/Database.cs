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
        private static object syncRoot = new Database("kpsmart.db", false);

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
                            instance = new Database("kpsmart.db", false);
                    }
                }


                return instance;
            }
        }

        private SQLiteConnection connection;
        private IDictionary<string, string> tables = new Dictionary<string, string>();
        private string databaseFileName;
        private int versionNumber = 3;
        private bool testDB;
        public bool IsTestDatabase {
            get { return testDB; }
        }

        private Database(string databaseFileName, bool testDatabase)
        {
            
            // set the tables to create
            tables.Add("countries", "CREATE TABLE 'countries' ('id' INTEGER PRIMARY KEY AUTOINCREMENT, 'event_id' INTEGER NOT NULL, country_id INTEGER, 'created' TIMESTAMP DEFAULT (CURRENT_TIMESTAMP) ,'active' INT DEFAULT ('0') ,'name' TEXT,'code' VARCHAR(3))");
            tables.Add("companies",
                       "CREATE  TABLE 'companies' ('id' INTEGER PRIMARY KEY AUTOINCREMENT, 'company_id' INTEGER NOT NULL , 'event_id' INTEGER NOT NULL, 'created' TIMESTAMP DEFAULT(CURRENT_TIMESTAMP) , 'active' INTEGER NOT NULL DEFAULT('0') ,'name' VARCHAR(20))");
            tables.Add("events", "CREATE TABLE 'events' ('id' INTEGER PRIMARY KEY AUTOINCREMENT, 'created' TIMESTAMP DEFAULT (CURRENT_TIMESTAMP), 'object_type' VARCHAR(20), 'event_type' VARCHAR(10))");

            // set filename and version
            // TODO: Use a config value for database to be opened.
            this.databaseFileName = databaseFileName;
            versionNumber = 3;
            testDB = testDatabase;
        }

        public void Connect()
        {
            this.connection = new SQLiteConnection(String.Format("Data Source={0};Version={1};", databaseFileName, versionNumber));
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
            return this.InsertQuery(sql, null);
        }

        public long InsertQuery(string sql, SQLiteTransaction transaction)
        {

            SQLiteCommand sqlCommand;
            if (transaction != null)
            {
                sqlCommand = new SQLiteCommand(sql, this.connection, transaction);
            }
            else
            {
                sqlCommand = new SQLiteCommand(sql, this.connection);
            }
            using (sqlCommand)
            {
                int n_rows = sqlCommand.ExecuteNonQuery();
                return this.connection.LastInsertRowId;
            }
        }

        public long FetchNumberQuery(string sql)
        {
            SQLiteCommand sqlCommand = new SQLiteCommand(sql, this.connection);

            long returnValue = 0;

            try
            {
                object row = sqlCommand.ExecuteScalar();
                if (row == null || row == DBNull.Value) return 0;

                returnValue = (long)row;
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Exception: {0}", ex);
            }
            finally
            {
                sqlCommand.Dispose();
            }

            return returnValue;
        }

        /// <summary>
        /// Returns the first row of an executed query, or an empty array if there were no results.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object[] FetchRow(string sql)
        {
            SQLiteCommand sqlCommand = new SQLiteCommand(sql, this.connection);
            List<object> row = new List<object>();

            try
            {
                SQLiteDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {

                    for (int i = 0; i < reader.VisibleFieldCount; i++)
                        row.Add(reader.GetValue(i));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine("Exception: {0}", ex.Message);
            }
            finally
            {
                sqlCommand.Dispose();
            }

            return row.ToArray();
        }

        public object[][] FetchRows(string sql)
        {
            SQLiteCommand sqlCommand = new SQLiteCommand(sql, this.connection);
            List<object[]> allRows = new List<object[]>();

            try
            {
                SQLiteDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    List<object> row = new List<object>();

                    for (int i = 0; i < reader.VisibleFieldCount; i++)
                    {
                        row.Add(reader.GetValue(i));
                    }

                    allRows.Add(row.ToArray());
                }
            }
            catch (Exception e)
            {
                Logger.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                sqlCommand.Dispose();
            }

            return allRows.ToArray();
        }

        public SQLiteTransaction BeginTransaction()
        {
            return connection.BeginTransaction();
        }


        #region for unit tests

        /// <summary>
        /// Constructor for Unit tests.  Let me know if this is a bad idea ben.
        /// </summary>
        /// <param name="databaseFileName"></param>
        public Database(string databaseFileName):this(databaseFileName, true)
        {
            instance = this;
            Connect();
        }

        /// <summary>
        /// Pull down method for Unit Testing the DB
        /// </summary>
        public void DropAllTables()
        {
            if (!testDB)
                throw new DatabaseException("Cannot drop tables of live database.");

            foreach (string tableName in tables.Keys)
            {
                var sql = String.Format("DROP TABLE IF EXISTS {0}", tableName);
                SQLiteCommand command = new SQLiteCommand(sql, this.connection);
                command.ExecuteNonQuery();
                command.Dispose();
                Logger.WriteLine(String.Format("Dropped table {0} from database {1}", tableName, databaseFileName));
            }         
        }


        public void ClearTable(string tableName)
        {
            if (!testDB)
                throw new DatabaseException("Cannot drop tables of live database.");

            var sql = String.Format("DELETE from {0}", tableName);
            SQLiteCommand command = new SQLiteCommand(sql, this.connection);
            command.ExecuteNonQuery();
            command.Dispose();

            Logger.WriteLine("Deleted all entries from table '{0}'", tableName);
        }

        public object[][] GetLastRows(string tableName, int numRowsToGet)
        {
            var sql = String.Format("SELECT * FROM (SELECT * FROM {0} ORDER BY id DESC LIMIT {1}) ORDER BY id ASC",
                                    tableName,
                                    numRowsToGet);

            return FetchRows(sql);
        }

        #endregion

    }
}
