using System.Linq;
using AutoMapper;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core.Testing;
using SAHL.UI.Halo.Shared;
using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.Halo;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan.Actions;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client;
using SAHL.Services.Interfaces.Halo.Models;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestMapHaloTileConfigurationToHaloTileConfigurationModel : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var repository         = Substitute.For<ITileConfigurationRepository>();
            var tileDataRepository = Substitute.For<ITileDataRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloTileConfigurationToHaloTileConfigurationModel(repository, tileDataRepository);
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
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<MapHaloTileConfigurationToHaloTileConfigurationModel>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Profile_ShouldEqualTypeName()
        {
            //---------------Set up test pack-------------------
            var repository         = Substitute.For<ITileConfigurationRepository>();
            var tileDataRepository = Substitute.For<ITileDataRepository>();
            var expectedName       = typeof(MapHaloTileConfigurationToHaloTileConfigurationModel).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloTileConfigurationToHaloTileConfigurationModel(repository, tileDataRepository);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedName, mapProfile.ProfileName);
        }

        [Test]
        public void Map_GivenHaloTileConfiguration_AndConvertToHaloTileConfigurationModel_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ClientRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => Mapper.Map<HaloTileConfigurationModel>(tileConfiguration));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Map_GivenHaloRootTileConfiguration_AndConvertToHaloTileConfigurationModel_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ClientRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => Mapper.Map<HaloTileConfigurationModel>(tileConfiguration));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Map_GivenHaloTileConfiguration_AndConvertToHaloTileConfigurationModel_ShouldMapAllProperties()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ClientRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var model = Mapper.Map<HaloTileConfigurationModel>(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.AreEqual(tileConfiguration.Name, model.Name);
            Assert.AreEqual(tileConfiguration.Sequence, model.Sequence);
            Assert.AreEqual(0, model.StartRow);
            Assert.AreEqual(0, model.StartColumn);
            Assert.AreEqual(0, model.NoOfRows);
            Assert.AreEqual(0, model.NoOfColumns);
            Assert.IsNull(model.BusinessContext);
            Assert.IsNull(model.TileData);
            Assert.IsNull(model.TileSubKeys);
        }

        [Test]
        public void Map_GivenHaloRootTileConfiguration_AndConvertToHaloTileConfigurationModel_ShouldMapAllProperties()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration  = new ClientRootTileConfiguration();
            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capabilities" });
            var businessContext    = new BusinessContext("context", GenericKeyType.Account, 0);
            var configurationModel = new HaloTileConfigurationModel
                {
                    BusinessContext = businessContext,
                    Role            = haloRoleModel,
                };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var model = Mapper.Map<IHaloSubTileConfiguration, HaloTileConfigurationModel>(tileConfiguration, configurationModel);
            //---------------Test Result -----------------------
            Assert.AreEqual(tileConfiguration.Name, model.Name);
            Assert.AreEqual(tileConfiguration.Sequence, model.Sequence);
            Assert.AreEqual(tileConfiguration.StartRow, model.StartRow);
            Assert.AreEqual(tileConfiguration.StartColumn, model.StartColumn);
            Assert.AreEqual(tileConfiguration.NoOfRows, model.NoOfRows);
            Assert.AreEqual(tileConfiguration.NoOfColumns, model.NoOfColumns);
            Assert.AreSame(businessContext, model.BusinessContext);
            Assert.IsNull(model.TileData);
            Assert.IsNull(model.TileSubKeys);
        }

        [Test]
        public void Map_GivenDynamicActionProviderExistsForTileConfiguration_ShouldMapDynamicTileActions()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration  = new MortgageLoanRootTileConfiguration();
            var haloRoleModel = new HaloRoleModel("area", "name", new string[] { "capabilities" });
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 0);
            var configurationModel = new HaloTileConfigurationModel
                {
                    BusinessContext = businessContext,
                    Role            = haloRoleModel,
                };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var model = Mapper.Map<IHaloRootTileConfiguration, HaloTileConfigurationModel>(tileConfiguration, configurationModel);
            //---------------Test Result -----------------------
            Assert.IsNotNull(model.TileActions);

            var wizardAction = new MortgageLoanCloseWizardAction();

            var foundAction = model.TileActions.FirstOrDefault(actionModel => actionModel.Name == wizardAction.Name);
            Assert.IsNotNull(foundAction);
            Assert.AreEqual("Wizard", foundAction.ActionType);
        }
    }
}
