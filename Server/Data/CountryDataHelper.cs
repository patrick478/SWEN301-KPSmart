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
            var sql = String.Format("SELECT name, code FROM `{0}` WHERE {1}={2} ORDER BY created DESC LIMIT 1", TABLE_NAME, ID_COL_NAME, id);
            object[] row = Database.Instance.FetchRow(sql);

            string name = row[0] as string;
            string code = row[1] as string;

            // todo refresh timestamp

            return new Country {Name = name, Code = code, ID = id};
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

            int id = (int)row[0];
            string name = row[1] as string;

            // todo refresh timestamp

            return new Country {ID = id, Name = name, Code = code};
        }



        /// <summary>
        /// Loads the most up to date version of all Countries in the system.
        /// </summary>
        /// <returns></returns>
        public override IDictionary<int, Country> LoadAll()
        {
            throw new NotImplementedException();
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

            // todo refresh timestamp

            // LOCK ENDS HERE
        }

        /// <summary>
        /// Used for saving a new Country. 
        /// </summary>
        /// <param name="Country"></param>
        public override void Create(Country country)
        {
            if (GetId(country) != 0)
                throw new DatabaseException("A country with that name already exists");

            if(Load(country.Code) != null)
                throw new DatabaseException("A country with that code already exists");

            // insert the record
            var sql = String.Format("INSERT INTO `{0}` ({1}, active, name, code) VALUES (coalesce((SELECT MAX({1})+1 FROM `{0}`), 1), 1, '{2}', '{3}')", TABLE_NAME, ID_COL_NAME, country.Name, country.Code);
            long inserted_id = Database.Instance.InsertQuery(sql);
            
            // get id
            sql = String.Format("SELECT {1} FROM `{0}` WHERE id={2}",TABLE_NAME, ID_COL_NAME, inserted_id);
            long country_id = Database.Instance.FetchNumberQuery(sql);

            // set id
            country.ID = (int)country_id;

            //todo set timestamp
        }


        /// <summary>
        /// Returns the id of the country that is active and matches the given countries name.  Returns 0 if it doesn't exist.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int GetId(Country obj)
        {
            var sql = String.Format("SELECT {1} FROM `{0}` WHERE active=1 AND name LIKE '{2}'", TABLE_NAME, ID_COL_NAME, obj.Name);
            long id = Database.Instance.FetchNumberQuery(sql);
            return (int) id;
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
            sql = String.Format("INSERT INTO `{0}` ({1}, active, name, code) VALUES ({2}, -1, {3}, {4})", TABLE_NAME, ID_COL_NAME, id, "", "");
            Database.Instance.InsertQuery(sql);
            // LOCK ENDS HERE
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
