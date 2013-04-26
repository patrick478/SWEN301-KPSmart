using Server.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Common;
using System.Collections.Generic;

namespace ServerTests
{
    
    
    /// <summary>
    ///This is a test class for RouteServiceTest and is intended
    ///to contain all RouteServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RouteServiceTest
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


        private static RouteService routeService;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var currentState = new CurrentState();
            currentState.InitialiseRoutes(getRoutes());

            routeService = new RouteService(currentState);

        }



        private static IDictionary<int, Route> getRoutes()
        {
            Company company = new Company() { Name = "NZPost" };
            RouteNode origin = new DistributionCentre("Wellington");

            var routes = new Dictionary<int, Route>();

            // route1
            Route route1 = new Route(company, TransportType.Air, origin, new DistributionCentre("Auckland"));
            route1.AddDepartureTime(new WeeklyTime(DayOfWeek.Friday, 15, 0));
            route1.AddDepartureTime(new WeeklyTime(DayOfWeek.Wednesday, 5, 50));
            route1.ID = 1;
            routes[1] = route1;

            // route2
            Route route2 = new Route(company, TransportType.Land, origin, new DistributionCentre("Christchurch"));         
            route2.AddDepartureTime(new WeeklyTime(DayOfWeek.Monday, 15, 0));
            route2.ID = 2;
            routes[2] = route2;

            return routes;
        }


        /// <summary>
        ///A test for GetAll
        ///</summary>
        [TestMethod()]
        public void GetAllFromRouteNodeTest()
        {
            var expected = getRoutes();

            var actual = routeService.GetAll(new DistributionCentre("Wellington"));

            Assert.AreEqual(2, actual.Count);
        }
    }
}
