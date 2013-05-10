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

        /// <summary>
        /// SELECT 'requiredFieldNames' FROM `tableName` WHERE active=1 AND 'fieldName[0]'='fieldValue[0]' AND 'fieldName[1]'='fieldValue[1]' etc...
        /// </summary>
        /// <param name="id">the id of the object to be loaded</param>
        /// <param name="tableName"></param>
        /// <param name="idColumnName"></param>
        /// <param name="requiredFieldNames"></param>
        /// <returns></returns>
        public static string SelectFieldsWhereFieldsEqual (string tableName, string[] fieldNames, string[] fieldValues, string[] requiredFieldNames)
        {
            // format the fields section
            string fields = "";
            foreach (string field in requiredFieldNames)
            {
                fields += field + ", ";
            }
            fields = fields.Trim();
            fields = fields.Trim(',');

            // format equals section
            string equalsSection = "";
            for(int i = 0; i < fieldNames.Length; i++) 
            {
                equalsSection += String.Format("AND {0}={1} ", fieldNames[i], fieldValues[i]); 
            }
            
            // build the query
            var sql = String.Format("SELECT {2} FROM `{0}` WHERE active=1 {1}}", tableName, equalsSection, fields);

            return sql;
        }


        /// <summary>
        /// "SELECT 'requiredFieldNames' FROM `'tableName'` WHERE active=1 AND 'fieldName' LIKE 'fieldValue'"
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="requiredFieldNames"></param>
        /// <returns></returns>
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
                fieldValues += field + "', '";
            }
            fieldValues = fieldValues.Trim('\'');
            fieldValues = fieldValues.Trim();
            fieldValues = fieldValues.Trim(',');

            return String.Format("INSERT INTO `{0}` ({1}) VALUES ('{2})", tableName, fields, fieldValues);
        }


        /// <summary>
        /// INSERT INTO `'tableName'` ('idFieldName', active, 'fieldNames') VALUES (coalesce((SELECT MAX({idFieldName})+1 FROM `'tableName'`), 1), 1, 'fieldValues')
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldNames">all field names other than the 'Object_ID_field' and 'active'</param>
        /// <param name="fieldValues">values matching the fields in 'fieldNames'</param>
        /// <returns></returns>
        public static string CreateNewRecord(string tableName, string idFieldName, string[] fieldNames, string[] fieldValues) {

            // format the fieldNames section
            string fields = "";
            foreach (string field in fieldNames)
            {
                fields += field + ", ";
            }
            fields = fields.Trim();
            fields = fields.Trim(',');

            // format the fieldValues section
            string values = "";
            foreach (string field in fieldValues)
            {
                values += field + "', '";
            }
            values = values.Trim('\'');
            values = values.Trim();
            values = values.Trim(',');


            return String.Format("INSERT INTO `{0}` ({1}, active, {2}) VALUES (coalesce((SELECT MAX({1})+1 FROM `{0}`), 1), 1, '{3})", tableName, idFieldName, fields, values);
        }


        public static string SaveEvent(ObjectType objType, EventType eventType)
        {
            return String.Format("INSERT INTO `events` (object_type, event_type) VALUES ('{0}', '{1}')", objType.ToString(),
                                 eventType.ToString());
        }

    }
}
