using Server.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Common;
using System.Collections.Generic;

namespace ServerTests
{
    
    
    /// <summary>
    ///This is a test class for CompanyDataHelperTest and is intended
    ///to contain all CompanyDataHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CompanyDataHelperTest
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
        ///A test for CompanyDataHelper Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void CompanyDataHelperConstructorTest()
        {
            CompanyDataHelper_Accessor target = new CompanyDataHelper_Accessor();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void CreateTest()
        {
            CompanyDataHelper_Accessor target = new CompanyDataHelper_Accessor(); // TODO: Initialize to an appropriate value
            Company Company = null; // TODO: Initialize to an appropriate value
            target.Create(Company);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void DeleteTest()
        {
            CompanyDataHelper_Accessor target = new CompanyDataHelper_Accessor(); // TODO: Initialize to an appropriate value
            Company obj = null; // TODO: Initialize to an appropriate value
            target.Delete(obj);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void DeleteTest1()
        {
            CompanyDataHelper_Accessor target = new CompanyDataHelper_Accessor(); // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            target.Delete(id);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetId
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void GetIdTest()
        {
            CompanyDataHelper_Accessor target = new CompanyDataHelper_Accessor(); // TODO: Initialize to an appropriate value
            Company obj = null; // TODO: Initialize to an appropriate value
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
        [DeploymentItem("Server.exe")]
        public void LoadTest()
        {
            CompanyDataHelper_Accessor target = new CompanyDataHelper_Accessor(); // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            Company expected = null; // TODO: Initialize to an appropriate value
            Company actual;
            actual = target.Load(id);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadAll
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void LoadAllTest()
        {
            CompanyDataHelper_Accessor target = new CompanyDataHelper_Accessor(); // TODO: Initialize to an appropriate value
            DateTime snapshotTime = new DateTime(); // TODO: Initialize to an appropriate value
            IDictionary<int, Company> expected = null; // TODO: Initialize to an appropriate value
            IDictionary<int, Company> actual;
            actual = target.LoadAll(snapshotTime);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for LoadAll
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void LoadAllTest1()
        {
            CompanyDataHelper_Accessor target = new CompanyDataHelper_Accessor(); // TODO: Initialize to an appropriate value
            IDictionary<int, Company> expected = null; // TODO: Initialize to an appropriate value
            IDictionary<int, Company> actual;
            actual = target.LoadAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Server.exe")]
        public void UpdateTest()
        {
            CompanyDataHelper_Accessor target = new CompanyDataHelper_Accessor(); // TODO: Initialize to an appropriate value
            Company Company = null; // TODO: Initialize to an appropriate value
            target.Update(Company);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
