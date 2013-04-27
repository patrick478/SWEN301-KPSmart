using System.Collections.Generic;
using Common;
using Server.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ServerTests
{
    
    
    /// <summary>
    ///This is a test class for PathFinderTest and is intended
    ///to contain all PathFinderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PathFinderTest
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


        private static PathFinder pathFinder;


        /// <summary>
        /// Use ClassInitialize to run code before running the first test in the class
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            // initialise state
            CurrentState state = new CurrentState();
            state.InitialiseRoutes(getRoutes());

            // initialise routeService
            var routeService = new RouteService(state);

            // initialise pathfinder
            pathFinder = new PathFinder(routeService);
        }


        /// <summary>
        /// This is where you initialise the routes collection josh.
        /// </summary>
        /// <returns></returns>
        private static IDictionary<int, Route> getRoutes()
        {
            //todo!
            return null;
        }


        /// <summary>
        ///A test for PathFinder Constructor
        ///</summary>
        [TestMethod()]
        public void findRoutesTest()
        {
            //todo!



            Assert.AreEqual("expected", "actual");

        }
    }
}
