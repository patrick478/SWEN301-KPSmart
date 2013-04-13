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
            //todo
            throw new NotImplementedException();
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
        /// This either calls editCompany, or newCompany accordingly.
        /// </summary>
        /// <param name="newCompany"></param>
        public override void Save(Company Company)
        {
            if(Company.ID == 0)
                this.Create(Company);
            else 
                this.Update(Company);
        }

        /// <summary>
        /// Used for saving changes to an existing Company.
        /// </summary>
        /// <param name="Company"></param>
        public override Company Update(Company Company)
        {
            
            var sql = String.Format("INSERT INTO `{0}` ({1}, active, name) VALUES {3}, 1, '{2}')",TABLE_NAME, ID_COL_NAME,
            Company.Name, Company.ID);

            // START POTENTIAL LOCK ZONE
            long inserted_id = Database.Instance.InsertQuery(sql);
            sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}=(SELECT {1} FROM `{0}` WHERE id={2}) AND id != {0}",TABLE_NAME, ID_COL_NAME, inserted_id);
            Database.Instance.InsertQuery(sql);
            // END POTENTIAL LOCK ZONE

            throw new NotImplementedException();

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

            //todo: get the new id of the created company, and set

        }


        



        /// <summary>
        /// Deletes the Company, and returns a copy of the deleted Company.
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




