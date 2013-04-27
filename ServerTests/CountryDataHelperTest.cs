using System.Data.SQLite;
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
            var db = new Database("test.db");
            dataHelper = new CountryDataHelper();
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
            Database.Instance.ClearTable("events");
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Database.Instance.ClearTable("countries");
            Database.Instance.ClearTable("events");
        }    
        #endregion

        private static CountryDataHelper dataHelper;

        /// <summary>
        ///A test for Create - runs without failing and events work
        ///</summary>
        [TestMethod()]
        public void CreateTest1()
        {
            dataHelper.Create(new Country(){Name = "New Zealand", Code = "NZ"});
            VerifyEvent(EventType.Create);
            VerifyNumberOfEvents(1);
        }

        /// <summary>
        ///A test for Create - saves right values
        ///</summary>
        [TestMethod()]
        public void CreateTest2()
        {
            long countryId = 1;
            object created = DateTime.UtcNow;
            int active = 1;
            string name = "Wellington";
            string code = "NZ";
         
            dataHelper.Create(new Country() { Name = name, Code = code });

            var expected = new object[] {0, 0, countryId, created, active, name, code};
            var actual = Database.Instance.GetLastRows("countries", 1)[0];

            AssertRowValuesMatch(expected, actual);
        }

        /// <summary>
        ///A test for Create - fails if same name and code already exists. Also checks that event isn't saved if fails
        ///</summary>
        [TestMethod()]
        public void CreateFailsIfAlreadyExistsTest1()
        {
            dataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });

            try
            {
                dataHelper.Create(new Country() {Name = "New Zealand", Code = "NZ"});
                throw new AssertFailedException("Should have thrown a DatabaseException");
            }
            catch (DatabaseException e)
            {
                Console.WriteLine(e);
                VerifyNumberOfEvents(1);
            }
      
        }

        /// <summary>
        ///A test for Create - fails if same name already exists.
        ///</summary>
        [TestMethod()]
        public void CreateFailsIfAlreadyExistsTest2()
        {
            dataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });

            try
            {
                dataHelper.Create(new Country() { Name = "New Zealand", Code = "N" });
                throw new AssertFailedException("Should have thrown a DatabaseException");
            }
            catch (DatabaseException e)
            {
                Console.WriteLine(e);
                VerifyNumberOfEvents(1);
            }
        }

        /// <summary>
        ///A test for Create - fails if same code already exists.
        ///</summary>
        [TestMethod()]
        public void CreateFailsIfAlreadyExistsTest3()
        {           
            dataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });

            try
            {
                dataHelper.Create(new Country() { Name = "New Z", Code = "NZ" });
                throw new AssertFailedException("Should have thrown a DatabaseException");
            }
            catch (DatabaseException e)
            {
                Console.WriteLine(e);
                VerifyNumberOfEvents(1);
            }
        }

        /// <summary>
        ///A test for Delete - it runs without failing
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            dataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });
            dataHelper.Delete(1);
            VerifyEvent(EventType.Delete);
            VerifyNumberOfEvents(2);
        }

        /// <summary>
        ///A test for Delete - loading it after deleting returns null.
        ///</summary>
        [TestMethod()]
        public void DeleteTest2()
        {
            dataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });
            dataHelper.Delete(1);

            Assert.IsNull(dataHelper.Load(1));
        }

        /// <summary>
        ///A test for Delete 
        ///</summary>
        [TestMethod()]
        public void DeleteFailsIfNoCountryWithMatchingIdActive()
        {
            try
            {
                dataHelper.Delete(1);
                throw new AssertFailedException("Should have thrown a DatabaseException");
            }
            catch (DatabaseException e)
            {
                Console.WriteLine(e);
                VerifyNumberOfEvents(0);
            }
        }

        /// <summary>
        ///A test for GetId
        ///</summary>
        [TestMethod()]
        public void GetIdTest()
        {
            dataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });

            var actual = dataHelper.GetId(new Country() {Name = "New Zealand", Code = "NZ"});

            Assert.AreEqual(1, actual);
        }

        /// <summary>
        ///A test for GetId - only looks at fields it cares about
        ///</summary>
        [TestMethod()]
        public void GetIdTest2()
        {
            dataHelper.Create(new Country() { Name = "New Zealand", Code = "NZ" });

            var actual = dataHelper.GetId(new Country() { Name = "New Zealand" });

            Assert.AreEqual(1, actual);
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest1()
        {
            dataHelper.Create(new Country() { Name = "Wellington", Code = "NZ" });
            var actual = dataHelper.Load(1);
        }

        /// <summary>
        ///A test for Load - all values are correct
        ///</summary>
        [TestMethod()]
        public void LoadTest2()
        {
            long countryId = 1;
            DateTime created = DateTime.UtcNow;
            string name = "Wellington";
            string code = "NZ";

            dataHelper.Create(new Country() { Name = name, Code = code });
            var actual = dataHelper.Load(1);

            Assert.AreEqual(countryId, actual.ID);
            Assert.AreEqual(name, actual.Name);
            Assert.AreEqual(code, actual.Code);
            Assert.IsTrue(created.AddSeconds(1) > actual.LastEdited);
        }

        /// <summary>
        ///A test for Load - returns null when country is deleted.
        ///</summary>
        [TestMethod()]
        public void LoadTest3()
        {
            dataHelper.Create(new Country() { Name = "Wellington", Code = "NZ" });
            dataHelper.Delete(1);
            var actual = dataHelper.Load(1);

            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for Load 
        ///</summary>
        [TestMethod()]
        public void LoadReturnsNullWhenNoMatch()
        {
            dataHelper.Create(new Country() { Name = "Wellington", Code = "NZ" });
            var actual = dataHelper.Load(2);

            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for LoadAll - runs without failing
        ///</summary>
        [TestMethod()]
        public void LoadAllTest()
        {
            dataHelper.Create(new Country() { Name = "Wellington", Code = "NZ" });
            dataHelper.Create(new Country(){Name="Australia", Code="AUS"});

            var result = dataHelper.LoadAll();

            Assert.IsTrue(result.Keys.Count == 2);
        }

        /// <summary>
        ///A test for LoadAll - runs when table is empty
        ///</summary>
        [TestMethod()]
        public void LoadAllTest1()
        {
            var result = dataHelper.LoadAll();

            Assert.IsTrue(result.Keys.Count == 0);
        }

        /// <summary>
        ///A test for LoadAll - values are correct
        ///</summary>
        [TestMethod()]
        public void LoadAllTest2()
        {

            dataHelper.Create(new Country() { Name = "Wellington", Code = "NZ" });
            dataHelper.Create(new Country() { Name = "Australia", Code = "AUS" });

            var result = dataHelper.LoadAll();

            var country1 = result[1];
            var country2 = result[2];

            Assert.AreEqual(1, country1.ID);
            Assert.AreEqual("Wellington", country1.Name);
            Assert.AreEqual("NZ", country1.Code);
            Assert.IsTrue(country1.LastEdited.AddSeconds(1) > DateTime.UtcNow);

            Assert.AreEqual(2, country2.ID);
            Assert.AreEqual("Australia", country2.Name);
            Assert.AreEqual("AUS", country2.Code);
            Assert.IsTrue(country2.LastEdited.AddSeconds(1) > DateTime.UtcNow);
        }

        /// <summary>
        ///A test for Update - runs without failing
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            var country = new Country() {Name = "Wellington", Code = "NZ"};
            
            dataHelper.Create(country);

            country.Code = "NB";
            dataHelper.Update(country);

            VerifyEvent(EventType.Update);
            VerifyNumberOfEvents(2);
        }

        /// <summary>
        ///A test for Update - fails if doesn't exist
        ///</summary>
        [TestMethod()]
        public void UpdateTest1()
        {
            try
            {
                var country = new Country() {Name = "Wellington", Code = "NZ"};
                dataHelper.Update(country);
                throw new AssertFailedException("Should have thrown a DatabaseException");
            }
            catch (DatabaseException e)
            {
                Console.WriteLine(e);
                VerifyNumberOfEvents(0);
            }
        }


        /// <summary>
        ///A test for Update - should pass if you force an error in Update
        ///</summary>
        [TestMethod()]
        public void UpdateTestForTransactionRollback()
        {
            var country = new Country() { Name = "Wellington", Code = "NZ" };
            dataHelper.Create(country);

            try
            {           
                dataHelper.Update(country);
                throw new AssertFailedException("Should have thrown a DatabaseException");
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e);
                VerifyNumberOfEvents(1);
            }
        }


        /// <summary>
        ///A test for Update - runs without failing
        ///</summary>
        [TestMethod()]
        public void UpdateTest2()
        {      
            long countryId = 1;
            object created = DateTime.UtcNow;
            string name = "Wellington";
            string code = "NZ";

            // create
            var country = new Country() {Name = name, Code = code};
            dataHelper.Create(country);

            // update
            country.Code = "NB";
            dataHelper.Update(country);

            // get last two rows of database
            var actual = Database.Instance.GetLastRows("countries", 2);

            // what we expect them to look like
            var expected1 = new object[] { 0, 0, countryId, created, 0, name, code };
            var expected2 = new object[] { 0, 0, countryId, created, 1, name, "NB" };

            // assert
            AssertRowValuesMatch(expected1, actual[0]);
            AssertRowValuesMatch(expected2, actual[1]);
        }


        /// <summary>
        /// Super awesome private method to help verify that an update was correct.
        /// 
        /// It even checks DateTime!
        /// Give it DateTime.UtcNow for the 'created' field, and it will verify it happened within a second ago.  
        /// Otherwise give it null, and it won't check the date at all.
        /// 
        /// 
        /// </summary>
        /// <param name="expected">An array of objects with values for all columns.  First entry is the ID and won't be looked at so set it to 0.</param>
        /// <param name="actual">the row loaded from DB</param>
        private void AssertRowValuesMatch(object[] expected, object[] actual)
        {
            for (int i = 2; i < expected.Length; i++)
            {
                var valExpected = expected[i];
                var valActual = actual[i];

                if (valExpected != null)
                {
                    try
                    {
                        var timeActual = (DateTime)valActual;
                        var timeExpected = (DateTime)valExpected;

                        bool happenedNow = timeExpected.CompareTo(timeActual.AddSeconds(1)) < 0;

                        Assert.IsTrue(happenedNow, String.Format("Expected: {0}, Actual: {1}", valExpected, valActual));
                    }
                    catch (InvalidCastException)
                    {
                        // check same type
                        Assert.AreEqual(valExpected.GetType(), actual[i].GetType());

                        // check values
                        Assert.IsTrue(valExpected.Equals(valActual), String.Format("Field {0} is not correct. Expected={1}, Actual={2}", i + 1, valExpected, valActual));
                    }
                }
            }
        }

        /// <summary>
        /// Private method to verify the event was saved correctly.
        /// </summary>
        /// <param name="eventType"></param>
        private void VerifyEvent(EventType eventType)
        {
            var actual = Database.Instance.GetLastRows("events", 1);

            var expected = new object[] {null, null, "Country", eventType.ToString()};

            AssertRowValuesMatch(expected, actual[0]);
        }

        /// <summary>
        /// Private method to verify the event was saved correctly.
        /// </summary>
        /// <param name="eventType"></param>
        private void VerifyNumberOfEvents(int number)
        {
            var actual = Database.Instance.GetLastRows("events", 10);
          
            Assert.AreEqual(number, actual.Length);
        }




    }
}
