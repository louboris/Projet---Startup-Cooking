using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace database_project_2020
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        // TEST CHANGEMENT D'USERNAME
        public void TestMethod1()
        {

            User utilisateur = new User("Boris", "Louchart", 767171485);
            utilisateur.username = "Test";

            Assert.AreEqual("Test", utilisateur.username);
        }
    }
}
