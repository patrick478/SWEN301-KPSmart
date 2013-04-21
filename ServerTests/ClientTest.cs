using Server.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Sockets;
using Common;

namespace ServerTests
{
    
    
    /// <summary>
    ///This is a test class for ClientTest and is intended
    ///to contain all ClientTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ClientTest
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
        ///A test for Client Constructor
        ///</summary>
        [TestMethod()]
        public void ClientConstructorTest()
        {
            int id = 0; // TODO: Initialize to an appropriate value
            Socket socket = null; // TODO: Initialize to an appropriate value
            Client target = new Client(id, socket);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for getPendingDelivery
        ///</summary>
        [TestMethod()]
        public void getPendingDeliveryTest()
        {
            int id = 0; // TODO: Initialize to an appropriate value
            Socket socket = null; // TODO: Initialize to an appropriate value
            Client target = new Client(id, socket); // TODO: Initialize to an appropriate value
            Priority priority = new Priority(); // TODO: Initialize to an appropriate value
            Delivery expected = null; // TODO: Initialize to an appropriate value
            Delivery actual;
            actual = target.getPendingDelivery(priority);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for storePendingDelivery
        ///</summary>
        [TestMethod()]
        public void storePendingDeliveryTest()
        {
            int id = 0; // TODO: Initialize to an appropriate value
            Socket socket = null; // TODO: Initialize to an appropriate value
            Client target = new Client(id, socket); // TODO: Initialize to an appropriate value
            Delivery air = null; // TODO: Initialize to an appropriate value
            Delivery standard = null; // TODO: Initialize to an appropriate value
            target.storePendingDelivery(air, standard);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ID
        ///</summary>
        [TestMethod()]
        public void IDTest()
        {
            int id = 0; // TODO: Initialize to an appropriate value
            Socket socket = null; // TODO: Initialize to an appropriate value
            Client target = new Client(id, socket); // TODO: Initialize to an appropriate value
            int actual;
            actual = target.ID;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Socket
        ///</summary>
        [TestMethod()]
        public void SocketTest()
        {
            int id = 0; // TODO: Initialize to an appropriate value
            Socket socket = null; // TODO: Initialize to an appropriate value
            Client target = new Client(id, socket); // TODO: Initialize to an appropriate value
            Socket actual;
            actual = target.Socket;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
