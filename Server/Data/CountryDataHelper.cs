//////////////////////
// Original Writer: Ben Anderson
// Reviewed by: 
//
// 
//////////////////////

using System;
using System.Collections.Generic;
using System.Data.SQLite;
using Common;
using Server.Gui;

namespace Server.Data 
{
    /// <summary>
    /// The idea is this class is used to extract Countries from the DB, and save Countries to the DB.  
    /// It should be used by the CountryService class.
    /// 
    /// Maybe it returns the DateTime instead of void so StateSnapshot can be updated???
    /// </summary>
    public class CountryDataHelper : DataHelper<Country>
    {
        private const string TABLE_NAME = "countries";
        private const string ID_COL_NAME = "country_id";

        /// <summary>
        /// Loads the Country of the given id.  If no country exists, returns null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Country Load(int id)
        {
            string sql;
            object[] row;

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, ID_COL_NAME, id.ToString(), new string[] { "name", "code", "created" });
                row = Database.Instance.FetchRow(sql);
            }
            // LOCK ENDS HERE

            string name = row[0] as string;
            string code = row[1] as string;
            DateTime created = (DateTime)row[2];

            var country = new Country {Name = name, Code = code, ID = id, LastEdited = created};
            //Logger.WriteLine("Loaded country: " + country);

            return country;
        }

        /// <summary>
        /// Loads the Country with the given code.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Country Load(String code)
        {
            string sql;
            object[] row;

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                sql = SQLQueryBuilder.SelectFieldsWhereFieldLike(TABLE_NAME, "code", code,
                                                                     new string[] {ID_COL_NAME, "name", "created"});
                row = Database.Instance.FetchRow(sql);
            }
            // LOCK ENDS HERE

            if (row.Length == 0)
            {
                return null;
            }

            long id = (long)row[0];
            string name = row[1] as string;
            DateTime created = (DateTime)row[2];

            var country = new Country {ID = (int)id, Name = name, Code = code, LastEdited = created};
            Logger.WriteLine("Loaded Country: " + country);

            return country;
        }



        /// <summary>
        /// Loads the most up to date version of all Countries in the system.
        /// </summary>
        /// <returns></returns>
        public override IDictionary<int, Country> LoadAll()
        {
            string sql;
            object[][] rows;

            // BEGIN LOCK HERE
            lock (Database.Instance)
            {

                sql = SQLQueryBuilder.SelectFields(TABLE_NAME, new string[] {ID_COL_NAME, "name", "code", "created"});
                rows = Database.Instance.FetchRows(sql);
            }
            // END LOCK HERE
            Logger.WriteLine("Loaded {0} countries:", rows.Length);

            var results = new Dictionary<int, Country>();
            foreach (object[] row in rows)
            {                         
                // extract data
                long id = (long)row[0];
                string name = row[1] as string;
                string code = row[2] as string;
                DateTime created = (DateTime)row[3];

                // make country
                var country = new Country { ID = (int)id, Name = name, Code = code, LastEdited = created };
                Logger.WriteLine(country.ToString());

                // add country to results
                results.Add((int)id, country);
            }
            
            return results;
        }

        /// <summary>
        /// Loads all Countries that were in the system at the given snapshotTime.
        /// </summary>
        /// <param name="snapshotTime"></param>
        /// <returns></returns>
        public override IDictionary<int, Country> LoadAll(DateTime snapshotTime)
        {
            string sql;
            object[][] rows;

            // BEGIN LOCK HERE
            lock (Database.Instance)
            {

                sql = SQLQueryBuilder.SelectFieldsAtDateTime(TABLE_NAME, new string[] { ID_COL_NAME, "name", "code", "created" }, ID_COL_NAME, snapshotTime);
                rows = Database.Instance.FetchRows(sql);
            }
            // END LOCK HERE
            Logger.WriteLine("Loaded {0} countries:", rows.Length);

            var results = new Dictionary<int, Country>();
            foreach (object[] row in rows)
            {
                // extract data
                long id = (long)row[0];
                string name = row[1] as string;
                string code = row[2] as string;
                DateTime created = (DateTime)row[3];

                // make country
                var country = new Country { ID = (int)id, Name = name, Code = code, LastEdited = created };
                Logger.WriteLine(country.ToString());

                // add country to results
                Console.WriteLine("got data: {0}, {1}, {2}", id, name, code);
                results.Add((int)id, country);
            }

            return results;
        }

        /// <summary>
        /// Used for saving changes to an existing Country.
        /// </summary>
        /// <param name="Country"></param>
        public override void Update(Country country)
        {
            // check it has an id, and fetch it if not
            if (country.ID == 0)
                    throw new DatabaseException("Cannot update a country with no ID.");

            // check the key field isn't changing
            var existingCountry = Load(country.ID);
            if (country.Name != existingCountry.Name)
                throw new DatabaseException("Cannot modify the key field 'Name' of the object.");
            if (country.Code.Equals(existingCountry.Code))
                throw new DatabaseException("There are no changes to the country");

            // check that a country with the same code doesn't already exist.
            var countryByCode = Load(country.Code);
            if(countryByCode != null && (countryByCode.ID != country.ID) )
                throw new DatabaseException("Another country with that code already exists: " + countryByCode);

            string sql;
            object[] row;

            //LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // create a transaction
                SQLiteTransaction transaction = Database.Instance.BeginTransaction();
                try
                {

                    // get event number
                    sql = SQLQueryBuilder.SaveEvent(ObjectType.Country, EventType.Update);
                    long eventId = Database.Instance.InsertQuery(sql, transaction);

                    // deactivate all previous records
                    sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME,
                                        country.ID);
                    Database.Instance.InsertQuery(sql, transaction);

                    // insert new record
                    var fieldNames = new string[] { EVENT_ID, ID_COL_NAME, "active", "name", "code" };
                    var values = new string[] { eventId.ToString(), country.ID.ToString(), "1", country.Name, country.Code };
                    sql = SQLQueryBuilder.InsertFields(TABLE_NAME, fieldNames, values);
                    Database.Instance.InsertQuery(sql, transaction);

                    //Database.Instance.InsertQuery("invalid sql", transaction);

                    // commit transaction
                    transaction.Commit();
                }
                catch (SQLiteException de)
                {
                    Console.WriteLine("Got here");
                    transaction.Rollback();
                    Console.WriteLine("Rollback complete");
                    transaction.Dispose();
                    throw de;
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Exception occured during Country.Update() - rolling back:");
                    Logger.WriteLine(e.Message);
                }

                // get lastEdited 
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, ID_COL_NAME, country.ID.ToString(),
                                                                        new string[] { "created" });
                row = Database.Instance.FetchRow(sql);
            }
            // LOCK ENDS HERE

            // update last edited
            country.LastEdited = (DateTime)row[0];
            Logger.WriteLine("Updated country: " + country);
        }

        /// <summary>
        /// Used for saving a new Country. 
        /// </summary>
        /// <param name="Country"></param>
        public override void Create(Country country)
        {
            object[] row;
            int ID;

            // check it is legal
            int countryID = GetId(country);

            if (countryID != 0)
                throw new DatabaseException("A country with that name already exists");

            if (Load(country.Code) != null)
                throw new DatabaseException("A country with that code already exists");


            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get event number
                var sql = SQLQueryBuilder.SaveEvent(ObjectType.Country, EventType.Create);
                long eventId = Database.Instance.InsertQuery(sql);  

                // insert the record
                sql = SQLQueryBuilder.CreateNewRecord(TABLE_NAME,
                                                          ID_COL_NAME,
                                                          new string[] {EVENT_ID, "name, code"},
                                                          new string[] {eventId.ToString(), country.Name, country.Code});
                long inserted_id = Database.Instance.InsertQuery(sql);

                // get id and LastEdited
                var fields = new string[] {ID_COL_NAME, "created"};
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "id", inserted_id.ToString(), fields);
                row = Database.Instance.FetchRow(sql);
                long id = (long) row[0];
                ID = (int) id;
            }
            // LOCK ENDS HERE

            // set id and lastedited
            country.ID = (int) ID;
            country.LastEdited = (DateTime) row[1];

            Logger.WriteLine("Created country: " + country);
        }


        /// <summary>
        /// Returns the id of the country that is active and matches the given countries name,
        /// and also sets the ID field to that value.
        /// 
        /// Returns 0 if it doesn't exist in the Database.
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public override int GetId(Country country)
        {
            long id = 0;

            //LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get id of matching record
                var sql = SQLQueryBuilder.SelectFieldsWhereFieldLike(TABLE_NAME, "name", country.Name,
                                                                     new[] {ID_COL_NAME});
                id = Database.Instance.FetchNumberQuery(sql);
            }
            //LOCK ENDS HERE

            // set id in country
            country.ID = (int) id;

            // return result
            return country.ID;
            
        }

        /// <summary>
        /// Deletes the country
        /// This allows the delete to be time stamped.
        /// </summary>
        /// <param name="id">The ID of the country to be deleted</param>
        /// <returns></returns>
        public override void Delete(int id)
        {
            if(Load(id) == null)
                throw new DatabaseException(String.Format("There is no active record with country_id='{0}'", id));

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get event number
                var sql = SQLQueryBuilder.SaveEvent(ObjectType.Country, EventType.Delete);
                long eventId = Database.Instance.InsertQuery(sql); 
                
                // set all entries to inactive
                sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME, id);
                Database.Instance.InsertQuery(sql);

                // insert new 'deleted' row
                sql = SQLQueryBuilder.InsertFields(TABLE_NAME,
                                                   new string[] {EVENT_ID, ID_COL_NAME, "active", "name", "code"},
                                                   new string[] {eventId.ToString(), id.ToString(), "-1", "", ""});
                Database.Instance.InsertQuery(sql);
            }
            // LOCK ENDS HERE

            Logger.WriteLine("Deleted country: " + id);
        }

        /// <summary>
        /// Deltes the country
        /// </summary>
        /// <param name="obj">The country to be delelted</param>
        public override void Delete(Country obj)
        {
            if (obj.ID == 0)
            {
                int id = GetId(obj);

                if ( id == 0)
                    throw new DatabaseException("There is no active record matching that country to delete: " + obj);

                Delete(id);
            }
            
            Delete(obj.ID);
        }
    }
}
