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

        /// <summary>
        /// Loads the Country of the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Country Load(int id)
        {
            //todo
            throw new NotImplementedException();
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
        /// This either calls editCountry, or newCountry accordingly.
        /// </summary>
        /// <param name="newCountry"></param>
        public override void Save(Country country)
        {
            if(country.ID == 0)
                this.Create(country);
            this.Update(country);
        }

        /// <summary>
        /// Used for saving changes to an existing Country.
        /// </summary>
        /// <param name="Country"></param>
        public override Country Update(Country Country)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Used for saving a new Country.
        /// </summary>
        /// <param name="Country"></param>
        public override void Create(Country country)
        {
            var sql = String.Format("INSERT INTO `countries` (country_id, active, name, code) VALUES (coalesce((SELECT MAX(country_id)+1 FROM `countries`), 1), 1, '{0}', '{1}')", country.Name, country.Code);
            long inserted_id = Database.Instance.InsertQuery(sql);
            
            sql = String.Format("SELECT country_id FROM `countries` WHERE id={0}", inserted_id);
            Logger.WriteLine("Before");
            long country_id = Database.Instance.FetchNumberQuery(sql);
            Logger.WriteLine("After");
            country.ID = (int)country_id;
            Logger.WriteLine("Country.ID={0}", country.ID);
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
            var sql = String.Format("UPDATE `countries` SET active=0 WHERE country_id={0}", id);
            Database.Instance.InsertQuery(sql);

            // TODO: Should this insert the full row information?
            sql = String.Format("INSERT INTO `countries` (country_id, active, name, code) VALUES ({0}, -1, {1}, {2})", id, "", "");
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
