//////////////////////
// Original Writer: Ben Anderson
// Reviewed by: 
//
// 
//////////////////////

using System;
using System.Collections.Generic;
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
        /// Loads the Country of the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Country Load(int id)
        {
            var sql = String.Format("SELECT name, code, created FROM `{0}` WHERE active=1 AND {1}={2}", TABLE_NAME, ID_COL_NAME, id);
            object[] row = Database.Instance.FetchRow(sql);

            if (row.Length == 0)
            {
                return null;
            }

            string name = row[0] as string;
            string code = row[1] as string;
            DateTime created = (DateTime)row[2];

            var country = new Country {Name = name, Code = code, ID = id, LastEdited = created};
            Logger.WriteLine("Loaded country: " + country);

            return country;
        }

        /// <summary>
        /// Loads the Country with the given code.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Country Load(String code)
        {
            var sql = String.Format("SELECT {1}, name, created FROM `{0}` WHERE active=1 AND code LIKE '{2}'", TABLE_NAME, ID_COL_NAME, code);
            object[] row = Database.Instance.FetchRow(sql);

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
            var sql = String.Format("Select {1}, name, code, created FROM {0} WHERE active=1", TABLE_NAME, ID_COL_NAME);
            object[][] rows = Database.Instance.FetchRows(sql);

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Used for saving changes to an existing Country.
        /// </summary>
        /// <param name="Country"></param>
        public override void Update(Country country)
        {
            // check it has an id, and fetch it if not
            if (country.ID == 0)
            {
                int id = GetId(country);
                if (id == 0)
                    throw new DatabaseException("Cannot update a country that doesn't exist: " + country); 

                country.ID = id;
            }                     
            
            // LOCK BEGINS HERE
            // deactivate all previous records
            var sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME, country.ID);
            Database.Instance.InsertQuery(sql);

            // insert new record
            sql = String.Format("INSERT INTO `{0}` ({1}, active, name, code) VALUES ({2}, 1, '{3}', '{4}')", TABLE_NAME, ID_COL_NAME, country.ID, country.Name, country.Code);
            Database.Instance.InsertQuery(sql);

            // update lastEdited 
            sql = String.Format("SELECT created FROM `{0}` WHERE active=1 AND {1}={2}", TABLE_NAME, ID_COL_NAME, country.ID);
            var row = Database.Instance.FetchRow(sql);
            country.LastEdited = (DateTime)row[0];
            // LOCK ENDS HERE

            Logger.WriteLine("Updated country: " + country);
        }

        /// <summary>
        /// Used for saving a new Country. 
        /// </summary>
        /// <param name="Country"></param>
        public override void Create(Country country)
        {
            // check it is legal
            int countryID = GetId(country);

            if (countryID != 0)
                throw new DatabaseException("A country with that name already exists");

            if(Load(country.Code) != null)
                throw new DatabaseException("A country with that code already exists");

            // insert the record
            var sql = String.Format("INSERT INTO `{0}` ({1}, active, name, code) VALUES (coalesce((SELECT MAX({1})+1 FROM `{0}`), 1), 1, '{2}', '{3}')", TABLE_NAME, ID_COL_NAME, country.Name, country.Code);
            long inserted_id = Database.Instance.InsertQuery(sql);

            // set id and LastEdited
            sql = String.Format("SELECT {1}, created FROM `{0}` WHERE id={2}",TABLE_NAME, ID_COL_NAME, inserted_id);
            var row = Database.Instance.FetchRow(sql);

            long id = (long) row[0];
            country.ID = (int) id;
            country.LastEdited = (DateTime) row[1];

            Logger.WriteLine("Created country: " + country);
        }


        /// <summary>
        /// Returns the id of the country that is active and matches the given countries name.  Returns 0 if it doesn't exist.
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public override int GetId(Country country)
        {
            var sql = String.Format("SELECT {1} FROM `{0}` WHERE active=1 AND name LIKE '{2}'", TABLE_NAME, ID_COL_NAME, country.Name);
            long id = Database.Instance.FetchNumberQuery(sql);

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
            // LOCK BEGINS HERE
            var sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME, id);
            Database.Instance.InsertQuery(sql);

            // TODO: Should this insert the full row information?
            sql = String.Format("INSERT INTO `{0}` ({1}, active, name, code) VALUES ({2}, -1, '{3}', '{4}')", TABLE_NAME, ID_COL_NAME, id, "", "");
            Database.Instance.InsertQuery(sql);
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
