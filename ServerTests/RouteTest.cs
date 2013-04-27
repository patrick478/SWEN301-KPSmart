using System.Collections.Generic;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ServerTests
{
    
    
    /// <summary>
    ///This is a test class for RouteTest and is intended
    ///to contain all RouteTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RouteTest
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



        private Route getRoute()
        {
            Company company = new Company() {Name = "NZPost"};
            TransportType transportType = TransportType.Land;
            RouteNode origin = new DistributionCentre("Wellington");
            RouteNode destination = new DistributionCentre("Auckland");
            Route target = new Route(company, transportType, origin, destination);

            target.AddDepartureTime(new WeeklyTime(DayOfWeek.Friday, 15, 0));
            target.AddDepartureTime(new WeeklyTime(DayOfWeek.Wednesday, 5, 50));

            return target;
        }

        private Route getRoute2()
        {
            Company company = new Company() { Name = "NZPost" };
            TransportType transportType = TransportType.Sea;
            RouteNode origin = new DistributionCentre("Wellington");
            RouteNode destination = new DistributionCentre("Christchurch");
            Route target = new Route(company, transportType, origin, destination);

            target.AddDepartureTime(new WeeklyTime(DayOfWeek.Monday, 15, 0));

            return target;
        }


        /// <summary>
        ///A test for GetNextDeparture - if next departure is later in the week.
        ///</summary>
        [TestMethod()]
        public void GetNextDepartureTest1()
        {
            // create a new route
            Route target = getRoute();

            DateTime time = new DateTime(2013, 4, 22, 3, 30, 0); // MON 3.30am            
            DateTime expectedTime = new DateTime(2013, 4, 24, 5, 50, 0); // WED 5.50am

            // get the route instance
            RouteInstance routeInstance = target.GetNextDeparture(time);

            // make sure the date is correct
            Assert.AreEqual(expectedTime, routeInstance.DepartureTime);    
        }


        /// <summary>
        ///A test for GetNextDeparture - if next departure is later in the week.
        ///</summary>
        [TestMethod()]
        public void GetNextDepartureTest2()
        {
            // create a new route
            var target = getRoute();
          
            DateTime time = new DateTime(2013, 4, 26, 15, 30, 0); // FRI 3.30pm 
            DateTime expectedTime = new DateTime(2013, 5, 1, 5, 50, 0); // WED 5.50am next week

            // get the route instance
            RouteInstance routeInstance = target.GetNextDeparture(time);

            // make sure the date is correct
            Assert.AreEqual(expectedTime, routeInstance.DepartureTime);
        }


        [TestMethod()]
        public void RouteServiceGetAllTest()
        {
            var routes = new List<Route>() {getRoute(), getRoute2()};

            
        }


    }
}
