using System.Collections.Generic;
using Server.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Common;
using Server.Data;

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



        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var countries = new Dictionary<int,Country> {{1, new Country {Name = "New Zealand", Code = "NZ", ID = 1}}};

            state = new CurrentState();
            state.InitialiseCountries(countries);
            state.InitialiseRouteNodes(new Dictionary<int, RouteNode>());
            service = new CountryService(state);

            new Database("test.db");

        }

        private static CountryService service;
        private static CurrentState state;

        /// <summary>
        ///A test for Exists
        ///</summary>
        [TestMethod()]
        public void ExistsTest()
        {
            Assert.IsTrue(service.Exists(new Country{Name="New Zealand"}));
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            service.Delete(1);
            Assert.IsTrue(state.GetAllCountries().Count == 0);
        }


    }
}
