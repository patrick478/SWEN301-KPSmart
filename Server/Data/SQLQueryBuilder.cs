using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Data
{
    /// <summary>
    /// The purpose of this class is to put all repeatable SQL code in here in case we want to change it.
    /// This means any changes to code will be easy to update.
    /// </summary>
    public class SQLQueryBuilder
    {


        /// <summary>
        /// Returns a load query that can be run to get the given fields of the active result matching the given id in the table.
        /// </summary>
        /// <param name="id">the id of the object to be loaded</param>
        /// <param name="tableName"></param>
        /// <param name="idColumnName"></param>
        /// <param name="requiredFieldNames"></param>
        /// <returns></returns>
        public static string LoadQuery(int id, string tableName, string idColumnName, string[] requiredFieldNames)
        {
            // format the fields section
            string fields = "";
            foreach (string field in requiredFieldNames)
            {
                fields += field + ", ";
            }
            fields = fields.Trim();
            fields = fields.Trim(',');

            // build the query
            var sql = String.Format("SELECT {3} FROM `{0}` WHERE active=1 AND {1}={2}", tableName, idColumnName, id, fields);

            return sql;
        }



    }
}
