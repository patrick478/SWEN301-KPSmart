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
        private static CompanyDataHelper dataHelper;

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
            dataHelper = new CompanyDataHelper();
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
            Database.Instance.ClearTable("companies");
            Database.Instance.ClearTable("events");
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            Database.Instance.ClearTable("companies");
            Database.Instance.ClearTable("events");
        }    
        #endregion


        /// <summary>
        ///A test for Load
        ///</summary>
        [TestMethod()]
        public void LoadTest()
        {
            dataHelper.Create(new Company { Name = "TestCompany" } );
            Company expected = dataHelper.Load(1);
        }

        public void LoadTest2()
        {
            long companyID = 1;
            DateTime created = DateTime.UtcNow;
            string name = "TestCorp";

            dataHelper.Create(new Company() { Name = name });
            var actual = dataHelper.Load(1);

            Assert.AreEqual(companyID, actual.ID);
            Assert.AreEqual(name, actual.Name);
            Assert.IsTrue(created.AddSeconds(1) > actual.LastEdited);
        }

        public void LoadTest3()
        {
            dataHelper.Create(new Company() { Name = "TestCorp" });
            dataHelper.Delete(1);
            var actual = dataHelper.Load(1);

            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            CompanyDataHelper target = new CompanyDataHelper(); // TODO: Initialize to an appropriate value
            Company company = null; // TODO: Initialize to an appropriate value
            target.Update(company);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LoadAll
        ///</summary>
        [TestMethod()]
        public void LoadAllTest()
        {
            CompanyDataHelper target = new CompanyDataHelper(); // TODO: Initialize to an appropriate value
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
        public void LoadAllTest1()
        {
            CompanyDataHelper target = new CompanyDataHelper(); // TODO: Initialize to an appropriate value
            IDictionary<int, Company> expected = null; // TODO: Initialize to an appropriate value
            IDictionary<int, Company> actual;
            actual = target.LoadAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetId
        ///</summary>
        [TestMethod()]
        public void GetIdTest()
        {
            CompanyDataHelper target = new CompanyDataHelper(); // TODO: Initialize to an appropriate value
            Company company = null; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.GetId(company);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            CompanyDataHelper target = new CompanyDataHelper(); // TODO: Initialize to an appropriate value
            Company obj = null; // TODO: Initialize to an appropriate value
            target.Delete(obj);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod()]
        public void DeleteTest1()
        {
            CompanyDataHelper target = new CompanyDataHelper(); // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            target.Delete(id);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            CompanyDataHelper target = new CompanyDataHelper(); // TODO: Initialize to an appropriate value
            Company company = null; // TODO: Initialize to an appropriate value
            target.Create(company);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
