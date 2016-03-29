using System;

using NUnit.Framework;
using SAHL.Services.Interfaces.Halo.Models;
using SAHL.Services.Interfaces.Halo.Queries;

namespace SAHL.Services.Interfaces.Tests
{
    [TestFixture]
    public class TestGetModuleConfigurationQuery
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetModuleConfigurationQuery("application", "module");
            //---------------Test Result -----------------------
            Assert.IsNotNull(query);
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            const string applicationName  = "application";
            const string moduleName       = "module";
            const string moduleParameters = "some parameters";

            var returnAllRoots = true;
            var role           = new HaloRoleModel("Area", "Role1",new string[]{ "capability"});
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetModuleConfigurationQuery(applicationName, moduleName, 
                                                        returnAllRoots: returnAllRoots, moduleParameters: moduleParameters, role: role);
            //---------------Test Result -----------------------
            Assert.AreEqual(applicationName, query.ApplicationName);
            Assert.AreEqual(moduleName, query.ModuleName);
            Assert.AreEqual(returnAllRoots, query.ReturnAllRoots);
            Assert.AreEqual(moduleParameters, query.ModuleParameters);
            Assert.AreEqual(role, query.Role);
        }

        [Test]
        public void Constructor_GivenEmptyApplicationName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new GetModuleConfigurationQuery(string.Empty, "module"));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("applicationName", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenEmptyModuleName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new GetModuleConfigurationQuery("application", string.Empty));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("moduleName", exception.ParamName);
        }
    }
}
