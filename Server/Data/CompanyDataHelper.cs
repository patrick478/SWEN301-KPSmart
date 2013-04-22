using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Gui;

namespace Server.Data
{

        /// <summary>
    /// The idea is this class is used to extract Countries from the DB, and save Countries to the DB.  
    /// It should be used by the CompanyService class.
    /// 
    /// Maybe it returns the DateTime instead of void so StateSnapshot can be updated???
    /// </summary>
    class CompanyDataHelper: DataHelper<Company>
    {
        private const string TABLE_NAME = "companies";
        private const string ID_COL_NAME = "company_id";

        /// <summary>
        /// Loads the Company of the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Company Load(int id)
        {
            var sql = String.Format("SELECT name FROM `{0}` WHERE {1}={2} ORDER BY created DESC LIMIT 1", TABLE_NAME, ID_COL_NAME, id);
            object[] row = Database.Instance.FetchRow(sql);

            string name = row[0] as string;

            return new Company { Name = name, ID = id };
        }

        /// <summary>
        /// Loads the most up to date version of all Countries in the system.
        /// </summary>
        /// <returns></returns>
        public override IDictionary<int, Company> LoadAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads all Countries that were in the system at the given snapshotTime.
        /// </summary>
        /// <param name="snapshotTime"></param>
        /// <returns></returns>
        public override IDictionary<int, Company> LoadAll(DateTime snapshotTime)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Used for saving changes to an existing Company.
        /// </summary>
        /// <param name="Company"></param>
        public override void Update(Company Company)
        {
            // LOCK BEGINS HERE
            var sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME, Company.ID);
            Database.Instance.InsertQuery(sql);

            // TODO: Should this insert the full row information?
            sql = String.Format("INSERT INTO `{0}` ({1}, active, name) VALUES ({2}, 1, '{3}')", TABLE_NAME, ID_COL_NAME, Company.ID, Company.Name);
            Database.Instance.InsertQuery(sql);
            // LOCK ENDS HERE
        }

        /// <summary>
        /// Used for saving a new Company.
        /// </summary>
        /// <param name="Company"></param>
        public override void Create(Company Company)
        {
            var sql = String.Format("INSERT INTO `{0}` ({1}, active, name) VALUES (coalesce((SELECT MAX({1})+1 FROM `{0}`), 1), 1, '{2}')",TABLE_NAME, ID_COL_NAME,
            Company.Name);
            long inserted_id = Database.Instance.InsertQuery(sql);

            sql = String.Format("SELECT {1} FROM `{0}` WHERE id={2}", TABLE_NAME, ID_COL_NAME, inserted_id);
            long country_id = Database.Instance.FetchNumberQuery(sql);
            Company.ID = (int)country_id;
        }

            public override int GetId(Company country)
            {
                throw new NotImplementedException();
            }

            public override void Delete(Company obj)
        {
            throw new NotImplementedException();
        }


        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }




    }
}




