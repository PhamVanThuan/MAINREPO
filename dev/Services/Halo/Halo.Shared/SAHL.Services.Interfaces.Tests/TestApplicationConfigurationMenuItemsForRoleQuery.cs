using System;

using NUnit.Framework;

using SAHL.Services.Interfaces.Halo;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Interfaces.Tests
{
    [TestFixture]
    public class TestApplicationConfigurationMenuItemsForRoleQuery
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetApplicationConfigurationMenuItemsQuery("application", new HaloRoleModel("area", "role", null));
            //---------------Test Result -----------------------
            Assert.IsNotNull(query);
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "application";
            var haloRoleModel            = new HaloRoleModel("area", "role", null);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetApplicationConfigurationMenuItemsQuery(applicationName, haloRoleModel);
            //---------------Test Result -----------------------
            Assert.AreEqual(applicationName, query.ApplicationName);
            Assert.AreEqual(haloRoleModel, query.RoleModel);
        }

        [Test]
        public void Constructor_GivenEmptyApplicationName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new GetApplicationConfigurationMenuItemsQuery(string.Empty, null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("applicationName", exception.ParamName);
        }
    }
}
