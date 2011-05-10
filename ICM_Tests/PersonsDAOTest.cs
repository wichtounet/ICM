using System.Data;
using ICM.Dao;
using ICM.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ICM_Tests
{
    /// <summary>
    ///This is a test class for PersonsDAOTest and is intended
    ///to contain all PersonsDAOTest Unit Tests
    ///</summary>
    [TestClass]
    public class PersonsDAOTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        private static int department;

        /// <summary>
        ///Run before the first test
        ///</summary>
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            var institutions = new InstitutionsDAO().GetInstitutionsClean();

            if(institutions.Count == 0)
            {
                Assert.Fail("This unit test need the institution table to contains at least one element");
            } 
            else
            {
                var inst = institutions[0];

                if (inst.Departments.Count == 0)
                {
                    Assert.Fail("This unit test need the department table to contains at least one element");
                } 
                else
                {
                    department = inst.Departments[0].Id;
                }
            }
        }

        /// <summary>
        ///A test for CreatePerson
        ///</summary>
        [TestMethod]
        public void CreatePersonTest()
        {
            var target = new PersonsDAO();

            var name = "Test" + new Random().Next();
            var actual = target.CreatePerson("New", name, "076/482.04.78", "test@gmail.com", department);

            Assert.IsTrue(actual > 0, "The id must be greater than 0");

            var created = target.GetPersonByID(actual);

            Assert.AreEqual(name, created.Name, "Values must be the same");
            Assert.AreEqual("New", created.FirstName, "Values must be the same");
            Assert.AreEqual("076/482.04.78", created.Phone, "Values must be the same");
            Assert.AreEqual("test@gmail.com", created.Email, "Values must be the same");
            Assert.AreEqual(department, created.Department.Id, "Values must be the same");
            Assert.AreEqual(false, created.Archived, "Values must be the same");
        }

        /// <summary>
        ///A test for ArchivePerson
        ///</summary>
        [TestMethod]
        public void ArchivePersonTest()
        {
            var target = new PersonsDAO();

            var actual = target.CreatePerson("Test" + new Random().Next(), "Test" + new Random().Next(), "076/482.04.78", "test@gmail.com", department);

            target.ArchivePerson(actual);

            var person = target.GetPersonByID(actual);

            Assert.IsNotNull(person, "Person must not be null");
            Assert.AreEqual(true, person.Archived, "The person must be archived");
        }

        /// <summary>
        ///A test for GetPersonByID
        ///</summary>
        [TestMethod]
        public void GetPersonByIDTest()
        {
            var target = new PersonsDAO();

            var actual = target.GetPersonByID(-1);
            Assert.IsNull(actual, "This person must not exists");

            //This method is already tested for correctness in CreatePersonTest
        }

        /// <summary>
        ///A test for SavePerson
        ///</summary>
        [TestMethod]
        public void SavePersonTest()
        {
            var target = new PersonsDAO(); // TODO: Initialize to an appropriate value

            var actual = target.CreatePerson("Test" + new Random().Next(), "Test" + new Random().Next(), "076/482.04.78", "test@gmail.com", department);

            var firstname = "Test" + new Random().Next();
            var name = "Test" + new Random().Next();
            var phone = "076/482.04.79";
            var email = "test@hotmail.com";

            using(var connection = DBManager.GetInstance().GetNewConnection())
            {
                var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                target.SavePerson(actual, firstname, name, phone, email, department, transaction);

                transaction.Commit();
            }

            var result = target.GetPersonByID(actual);

            Assert.AreEqual(name, result.Name, "Values must be the same");
            Assert.AreEqual(firstname, result.FirstName, "Values must be the same");
            Assert.AreEqual(phone, result.Phone, "Values must be the same");
            Assert.AreEqual(email, result.Email, "Values must be the same");
        }
    }
}