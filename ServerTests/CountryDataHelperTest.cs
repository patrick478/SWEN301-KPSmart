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
        ///A test for CountryDataHelper Constructor
        ///</summary>
        [TestMethod()]
        public void CountryDataHelperConstructorTest()
        {
            CountryDataHelper target = new CountryDataHelper();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            CountryDataHelper target = new CountryDataHelper(); // TODO: Initialize to an appropriate value
            Country country = null; // TODO: Initialize to an appropriate value
            target.Create(country);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            CountryDataHelper target = new CountryDataHelper(); // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            target.Delete(id);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest1()
        {
            CountryDataHelper target = new CountryDataHelper(); // TODO: Initialize to an appropriate value
            Country obj = null; // TODO: Initialize to an appropriate value
            target.Delete(obj);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetId
        ///</summary>
        [TestMethod()]
        public void GetIdTest()
        {
            CountryDataHelper target = new CountryDataHelper(); // TODO: Initialize to an appropriate value
            Country obj = null; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.GetId(obj);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest()
        {
            CountryDataHelper target = new CountryDataHelper(); // TODO: Initialize to an appropriate value
            string code = string.Empty; // TODO: Initialize to an appropriate value
            Country expected = null; // TODO: Initialize to an appropriate value
            Country actual;
            actual = target.Load(code);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest1()
        {
            CountryDataHelper target = new CountryDataHelper(); // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            Country expected = null; // TODO: Initialize to an appropriate value
            Country actual;
            actual = target.Load(id);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadAll
        ///</summary>
        [TestMethod()]
        public void LoadAllTest()
        {
            CountryDataHelper target = new CountryDataHelper(); // TODO: Initialize to an appropriate value
            IDictionary<int, Country> expected = null; // TODO: Initialize to an appropriate value
            IDictionary<int, Country> actual;
            actual = target.LoadAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadAll
        ///</summary>
        [TestMethod()]
        public void LoadAllTest1()
        {
            CountryDataHelper target = new CountryDataHelper(); // TODO: Initialize to an appropriate value
            DateTime snapshotTime = new DateTime(); // TODO: Initialize to an appropriate value
            IDictionary<int, Country> expected = null; // TODO: Initialize to an appropriate value
            IDictionary<int, Country> actual;
            actual = target.LoadAll(snapshotTime);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            CountryDataHelper target = new CountryDataHelper(); // TODO: Initialize to an appropriate value
            Country country = null; // TODO: Initialize to an appropriate value
            target.Update(country);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
