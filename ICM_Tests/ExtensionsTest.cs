using ICM.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICM.Model;
using System.Collections.Generic;

namespace ICM_Tests
{
    /// <summary>
    ///This is a test class for ExtensionsTest and is intended
    ///to contain all ExtensionsTest Unit Tests
    ///</summary>
    [TestClass]
    public class ExtensionsTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        ///A test for ContainsDepartmentWithName
        ///</summary>
        [TestMethod]
        public void ContainsDepartmentWithNameTest()
        {
            var departments = new List<Department>
            {
                new Department {Name = "Test"},
                new Department {Name = "youhou"},
                new Department {Name = "yeepee"}
            };

            Assert.IsTrue(departments.ContainsDepartmentWithName("Test"), "Test is contained in the list");
            Assert.IsFalse(departments.ContainsDepartmentWithName("ohayo"), "ohayo is not contained in the list");
            Assert.IsFalse(departments.ContainsDepartmentWithName(""), "no empty name is contained in the list");

            departments.Clear();

            Assert.IsFalse(departments.ContainsDepartmentWithName("Test"), "Must work with empty list");
        }

        /// <summary>
        ///A test for GetUserByLogin
        ///</summary>
        [TestMethod]
        public void GetUserByLoginTest()
        {
            var users = new List<User>
            {
                new User {Login = "Test"},
                new User {Login = "youhou"},
                new User {Login = "yeepee"}
            };

            var user = users.GetUserByLogin("Test");
            Assert.IsNotNull(user);
            Assert.AreEqual(user.Login, "Test", "Login is not equal");

            Assert.IsNull(users.GetUserByLogin("Null"));

            users.Clear();

            Assert.IsNull(users.GetUserByLogin("Test"), "Must work with empty list");
        }

        /// <summary>
        ///A test for ToInt
        ///</summary>
        [TestMethod]
        public void ToIntTest()
        {
            Assert.AreEqual(1, "1".ToInt());
            Assert.AreEqual(-1, "-1".ToInt());
            Assert.AreEqual(1024, "1024".ToInt());
            Assert.AreEqual(-999999, "-999999".ToInt());
        }

        /// <summary>
        ///A test for ToIntOrDefault
        ///</summary>
        [TestMethod]
        public void ToIntOrDefaultTest()
        {
            Assert.AreEqual(1, "1".ToIntOrDefault());
            Assert.AreEqual(-1, "-1".ToIntOrDefault());
            Assert.AreEqual(1024, "1024".ToIntOrDefault());
            Assert.AreEqual(-999999, "-999999".ToIntOrDefault());
            Assert.AreEqual(-1, "".ToIntOrDefault());
        }
    }
}
