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
        private static List<RouteNode> routeNodes;


        /// <summary>
        /// Use ClassInitialize to run code before running the first test in the class
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            // initialise state
            CurrentState state = new CurrentState();
            routeNodes = new List<RouteNode>();
            routeNodes.Add(new DistributionCentre("Auckland"));
            routeNodes.Add(new DistributionCentre("Wellington"));
            routeNodes.Add(new DistributionCentre("Christchurch"));
            state.InitialiseRoutes(getRoutes(routeNodes));

            // initialise routeService
            var routeService = new RouteService(state);

            // initialise pathfinder
            pathFinder = new PathFinder(routeService);
        
            
        }


        /// <summary>
        /// This is where the initialisation of the routes collection happens.
        /// </summary>
        /// <returns></returns>
        private static IDictionary<int, Route> getRoutes(List<RouteNode> routeNodes)
        {
            Company company = new Company() { Name = "NZPost" };

            var routes = new Dictionary<int, Route>();

            Route airChchWell = new Route { Company = company, TransportType = TransportType.Air, Origin = routeNodes[0], Destination = routeNodes[1] };
            airChchWell.AddDepartureTime(new WeeklyTime(DayOfWeek.Monday, 12, 0));
            airChchWell.Duration = 60;
            airChchWell.CostPerCm3 = 0;
            airChchWell.CostPerGram = 2;
            airChchWell.ID = 1;
            routes[1] = airChchWell;

            Route landChchWell = new Route { Company = company, TransportType = TransportType.Land, Origin = routeNodes[0], Destination = routeNodes[1] };
            landChchWell.AddDepartureTime(new WeeklyTime(DayOfWeek.Tuesday, 12, 0));
            landChchWell.Duration = 60;
            landChchWell.CostPerCm3 = 0;
            landChchWell.CostPerGram = 1;
            landChchWell.ID = 2;
            routes[2] = landChchWell;

            Route airWellAuck = new Route { Company = company, TransportType = TransportType.Air, Origin = routeNodes[1], Destination = routeNodes[2] };
            airWellAuck.AddDepartureTime(new WeeklyTime(DayOfWeek.Thursday, 12, 0));
            airWellAuck.Duration = 60;
            airWellAuck.CostPerCm3 = 0;
            airWellAuck.CostPerGram = 3;
            airWellAuck.ID = 3;
            routes[3] = airWellAuck;

            Route landWellAuck = new Route { Company = company, TransportType = TransportType.Land, Origin = routeNodes[1], Destination = routeNodes[2] };
            landWellAuck.AddDepartureTime(new WeeklyTime(DayOfWeek.Wednesday, 12, 0));
            landWellAuck.Duration = 60;
            landWellAuck.CostPerCm3 = 0;
            landWellAuck.CostPerGram = 4;
            landWellAuck.ID = 4;
            routes[3] = landWellAuck;

            return routes;
        }


        /// <summary>
        ///A test for PathFinder Constructor
        ///</summary>
        [TestMethod()]
        public void findRoutesTest()
        {
            /*
            Dictionary<PathType, Delivery> options = pathFinder.findRoutes(routeNodes[0], routeNodes[2], 1, 1);

            //Expected cost = 5, time = 
            Assert.AreEqual("expected", options[PathType.AirExpress].TotalCost);
            Assert.AreEqual("expected", options[PathType.AirExpress]);

            //Expected cost = 5, time = 
            Assert.AreEqual("expected", options[PathType.AirStandard].TotalCost);
            Assert.AreEqual("expected", options[PathType.AirStandard]);

            //Expected cost = 5, time = 
            Assert.AreEqual("expected", options[PathType.Express].TotalCost);
            Assert.AreEqual("expected", options[PathType.Express]);

            //Expected cost = 4, time = 
            Assert.AreEqual("expected", options[PathType.Standard].TotalCost);
            Assert.AreEqual("expected", options[PathType.Standard]);
            */
        }
    }
}
