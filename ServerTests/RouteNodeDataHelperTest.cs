using Server.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Common;

namespace ServerTests
{
    
    
    /// <summary>
    ///This is a test class for RouteNodeDataHelperTest and is intended
    ///to contain all RouteNodeDataHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RouteNodeDataHelperTest
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
            countryDataHelper = new CountryDataHelper();
            routeNodeDataHelper = new RouteNodeDataHelper();

            nz = new Country { Name = "New Zealand", Code = "NZ" };
            aus = new Country { Name = "Australia", Code = "AUS" };
            db.ClearTable("countries");

            countryDataHelper.Create(nz);
            countryDataHelper.Create(aus);           
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
            Database.Instance.ClearTable("route_nodes");
            Database.Instance.ClearTable("events");
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Database.Instance.ClearTable("route_nodes");
            Database.Instance.ClearTable("events");
            Database.Instance.ClearTable("countries");
        }
        #endregion

        private static RouteNodeDataHelper routeNodeDataHelper;
        private static CountryDataHelper countryDataHelper;
        public static Country nz;
        public static Country aus;

        /// <summary>
        ///A test for Load - test it doesn't throw an error
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void LoadTest()
        {
            // create the route
            InternationalPort testRoute = new InternationalPort(aus);
            routeNodeDataHelper.Create(testRoute);

            // load
            var actual = routeNodeDataHelper.Load(1);

            Assert.AreEqual(testRoute,actual);
        }

        /// <summary>
        ///A test for Load - all fields are correct
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void LoadTestForInternationalPort()
        {
            // create the route
            RouteNode testRoute = new InternationalPort(nz);
            routeNodeDataHelper.Create(testRoute);

            // load
            var actual = routeNodeDataHelper.Load(1);

            // check values
            Assert.AreEqual(1, actual.ID);
            Assert.IsNotNull(actual.LastEdited);
            Assert.AreEqual(testRoute.GetType(), actual.GetType());
            Assert.AreEqual(testRoute.Country, actual.Country);
        }


        /// <summary>
        ///A test for Load - test it doesn't throw an error
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void LoadTestForDistributionCentre()
        {
            // create the route
            DistributionCentre testRoute = new DistributionCentre("Hamilton");
            routeNodeDataHelper.Create(testRoute);

            // load
            var actual = (DistributionCentre)routeNodeDataHelper.Load(1);

            // check values
            Assert.AreEqual(1, actual.ID);
            Assert.IsNotNull(actual.LastEdited);
            Assert.AreEqual(testRoute.GetType(), actual.GetType());
            Assert.AreEqual(testRoute.Country, actual.Country);
            Assert.AreEqual(testRoute.Name, actual.Name);
        }


        /// <summary>
        ///A test for Create - runs without failing and events work
        ///</summary>
        [TestMethod()]
        public void CreateTest1InternationalPort()
        {
            var internationalport = new InternationalPort(aus);
            
            routeNodeDataHelper.Create(internationalport);
            VerifyEvent(EventType.Create);
            VerifyNumberOfEvents(1);
        }

        /// <summary>
        ///A test for Create - runs without failing and events work
        ///</summary>
        [TestMethod()]
        public void CreateTest1DistributionCentre()
        {
            var distributionCentre = new DistributionCentre("Christchurch");

            routeNodeDataHelper.Create(distributionCentre);
            VerifyEvent(EventType.Create);
            VerifyNumberOfEvents(1);
        }

        /// <summary>
        ///A test for Create - saves right values
        ///</summary>
        [TestMethod()]
        public void CreateTest2InternationalPort()
        {
            long routeNodeId = 1;
            object created = DateTime.UtcNow;
            int active = 1;
            string name = "";
            long countryId = 2;

            routeNodeDataHelper.Create(new InternationalPort(aus));

            var expected = new object[] { 0, 0, routeNodeId, created, active, countryId, name };
            var actual = Database.Instance.GetLastRows("route_nodes", 1)[0];

            AssertRowValuesMatch(expected, actual);
        }

        /// <summary>
        ///A test for Create - saves right values
        ///</summary>
        [TestMethod()]
        public void CreateTest2DistributionCentre()
        {
            long routeNodeId = 1;
            object created = DateTime.UtcNow;
            int active = 1;
            string name = "Christchurch";
            long countryId = 1;

            routeNodeDataHelper.Create(new DistributionCentre("Christchurch"));

            var expected = new object[] { 0, 0, routeNodeId, created, active, countryId, name };
            var actual = Database.Instance.GetLastRows("route_nodes", 1)[0];

            AssertRowValuesMatch(expected, actual);
        }


        /// <summary>
        ///A test for Create - fails if same name and code already exists. Also checks that event isn't saved if fails
        ///</summary>
        [TestMethod()]
        public void CreateFailsIfAlreadyExistsInternationalPort()
        {
            var distributionCentre = new InternationalPort(aus);

            routeNodeDataHelper.Create(distributionCentre);
            try
            {
                distributionCentre = new InternationalPort(aus);
                routeNodeDataHelper.Create(distributionCentre);
            }
            catch (DatabaseException e)
            {
                VerifyEvent(EventType.Create);
                VerifyNumberOfEvents(1);
            }
        }

        /// <summary>
        ///A test for Create - fails if same name already exists.
        ///</summary>
        [TestMethod()]
        public void CreateFailsIfAlreadyExistsDistributionCentre()
        {
            var distributionCentre = new DistributionCentre("Christchurch");


            routeNodeDataHelper.Create(distributionCentre);
            try
            {
                distributionCentre = new DistributionCentre("Christchurch");
                routeNodeDataHelper.Create(distributionCentre);
            }
            catch (DatabaseException e)
            {
                VerifyEvent(EventType.Create);
                VerifyNumberOfEvents(1);
            }
        }


        /// <summary>
        ///A test for Delete - it runs without failing
        ///</summary>
        [TestMethod()]
        public void DeleteTestInternationalPort()
        {
            routeNodeDataHelper.Create(new InternationalPort(aus));
            routeNodeDataHelper.Delete(1);
            VerifyEvent(EventType.Delete);
            VerifyNumberOfEvents(2);
        }

        /// <summary>
        ///A test for Delete - it runs without failing
        ///</summary>
        [TestMethod()]
        public void DeleteTestDistributionCentre()
        {
            routeNodeDataHelper.Create(new DistributionCentre("Christchurch"));
            routeNodeDataHelper.Delete(1);
            VerifyEvent(EventType.Delete);
            VerifyNumberOfEvents(2);
        }


        /// <summary>
        ///A test for Delete - loading it after deleting returns null.
        ///</summary>
        [TestMethod()]
        public void DeleteTest2()
        {
            routeNodeDataHelper.Create(new DistributionCentre("Christchurch"));
            routeNodeDataHelper.Delete(1);

            Assert.IsNull(routeNodeDataHelper.Load(1));
        }

        /// <summary>
        ///A test for Delete 
        ///</summary>
        [TestMethod()]
        public void DeleteFailsIfNoCountryWithMatchingIdActive()
        {
            try
            {
                routeNodeDataHelper.Delete(1);
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
        public void GetIdTestInternationalPort()
        {
            routeNodeDataHelper.Create(new InternationalPort(aus));

            var actual = routeNodeDataHelper.GetId(new InternationalPort(aus));

            Assert.AreEqual(1, actual);
        }

        /// <summary>
        ///A test for GetId
        ///</summary>
        [TestMethod()]
        public void GetIdTestDistributionCentre()
        {
            routeNodeDataHelper.Create(new DistributionCentre("Christchurch"));

            var actual = routeNodeDataHelper.GetId(new DistributionCentre("Christchurch"));

            Assert.AreEqual(1, actual);
        }


        /// <summary>
        ///A test for LoadAll - runs without failing
        ///</summary>
        [TestMethod()]
        public void LoadAllTest()
        {
            routeNodeDataHelper.Create(new InternationalPort(aus));
            routeNodeDataHelper.Create(new DistributionCentre("Christchurch"));

            var result = routeNodeDataHelper.LoadAll();

            Assert.IsTrue(result.Keys.Count == 2);
        }

        /// <summary>
        ///A test for LoadAll - runs when table is empty
        ///</summary>
        [TestMethod()]
        public void LoadAllTest1()
        {
            var result = routeNodeDataHelper.LoadAll();

            Assert.IsTrue(result.Keys.Count == 0);
        }

        /// <summary>
        ///A test for LoadAll - values are correct
        ///</summary>
        [TestMethod()]
        public void LoadAllTest2()
        {
            routeNodeDataHelper.Create(new InternationalPort(aus));
            routeNodeDataHelper.Create(new DistributionCentre("Christchurch"));

            var result = routeNodeDataHelper.LoadAll();

            var country1 = result[1] as InternationalPort;
            var country2 = result[2] as DistributionCentre;

            // international port
            Assert.AreEqual(1, country1.ID);
            Assert.AreEqual(aus, country1.Country);
            Assert.IsTrue(country1.LastEdited.AddSeconds(10) > DateTime.UtcNow);

            Assert.AreEqual(2, country2.ID);
            Assert.AreEqual("Christchurch", country2.Name);
            Assert.AreEqual(nz, country2.Country);
            Assert.IsTrue(country2.LastEdited.AddSeconds(10) > DateTime.UtcNow);
        }

        /// <summary>
        /// Super awesome private method to help verify that an update was correct.
        /// 
        /// It even checks DateTime!
        /// Give it DateTime.UtcNow for the 'created' field, and it will verify it happened within a second ago.  
        /// Otherwise give it null, and it won't check the date at all.
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
                        Assert.AreEqual(valExpected.GetType(), actual[i].GetType(), String.Format("Field {0} is not correct. Expected={1}, Actual={2}", i+1, valExpected.GetType(), valActual.GetType()) );

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

            var expected = new object[] { null, null, "RouteNode", eventType.ToString() };

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
