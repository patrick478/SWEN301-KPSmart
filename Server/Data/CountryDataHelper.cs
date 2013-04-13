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
            var sql = String.Format("INSERT INTO `countries` (country_id, active, name, code) SELECT MAX(country_id)+1, 1, '{0}', '{1}' FROM `countries`", country.Name, country.Code);

            // START POTENTIAL LOCK ZONE
            long inserted_id = Database.Instance.InsertQuery(sql);
            sql = String.Format("UPDATE `countries` SET active=0 WHERE country_id=(SELECT country_id FROM `countries` WHERE id={0}) AND id != {0}", inserted_id);
            Database.Instance.InsertQuery(sql);
            // END POTENTIAL LOCK ZONE
            
            sql = String.Format("SELECT country_id FROM `countries` WHERE id={0}", inserted_id);
            Logger.WriteLine("Before");
            int country_id = Database.Instance.FetchValueQuery<int>(sql);
            Logger.WriteLine("After");
            country.ID = country_id;
            Logger.WriteLine("Country.ID={0}", country.ID);
        }

        /// <summary>
        /// Deletes the Country, and returns a copy of the deleted Country.
        /// This allows the delete to be time stamped.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
