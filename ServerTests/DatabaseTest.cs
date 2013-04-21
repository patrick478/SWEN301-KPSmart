using Server.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Server.Gui;

namespace ServerTests
{
    
    
    /// <summary>
    ///This is a test class for DatabaseTest and is intended
    ///to contain all DatabaseTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DatabaseTest
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

       
        [TestCleanup()]
        public void MyTestCleanup()
        {
            if(target != null)
                target.DropAllTables();
        }

        #endregion


        private Database target;

        /// <summary>
        ///A test for Test Database Constructor
        ///</summary>
        [TestMethod()]
        public void DatabaseConstructorTest()
        {
            string databaseFileName = "testDB1.db";
            IDictionary<string, string> tables = new Dictionary<string, string>(); 

            target = new Database(databaseFileName, tables);
        }


        /// <summary>
        ///A test for Test Database Constructor
        ///</summary>
        [TestMethod()]
        public void CheckThatDatabaseInstanceIsTestDBWhenUsingTestConstructor()
        {
            string databaseFileName = "testDB1.db";
            IDictionary<string, string> tables = new Dictionary<string, string>();

            target = new Database(databaseFileName, tables);
            Assert.IsTrue(Database.Instance.IsTestDatabase);
        }





    }
}
