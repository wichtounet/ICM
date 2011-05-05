using ICM.Dao;
using ICM.Model;
using ICM.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace ICM_Tests
{
    /// <summary>
    ///This is a test class for PersonsDAOTest and is intended
    ///to contain all PersonsDAOTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PersonsDAOTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

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
        ///A test for CreatePerson
        ///</summary>
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("H:\\prog\\dev\\visual-studio\\ICM\\ICM", "/")]
        [UrlToTest("http://localhost:37234/")]
        public void CreatePersonTest()
        {
            var target = new PersonsDAO(); // TODO: Initialize to an appropriate value

            var name = "Test" + new Random().Next();
            var actual = target.CreatePerson("New", name, "076/482.04.78", "test@gmail.com", 4);

            Assert.IsTrue(actual > 0, "The id must be greater than 0");

            var created = target.GetPersonByID(actual);

            Assert.AreEqual(created.Name, name, "Values must be the same");
            Assert.AreEqual(created.FirstName, "New", "Values must be the same");
            Assert.AreEqual(created.Phone, "076/482.04.78", "Values must be the same");
            Assert.AreEqual(created.Email, "test@gmail.com", "Values must be the same");

            DBManager.GetInstance().Close();

            Assert.Inconclusive("Method executed correctly");
        }
    }
}
