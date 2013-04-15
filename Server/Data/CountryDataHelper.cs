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

            return new Country {Name = name, Code = code, ID = id};
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
            // LOCK BEGINS HERE
            var sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME, country.ID);
            Database.Instance.InsertQuery(sql);

            // TODO: Should this insert the full row information?
            sql = String.Format("INSERT INTO `{0}` ({1}, active, name, code) VALUES ({2}, 1, '{3}', '{4}')", TABLE_NAME, ID_COL_NAME, country.ID, country.Name, country.Code);
            Database.Instance.InsertQuery(sql);
            // LOCK ENDS HERE
        }

        /// <summary>
        /// Used for saving a new Country. 
        /// </summary>
        /// <param name="Country"></param>
        public override void Create(Country country)
        {
            var sql = String.Format("INSERT INTO `{0}` ({1}, active, name, code) VALUES (coalesce((SELECT MAX({1})+1 FROM `{0}`), 1), 1, '{2}', '{3}')", TABLE_NAME, ID_COL_NAME, country.Name, country.Code);
            long inserted_id = Database.Instance.InsertQuery(sql);
            
            sql = String.Format("SELECT {1} FROM `{0}` WHERE id={2}",TABLE_NAME, ID_COL_NAME, inserted_id);
            long country_id = Database.Instance.FetchNumberQuery(sql);
            country.ID = (int)country_id;
        }

        /// <summary>
        /// Checks whether there is an active country with the same Name already in the DB.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override bool Exists(Country obj)
        {
            var sql = String.Format("SELECT {1} FROM `{0}` WHERE active=1 AND name={2} ", TABLE_NAME, ID_COL_NAME, obj.Name);

            // do more stuff!

            return false;

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
            this.Delete(obj.ID);
        }
    }
}
