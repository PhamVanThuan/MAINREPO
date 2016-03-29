using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using AutoMapper;

using SAHL.Core.Testing;
using SAHL.UI.Halo.Shared;
using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.Services.Interfaces.Halo.Models;
using SAHL.Services.Halo.Server.MapProfiles;
using SAHL.UI.Halo.Configuration.Client;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestMapHaloRootTileConfigurationToHaloRootTileConfigurationModel : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var repository         = Substitute.For<ITileConfigurationRepository>();
            var tileDataRepository = Substitute.For<ITileDataRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloRootTileConfigurationToHaloRootTileConfigurationModel(repository, tileDataRepository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mapProfile);
        }

        [TestCase("tileConfigurationRepository")]
        [TestCase("tileDataRepository")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<MapHaloRootTileConfigurationToHaloRootTileConfigurationModel>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Profile_ShouldEqualTypeName()
        {
            //---------------Set up test pack-------------------
            var repository         = Substitute.For<ITileConfigurationRepository>();
            var tileDataRepository = Substitute.For<ITileDataRepository>();
            var expectedName       = typeof(MapHaloRootTileConfigurationToHaloRootTileConfigurationModel).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloRootTileConfigurationToHaloRootTileConfigurationModel(repository, tileDataRepository);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedName, mapProfile.ProfileName);
        }

        [Test]
        public void Map_GivenHaloRootTileConfiguration_AndConvertToHaloRootTileConfigurationModel_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ClientRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => Mapper.Map<HaloRootTileConfigurationModel>(tileConfiguration));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Map_GivenRootTileConfiguration_ShouldSetRootTileConfiguration()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ClientRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var model = Mapper.Map<HaloRootTileConfigurationModel>(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.AreEqual(tileConfiguration.Name, model.RootTileConfigurations.First().Name);
            Assert.AreEqual(tileConfiguration.Sequence, model.RootTileConfigurations.First().Sequence);
            Assert.AreEqual(tileConfiguration.NoOfRows, model.RootTileConfigurations.First().NoOfRows);
            Assert.AreEqual(tileConfiguration.NoOfColumns, model.RootTileConfigurations.First().NoOfColumns);
        }

        [Test]
        public void Map_GivenRootTileConfiguration_ShouldLoadChildTileConfigurations()
        {
            //---------------Set up test pack-------------------
            var rootTileConfiguration = new ClientRootTileConfiguration();
            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capabilities" });
            var businessContext       = new BusinessContext("context", GenericKeyType.Account, 0);

            var rootTileConfigurationModel = new HaloRootTileConfigurationModel
                {
                    BusinessContext = businessContext,
                    Role            = haloRoleModel
                };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var model = Mapper.Map<IHaloRootTileConfiguration, HaloRootTileConfigurationModel>(rootTileConfiguration, rootTileConfigurationModel);
            //---------------Test Result -----------------------
            Assert.IsNotNull(model);
            Assert.IsTrue(model.ChildTileConfigurations.Any());
        }
    }
}
