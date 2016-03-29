using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.Halo.Models;
using SAHL.Services.Interfaces.Halo.Queries;

namespace SAHL.Services.Interfaces.Tests
{
    [TestFixture]
    public class TestGetRootTileConfigurationQuery
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 0);
            var haloRoleModel   = new HaloRoleModel("area", "name", new string[]{"capabilities"});
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetRootTileConfigurationQuery("application", "module", "root tile", businessContext, haloRoleModel);
            //---------------Test Result -----------------------
            Assert.IsNotNull(query);
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "application";
            const string moduleName      = "module";
            const string rootTileName    = "root tile";

            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capability" });
            var businessKey   = new BusinessContext("context", GenericKeyType.Account, 0);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var query = new GetRootTileConfigurationQuery(applicationName, moduleName, rootTileName, businessKey, haloRoleModel);
            //---------------Test Result -----------------------
            Assert.AreEqual(applicationName, query.ApplicationName);
            Assert.AreEqual(moduleName, query.ModuleName);
            Assert.AreEqual(rootTileName, query.RootTileName);
            Assert.AreSame(businessKey, query.BusinessContext);
        }

        [Test]
        public void Constructor_GivenNullApplicationName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var haloRoleModel   = new HaloRoleModel("area", "name", new string[]{ "capabilities"});
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 0);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new GetRootTileConfigurationQuery(null, "module", "rootTile", businessContext, haloRoleModel));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("applicationName", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenNullModuleName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capabilities" });
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 0);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new GetRootTileConfigurationQuery("application", null, "rootTile", businessContext, haloRoleModel));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("moduleName", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenNullRootTileNameName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capabilities" });
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 0);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new GetRootTileConfigurationQuery("application", "module", null, businessContext, haloRoleModel));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("rootTileName", exception.ParamName);
        }
    }
}
