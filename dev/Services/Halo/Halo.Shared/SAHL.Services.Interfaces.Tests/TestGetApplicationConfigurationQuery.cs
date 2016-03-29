using System;

using NUnit.Framework;

using SAHL.Services.Interfaces.Halo;

namespace SAHL.Services.Interfaces.Tests
{
    [TestFixture]
    public class TestGetApplicationConfigurationQuery
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetApplicationConfigurationQuery("Home");
            //---------------Test Result -----------------------
            Assert.IsNotNull(query);
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "Home";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetApplicationConfigurationQuery(applicationName);
            //---------------Test Result -----------------------
            Assert.AreEqual(applicationName, query.ApplicationName);
        }

        [Test]
        public void Constructor_GivenEmptyApplicationName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new GetApplicationConfigurationQuery(string.Empty));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("applicationName", exception.ParamName);
        }
    }
}
