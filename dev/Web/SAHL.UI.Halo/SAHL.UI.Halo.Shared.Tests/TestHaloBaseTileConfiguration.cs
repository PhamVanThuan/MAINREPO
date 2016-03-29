using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using SAHL.UI.Halo.Configuration.Client;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Application;

namespace SAHL.UI.Halo.Shared.Tests
{
    [TestFixture]
    public class TestHaloBaseTileConfiguration
    {
        [Test]
        public void IsDynamicTile_GivenNonDynamicTile_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var configuration = new ClientRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isDynamicTile = configuration.IsDynamicTile();
            //---------------Test Result -----------------------
            Assert.IsFalse(isDynamicTile);
        }

        [Test]
        public void IsDynamicTile_GivenDynamicTile_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var configuration = new ApplicationRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isDynamicTile = configuration.IsDynamicTile();
            //---------------Test Result -----------------------
            Assert.IsTrue(isDynamicTile);
        }

        [Test]
        public void GetAllRoleNames_GivenTileWithNoRole_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var configuration = new NonRoleTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var allRoleNames = configuration.GetAllRoleNames();
            //---------------Test Result -----------------------
            Assert.IsNull(allRoleNames);
        }

        [Test]
        public void GetAllRoleNames_GivenTileWithRoles_ShouldReturnListOfRoleNames()
        {
            //---------------Set up test pack-------------------
            var configuration = new RoleTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var allRoleNames = configuration.GetAllRoleNames();
            //---------------Test Result -----------------------
            Assert.IsNotNull(allRoleNames);
            CollectionAssert.Contains(allRoleNames, "Role1");
            CollectionAssert.Contains(allRoleNames, "Role2");
        }

        [Test]
        public void IsInRole_GivenTileWithNoRole_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var configuration = new NonRoleTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isInRole = configuration.IsInRole("test");
            //---------------Test Result -----------------------
            Assert.IsTrue(isInRole);
        }

        [Test]
        public void IsInRole_GivenTileWithRole_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var configuration = new RoleTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isInRole = configuration.IsInRole("Role1");
            //---------------Test Result -----------------------
            Assert.IsTrue(isInRole);
        }

        [Test]
        public void IsInRole_GivenTileWithRoleNotSpecified_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var configuration = new RoleTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isInRole = configuration.IsInRole("NonRole");
            //---------------Test Result -----------------------
            Assert.IsFalse(isInRole);
        }

        [Test]
        public void IsInCapability_GivenTileWithNoRoleAndNoCapability_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var configuration = new NonRoleTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isInCapability = configuration.IsInCapability(new string[]{"test"});
            //---------------Test Result -----------------------
            Assert.IsTrue(isInCapability);
        }

        [Test]
        public void IsInCapability_GivenTileWithRoleAndCapabilitySpecified_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            var configuration = new RoleTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isInCapability = configuration.IsInCapability(new string[]{"Capability1"});
            //---------------Test Result -----------------------
            Assert.IsTrue(isInCapability);
        }

        [Test]
        public void IsInCapability_GivenTileWithRoleSpecifiedAndCapabilityNotSpecified_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var configuration = new RoleTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var isInCapability = configuration.IsInCapability(new string[]{"NonCapability"});
            //---------------Test Result -----------------------
            Assert.IsFalse(isInCapability);
        }

        private class NonRoleTileConfiguration : HaloBaseTileConfiguration
        {
            public NonRoleTileConfiguration() 
                : base("Non Role Tile", "NonRoleTile")
            {
            }
        }

        [HaloRole("Role1")]
        [HaloRole("Role2")]
        [HaloRole("Role3", "Capability1","Capability2")]
        private class RoleTileConfiguration : HaloBaseTileConfiguration
        {
            public RoleTileConfiguration() 
                : base("Role Tile", "RoleTile")
            {
            }
        }
    }
}
