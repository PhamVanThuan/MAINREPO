using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Interfaces.Tests
{
    [TestFixture]
    public class TestHaloRoleModel
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var organisationArea = Guid.NewGuid().ToString();
            var roleName         = Guid.NewGuid().ToString();
            var capabilities = new string[] { Guid.NewGuid().ToString() };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloRoleModel = new HaloRoleModel(organisationArea, roleName, capabilities);
            //---------------Test Result -----------------------
            Assert.IsNotNull(haloRoleModel);
        }

        [Test]
        public void Constructor_GivenValues_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            var organisationArea = Guid.NewGuid().ToString();
            var roleName = Guid.NewGuid().ToString();
            var capabilities = new string[] { Guid.NewGuid().ToString() };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloRoleModel = new HaloRoleModel(organisationArea, roleName, capabilities);
            //---------------Test Result -----------------------
            Assert.AreEqual(organisationArea, haloRoleModel.OrganisationArea);
            Assert.AreEqual(roleName, haloRoleModel.RoleName);
            Assert.AreEqual(capabilities, haloRoleModel.Capabilities);
        }
    }
}
