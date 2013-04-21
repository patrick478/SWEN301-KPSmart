using Server.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Common;
using System.Collections.Generic;

namespace ServerTests
{
    
    
    /// <summary>
    ///This is a test class for RouteDataHelperTest and is intended
    ///to contain all RouteDataHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RouteDataHelperTest
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
        ///A test for RouteDataHelper Constructor
        ///</summary>
        [TestMethod()]
        public void RouteDataHelperConstructorTest()
        {
            RouteDataHelper target = new RouteDataHelper();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for DeleteRoute
        ///</summary>
        [TestMethod()]
        public void DeleteRouteTest()
        {
            RouteDataHelper target = new RouteDataHelper(); // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            target.DeleteRoute(id);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for EditRoute
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void EditRouteTest()
        {
            RouteDataHelper_Accessor target = new RouteDataHelper_Accessor(); // TODO: Initialize to an appropriate value
            Route route = null; // TODO: Initialize to an appropriate value
            Route expected = null; // TODO: Initialize to an appropriate value
            Route actual;
            actual = target.EditRoute(route);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadAllRoutes
        ///</summary>
        [TestMethod()]
        public void LoadAllRoutesTest()
        {
            RouteDataHelper target = new RouteDataHelper(); // TODO: Initialize to an appropriate value
            DateTime snapshotTime = new DateTime(); // TODO: Initialize to an appropriate value
            IDictionary<int, Route> expected = null; // TODO: Initialize to an appropriate value
            IDictionary<int, Route> actual;
            actual = target.LoadAllRoutes(snapshotTime);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadAllRoutes
        ///</summary>
        [TestMethod()]
        public void LoadAllRoutesTest1()
        {
            RouteDataHelper target = new RouteDataHelper(); // TODO: Initialize to an appropriate value
            IDictionary<int, Route> expected = null; // TODO: Initialize to an appropriate value
            IDictionary<int, Route> actual;
            actual = target.LoadAllRoutes();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadRoute
        ///</summary>
        [TestMethod()]
        public void LoadRouteTest()
        {
            RouteDataHelper target = new RouteDataHelper(); // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            Route expected = null; // TODO: Initialize to an appropriate value
            Route actual;
            actual = target.LoadRoute(id);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for NewRoute
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void NewRouteTest()
        {
            RouteDataHelper_Accessor target = new RouteDataHelper_Accessor(); // TODO: Initialize to an appropriate value
            Route route = null; // TODO: Initialize to an appropriate value
            Route expected = null; // TODO: Initialize to an appropriate value
            Route actual;
            actual = target.NewRoute(route);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SaveRoute
        ///</summary>
        [TestMethod()]
        public void SaveRouteTest()
        {
            RouteDataHelper target = new RouteDataHelper(); // TODO: Initialize to an appropriate value
            Route newRoute = null; // TODO: Initialize to an appropriate value
            Route expected = null; // TODO: Initialize to an appropriate value
            Route actual;
            actual = target.SaveRoute(newRoute);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
