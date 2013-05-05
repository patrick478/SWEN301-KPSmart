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
    /// It should be used by the companyService class.
    /// 
    /// Maybe it returns the DateTime instead of void so StateSnapshot can be updated???
    /// </summary>
    public class CompanyDataHelper : DataHelper<Company>
    {
        private const string TABLE_NAME = "company";
        private const string ID_COL_NAME = "company_id";

        /// <summary>
        /// Loads the company of the given id.  If no company exists, returns null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Company Load(int id)
        {
            string sql;
            object[] row;

            // LOCK BEGINS HERE
            //lock (Database.Instance)
            {
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, ID_COL_NAME, id.ToString(), new string[] { "name", "created" });
                row = Database.Instance.FetchRow(sql);
            }
            // LOCK ENDS HERE

            if (row.Length == 0)
            {
                return null;
            }

            string name = row[0] as string;
            string code = row[1] as string;
            DateTime created = (DateTime)row[2];

            var company = new Company { Name = name, ID = id, LastEdited = created };
            Logger.WriteLine("Loaded company: " + company);

            return company;
        }

        /// <summary>
        /// Loads the most up to date version of all Countries in the system.
        /// </summary>
        /// <returns></returns>
        public override IDictionary<int, Company> LoadAll()
        {
            string sql;
            object[][] rows;

            // BEGIN LOCK HERE
            //lock (Database.Instance)
            {

                sql = SQLQueryBuilder.SelectFields(TABLE_NAME, new string[] { ID_COL_NAME, "name", "code", "created" });
                rows = Database.Instance.FetchRows(sql);
            }
            // END LOCK HERE
            Logger.WriteLine("Loaded {0} countries:", rows.Length);

            var results = new Dictionary<int, Company>();
            foreach (object[] row in rows)
            {
                // extract data
                long id = (long)row[0];
                string name = row[1] as string;
                string code = row[2] as string;
                DateTime created = (DateTime)row[3];

                // make company
                var company = new Company { ID = (int)id, Name = name, LastEdited = created };
                Logger.WriteLine(company.ToString());

                // add company to results
                results.Add((int)id, company);
            }

            return results;
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
        /// Used for saving changes to an existing company.
        /// </summary>
        /// <param name="company"></param>
        public override void Update(Company company)
        {
            // check it has an id, and fetch it if not
            if (company.ID == 0)
                throw new DatabaseException("Cannot update a company with no ID.");

            // check the key field isn't changing
            var existingcompany = Load(company.ID);
            if (company.Name != existingcompany.Name)
                throw new DatabaseException("Cannot modify the key field 'Name' of the object.");

            string sql;
            object[] row;

            // create a transaction
            SQLiteTransaction transaction = Database.Instance.BeginTransaction();
            try
            {

                // get event number
                sql = SQLQueryBuilder.SaveEvent(ObjectType.Company, EventType.Update);
                long eventId = Database.Instance.InsertQuery(sql, transaction);

                // deactivate all previous records
                sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME,
                                    company.ID);
                Database.Instance.InsertQuery(sql, transaction);

                // insert new record
                var fieldNames = new string[] { EVENT_ID, ID_COL_NAME, "active", "name", "code" };
                var values = new string[] { eventId.ToString(), company.ID.ToString(), "1", company.Name };
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
                Logger.WriteLine("Exception occured during company.Update() - rolling back:");
                Logger.WriteLine(e.Message);
            }

            // get lastEdited 
            sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, ID_COL_NAME, company.ID.ToString(),
                                                                    new string[] { "created" });
            row = Database.Instance.FetchRow(sql);

            // update last edited
            company.LastEdited = (DateTime)row[0];
            Logger.WriteLine("Updated company: " + company);
        }

        /// <summary>
        /// Used for saving a new company. 
        /// </summary>
        /// <param name="company"></param>
        public override void Create(Company company)
        {
            object[] row;
            int ID;

            // check it is legal
            int companyID = GetId(company);

            if (companyID != 0)
                throw new DatabaseException("A company with that name already exists");


            // LOCK BEGINS HERE
            //lock (Database.Instance)
            {
                // get event number
                var sql = SQLQueryBuilder.SaveEvent(ObjectType.Company, EventType.Create);
                long eventId = Database.Instance.InsertQuery(sql);

                // insert the record
                sql = SQLQueryBuilder.CreateNewRecord(TABLE_NAME,
                                                          ID_COL_NAME,
                                                          new string[] { EVENT_ID, "name" },
                                                          new string[] { eventId.ToString(), company.Name });
                long inserted_id = Database.Instance.InsertQuery(sql);

                // get id and LastEdited
                var fields = new string[] { ID_COL_NAME, "created" };
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "id", inserted_id.ToString(), fields);
                row = Database.Instance.FetchRow(sql);
                long id = (long)row[0];
                ID = (int)id;
            }
            // LOCK ENDS HERE

            // set id and lastedited
            company.ID = (int)ID;
            company.LastEdited = (DateTime)row[1];

            Logger.WriteLine("Created company: " + company);
        }


        /// <summary>
        /// Returns the id of the company that is active and matches the given countries name,
        /// and also sets the ID field to that value.
        /// 
        /// Returns 0 if it doesn't exist in the Database.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public override int GetId(Company company)
        {
            long id = 0;

            //LOCK BEGINS HERE
            //lock (Database.Instance)
            {
                // get id of matching record
                var sql = SQLQueryBuilder.SelectFieldsWhereFieldLike(TABLE_NAME, "name", company.Name,
                                                                     new[] { ID_COL_NAME });
                id = Database.Instance.FetchNumberQuery(sql);
            }
            //LOCK ENDS HERE

            // set id in company
            company.ID = (int)id;

            // return result
            return company.ID;

        }

        /// <summary>
        /// Deletes the company
        /// This allows the delete to be time stamped.
        /// </summary>
        /// <param name="id">The ID of the company to be deleted</param>
        /// <returns></returns>
        public override void Delete(int id)
        {
            if (Load(id) == null)
                throw new DatabaseException(String.Format("There is no active record with company_id='{0}'", id));

            // LOCK BEGINS HERE
            //lock (Database.Instance)
            {
                // get event number
                var sql = SQLQueryBuilder.SaveEvent(ObjectType.Company, EventType.Delete);
                long eventId = Database.Instance.InsertQuery(sql);

                // set all entries to inactive
                sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME, id);
                Database.Instance.InsertQuery(sql);

                // insert new 'deleted' row
                sql = SQLQueryBuilder.InsertFields(TABLE_NAME,
                                                   new string[] { EVENT_ID, ID_COL_NAME, "active", "name", "code" },
                                                   new string[] { eventId.ToString(), id.ToString(), "-1", "", "" });
                Database.Instance.InsertQuery(sql);
            }
            // LOCK ENDS HERE

            Logger.WriteLine("Deleted company: " + id);
        }

        /// <summary>
        /// Deltes the company
        /// </summary>
        /// <param name="obj">The company to be delelted</param>
        public override void Delete(Company  obj)
        {
            if (obj.ID == 0)
            {
                int id = GetId(obj);

                if (id == 0)
                    throw new DatabaseException("There is no active record matching that company to delete: " + obj);

                Delete(id);
            }

            Delete(obj.ID);
        }
    }
}
