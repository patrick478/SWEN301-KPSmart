using Server.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Common;
using System.Collections.Generic;

namespace ServerTests
{
    
    
    /// <summary>
    ///This is a test class for CountryDataHelperTest and is intended
    ///to contain all CountryDataHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CountryDataHelperTest
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

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var db = new Database("test.db", new Dictionary<string, string>() { { "countries", "CREATE TABLE 'countries' ('id' INTEGER PRIMARY KEY AUTOINCREMENT , country_id INTEGER, 'created' TIMESTAMP DEFAULT (CURRENT_TIMESTAMP) ,'active' INT DEFAULT ('0') ,'name' TEXT,'code' VARCHAR(3))" } });
            countryDataHelper = new CountryDataHelper();
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            Database.Instance.DropAllTables();
        }
        
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Database.Instance.ClearTable("countries");
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Database.Instance.ClearTable("countries");
        }    
        #endregion




        private static CountryDataHelper countryDataHelper;

        /// <summary>
        ///A test for Create - runs without failing
        ///</summary>
        [TestMethod()]
        public void CreateTest1()
        {
            countryDataHelper.Create(new Country(){Name = "New Zealand", Code = "NZ"});
        }

        /// <summary>
        ///A test for Create - saves right values
        ///</summary>
        [TestMethod()]
        public void CreateTest2()
        {
            long id = 0;
            long countryId = 1;
            object created = null;
            int active = 1;
            string name = "Wellington";
            string code = "NZ";
         
            countryDataHelper.Create(new Country() { Name = name, Code = code });


            var expected = new object[] {id, countryId, created, active, name, code};
            var actual = Database.Instance.GetLastRows("countries", 1)[0];

            AssertRowValuesMatch(expected, actual);
        }


        private void AssertRowValuesMatch(object[] expected, object[] actual)
        {
            for (int i = 1; i < expected.Length; i++)
            {
                var valExpected = expected[i];
                var valActual = actual[i];

                if (valExpected != null)
                {
//                    try
//                    {
//                        //var time = Date
//                    }

                    // check same type
                    Assert.AreEqual(valExpected.GetType(), actual[i].GetType());

                    // check values
                    Assert.IsTrue(valExpected.Equals(valActual), String.Format("Field {0} is not correct. Expected={1}, Actual={2}", i + 1, valExpected, valActual));
                }
            }


        }



        /// <summary>
        ///A test for Create - fails if same name and code already exists.
        ///</summary>
        [TestMethod()]
        public void CreateFailsIfAlreadyExistsTest1()
        {
            countryDataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });

            try
            {
                countryDataHelper.Create(new Country() {Name = "New Zealand", Code = "NZ"});
                throw new AssertFailedException("Should have thrown a DatabaseException");
            }
            catch (DatabaseException e)
            {
            }
      
        }

        /// <summary>
        ///A test for Create - fails if same name already exists.
        ///</summary>
        [TestMethod()]
        public void CreateFailsIfAlreadyExistsTest2()
        {
            countryDataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });

            try
            {
                countryDataHelper.Create(new Country() { Name = "New Zealand", Code = "N" });
                throw new AssertFailedException("Should have thrown a DatabaseException");
            }
            catch (DatabaseException e)
            {
            }
        }

        /// <summary>
        ///A test for Create - fails if same code already exists.
        ///</summary>
        [TestMethod()]
        public void CreateFailsIfAlreadyExistsTest3()
        {           
            countryDataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });

            try
            {
                countryDataHelper.Create(new Country() { Name = "New Z", Code = "NZ" });
                throw new AssertFailedException("Should have thrown a DatabaseException");
            }
            catch (DatabaseException e)
            {
            }
        }

        /// <summary>
        ///A test for Delete - it runs without failing
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            countryDataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });
            countryDataHelper.Delete(1);
        }

        /// <summary>
        ///A test for Delete - loading it after deleting returns null.
        ///</summary>
        [TestMethod()]
        public void DeleteTest2()
        {
            countryDataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });
            countryDataHelper.Delete(1);

            Assert.IsNull(countryDataHelper.Load(1));
        }

        /// <summary>
        ///A test for Delete 
        ///</summary>
        [TestMethod()]
        public void DeleteFailsIfNoCountryWithMatchingIdActive()
        {
            try
            {
                countryDataHelper.Delete(1);
                throw new AssertFailedException("Should have thrown a DatabaseException");
            }
            catch (DatabaseException e)
            {
            }
        }

        /// <summary>
        ///A test for GetId
        ///</summary>
        [TestMethod()]
        public void GetIdTest()
        {

        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest()
        {

        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest1()
        {

        }

        /// <summary>
        ///A test for LoadAll
        ///</summary>
        [TestMethod()]
        public void LoadAllTest()
        {

        }

        /// <summary>
        ///A test for LoadAll
        ///</summary>
        [TestMethod()]
        public void LoadAllTest1()
        {

        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {

        }
    }
}
