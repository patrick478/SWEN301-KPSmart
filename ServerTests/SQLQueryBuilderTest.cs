using Server.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ServerTests
{
    
    
    /// <summary>
    ///This is a test class for SQLQueryBuilderTest and is intended
    ///to contain all SQLQueryBuilderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SQLQueryBuilderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for LoadQuery
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void SelectFieldsWhereFieldEqualsTest()
        {
            
            string id = "0";
            string tableName = "countries";
            string idColumnName = "country_id";
            string[] requiredFieldNames = new string[]{"name", "code", "created"}; 

            string expected = "SELECT name, code, created FROM `countries` WHERE active=1 AND country_id=0";
            string actual = SQLQueryBuilder.SelectFieldsWhereFieldEquals(tableName, idColumnName, id, requiredFieldNames);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void SelectFieldsWhereFieldLikeTest()
        {
            string code = "NZ";
            string tableName = "countries";
            string idColumnName = "country_id";
            string[] requiredFieldNames = new string[] { "country_id", "name", "created" };
            string expected = "SELECT country_id, name, created FROM `countries` WHERE active=1 AND code LIKE 'NZ'";


            string actual = SQLQueryBuilder.SelectFieldsWhereFieldLike(tableName, "code", code, requiredFieldNames);

            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void SelectFieldsTest()
        {
            string tableName = "countries";
            string[] requiredFieldNames = new string[] { "country_id", "name", "created" };

            string expected = "SELECT country_id, name, created FROM `countries` WHERE active=1";

            string actual = SQLQueryBuilder.SelectFields(tableName, requiredFieldNames);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void InsertFieldsTest()
        {
            string expected = "INSERT INTO `countries` (country_id, name, code) VALUES ('1', 'New Zealand', 'NZ')";

            // arrange
            string tableName = "countries";
            string[] requiredFieldNames = new string[] { "country_id", "name", "code" };
            string[] fieldValues = new string[]{"1", "New Zealand", "NZ"};

            // action
            string actual = SQLQueryBuilder.InsertFields(tableName, requiredFieldNames, fieldValues);

            // assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void CreateNewRecordTest()
        {
            string expected =
                "INSERT INTO `countries` (country_id, active, name, code) VALUES (coalesce((SELECT MAX(country_id)+1 FROM `countries`), 1), 1, 'New Zealand', 'NZ')";

            // arrange
            string tableName = "countries";
            string idFieldName = "country_id";
            string[] fieldNames = new string[]{"name", "code"};
            string[] fieldValues = new string[]{"New Zealand", "NZ"};

            // act
            string actual = SQLQueryBuilder.CreateNewRecord(tableName, idFieldName, fieldNames, fieldValues);

            // assert
            Assert.AreEqual(expected, actual);
        }







    }
}
