using System;
using System.Linq;

using AutoMapper;

using NSubstitute;
using NUnit.Framework;

using SAHL.UI.Halo.Shared;
using SAHL.Services.Interfaces.Halo;
using SAHL.UI.Halo.MyHalo.Configuration.Home;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestMapHaloModuleConfigurationToHaloModuleModel : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<ITileConfigurationRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloModuleConfigurationToHaloModuleModel(repository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mapProfile);
        }

        [Test]
        public void Profile_ShouldEqualTypeName()
        {
            //---------------Set up test pack-------------------
            var repository   = Substitute.For<ITileConfigurationRepository>();
            var expectedName = typeof(MapHaloModuleConfigurationToHaloModuleModel).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloModuleConfigurationToHaloModuleModel(repository);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedName, mapProfile.ProfileName);
        }

        [Test]
        public void Map_GivenHaloModuleConfiguration_AndConvertToHaloModuleModel_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var homeModuleConfiguration = new HomeModuleConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => Mapper.Map<HaloModuleModel>(homeModuleConfiguration));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Map_GivenHaloModuleConfiguration_AndConvertToHaloModuleTileModel_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var homeModuleConfiguration = new HomeModuleConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => Mapper.Map<HaloModuleTileModel>(homeModuleConfiguration));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Map_GivenHaloModuleConfiguration_AndConvertToHaloModuleModel_ShouldMapAllProperties()
        {
            //---------------Set up test pack-------------------
            var homeModuleConfiguration = new HomeModuleConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloModuleModel = Mapper.Map<HaloModuleModel>(homeModuleConfiguration);
            //---------------Test Result -----------------------
            Assert.AreEqual(homeModuleConfiguration.Name, haloModuleModel.Name);
            Assert.AreEqual(homeModuleConfiguration.Sequence, haloModuleModel.Sequence);
            Assert.AreEqual(homeModuleConfiguration.IsTileBased, haloModuleModel.IsTileBased);
            Assert.AreEqual(homeModuleConfiguration.NonTilePageState, haloModuleModel.NonTilePageState);
        }

        [Test]
        public void Map_GivenHaloModuleConfiguration_AndConvertToHaloModuleTileModel_ShouldMapAllProperties()
        {
            //---------------Set up test pack-------------------
            var repository               = new TileConfigurationRepository(this.iocContainer);
            var applicationConfiguration = repository.FindApplicationConfiguration("Home");
            Assert.IsNotNull(applicationConfiguration);

            var moduleConfiguration = repository.FindModuleConfiguration(applicationConfiguration, "Clients");
            Assert.IsNotNull(moduleConfiguration);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloModuleModel = Mapper.Map<HaloModuleTileModel>(moduleConfiguration);
            //---------------Test Result -----------------------
            Assert.AreEqual(moduleConfiguration.Name, haloModuleModel.Name);
            Assert.AreEqual(moduleConfiguration.Sequence, haloModuleModel.Sequence);
            Assert.AreEqual(moduleConfiguration.IsTileBased, haloModuleModel.IsTileBased);
            Assert.AreEqual(moduleConfiguration.NonTilePageState, haloModuleModel.NonTilePageState);
            Assert.IsTrue(haloModuleModel.RootTileConfigurations.Any());
        }
    }
}
