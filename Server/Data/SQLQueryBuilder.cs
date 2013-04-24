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
        /// SELECT 'requiredFieldNames' FROM `tableName` WHERE active=1 AND 'fieldName'='fieldValue'
        /// </summary>
        /// <param name="id">the id of the object to be loaded</param>
        /// <param name="tableName"></param>
        /// <param name="idColumnName"></param>
        /// <param name="requiredFieldNames"></param>
        /// <returns></returns>
        public static string SelectFieldsWhereFieldEquals(string tableName, string fieldName, string fieldValue, string[] requiredFieldNames)
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
            var sql = String.Format("SELECT {3} FROM `{0}` WHERE active=1 AND {1}={2}", tableName, fieldName, fieldValue, fields);

            return sql;
        }


        public static string SelectFieldsWhereFieldLike(string tableName, string fieldName, string fieldValue, string[] requiredFieldNames)
        {
            // String.Format("SELECT {1}, name, created FROM `{0}` WHERE active=1 AND code LIKE '{2}'", TABLE_NAME, ID_COL_NAME, code);

            // format the fields section
            string fields = "";
            foreach (string field in requiredFieldNames)
            {
                fields += field + ", ";
            }
            fields = fields.Trim();
            fields = fields.Trim(',');

            // build the query
            var sql = String.Format("SELECT {3} FROM `{0}` WHERE active=1 AND {1} LIKE '{2}'", tableName, fieldName, fieldValue, fields);

            return sql;
        }

        /// <summary>
        /// SELECT 'requiredFieldNames' FROM `'tableName'` WHERE active=1
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="requiredFieldNames"></param>
        /// <returns></returns>
        public static string SelectFields(string tableName, string[] requiredFieldNames) {

            // format the fields section
            string fields = "";
            foreach (string field in requiredFieldNames)
            {
                fields += field + ", ";
            }
            fields = fields.Trim();
            fields = fields.Trim(',');

            // build the query
            var sql = String.Format("SELECT {1} FROM `{0}` WHERE active=1", tableName, fields);

            return sql;
        
        
        }

        public static string InsertFields(string tableName, string[] fieldNames, string[] values) {

            // format the fieldNames section
            string fields = "";
            foreach (string field in fieldNames)
            {
                fields += field + ", ";
            }
            fields = fields.Trim();
            fields = fields.Trim(',');

            // format the fieldValues section
            string fieldValues = "";
            foreach (string field in values)
            {
                fields += field + "', '";
            }
            fields.Trim('\'');
            fields = fields.Trim();
            fields = fields.Trim(',');

            return String.Format("INSERT INTO `{0}` ({1}) VALUES ({2})", tableName, fields, fieldValues);
        }


        public static string CreateNewRecord() {

            String.Format("INSERT INTO `{0}` ({1}, active, name, code) VALUES (coalesce((SELECT MAX({1})+1 FROM `{0}`), 1), 1, '{2}', '{3}')", TABLE_NAME, ID_COL_NAME, country.Name, country.Code);
        
        
        
        }

    }
}
