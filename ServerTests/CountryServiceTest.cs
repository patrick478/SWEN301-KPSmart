using Server.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Common;
using Server.Data;
using System.Collections.Generic;

namespace ServerTests
{
    
    
    /// <summary>
    ///This is a test class for CountryServiceTest and is intended
    ///to contain all CountryServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CountryServiceTest
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
        ///A test for CountryService Constructor
        ///</summary>
        [TestMethod()]
        public void CountryServiceConstructorTest()
        {
            StateSnapshot state = null; // TODO: Initialize to an appropriate value
            DataHelper<Country> dataHelper = null; // TODO: Initialize to an appropriate value
            CountryService target = new CountryService(state, dataHelper);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CreateCountry
        ///</summary>
        [TestMethod()]
        public void CreateCountryTest()
        {
            StateSnapshot state = null; // TODO: Initialize to an appropriate value
            DataHelper<Country> dataHelper = null; // TODO: Initialize to an appropriate value
            CountryService target = new CountryService(state, dataHelper); // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            string code = string.Empty; // TODO: Initialize to an appropriate value
            Country expected = null; // TODO: Initialize to an appropriate value
            Country actual;
            actual = target.CreateCountry(name, code);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DeleteCountry
        ///</summary>
        [TestMethod()]
        public void DeleteCountryTest()
        {
            StateSnapshot state = null; // TODO: Initialize to an appropriate value
            DataHelper<Country> dataHelper = null; // TODO: Initialize to an appropriate value
            CountryService target = new CountryService(state, dataHelper); // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            target.DeleteCountry(id);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for EditCountry
        ///</summary>
        [TestMethod()]
        public void EditCountryTest()
        {
            StateSnapshot state = null; // TODO: Initialize to an appropriate value
            DataHelper<Country> dataHelper = null; // TODO: Initialize to an appropriate value
            CountryService target = new CountryService(state, dataHelper); // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            string code = string.Empty; // TODO: Initialize to an appropriate value
            Country expected = null; // TODO: Initialize to an appropriate value
            Country actual;
            actual = target.EditCountry(id, code);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAllCountries
        ///</summary>
        [TestMethod()]
        public void GetAllCountriesTest()
        {
            StateSnapshot state = null; // TODO: Initialize to an appropriate value
            DataHelper<Country> dataHelper = null; // TODO: Initialize to an appropriate value
            CountryService target = new CountryService(state, dataHelper); // TODO: Initialize to an appropriate value
            IEnumerable<Country> expected = null; // TODO: Initialize to an appropriate value
            IEnumerable<Country> actual;
            actual = target.GetAllCountries();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadCountry
        ///</summary>
        [TestMethod()]
        public void LoadCountryTest()
        {
            StateSnapshot state = null; // TODO: Initialize to an appropriate value
            DataHelper<Country> dataHelper = null; // TODO: Initialize to an appropriate value
            CountryService target = new CountryService(state, dataHelper); // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            Country expected = null; // TODO: Initialize to an appropriate value
            Country actual;
            actual = target.LoadCountry(name);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
