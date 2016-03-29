using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using AutoMapper;

using SAHL.UI.Halo.Shared;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.Services.Halo.Server.MapProfiles;
using SAHL.UI.Halo.Configuration.Client;
using SAHL.Services.Interfaces.Halo.Models.Configuration;
using SAHL.UI.Halo.Models.Client;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestMapHaloTileHeaderToHaloTileHeaderModel : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var repository = Substitute.For<ITileConfigurationRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloTileHeaderToHaloTileHeaderModel(repository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mapProfile);
        }

        [Test]
        public void Profile_ShouldEqualTypeName()
        {
            //---------------Set up test pack-------------------
            var repository   = Substitute.For<ITileConfigurationRepository>();
            var expectedName = typeof(MapHaloTileHeaderToHaloTileHeaderModel).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloTileHeaderToHaloTileHeaderModel(repository);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedName, mapProfile.ProfileName);
        }

        [Test]
        public void Map_GivenHaloTileHeader_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var tileHeader = new ClientRootTileHeaderConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => Mapper.Map<HaloTileHeaderModel>(tileHeader));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Map_GivenTileHeader_AndNoDataModel_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var tileHeader  = new ClientRootTileHeaderConfiguration();
            var headerModel = new HaloTileHeaderModel
                {
                    Data = null,
                };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => Mapper.Map<IHaloTileHeader, HaloTileHeaderModel>(tileHeader, headerModel));
            //---------------Test Result -----------------------
        }

        [Test]
        public void Map_GivenTileHeader_AndDataModel_ShouldRetrieveAndSetText()
        {
            //---------------Set up test pack-------------------
            var tileHeader      = new ClientRootTileHeaderConfiguration();
            var clientRootModel = this.CreateClientRootModel();
            var headerModel     = new HaloTileHeaderModel
                {
                    Data = clientRootModel,
                };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var model = Mapper.Map<IHaloTileHeader, HaloTileHeaderModel>(tileHeader, headerModel);
            //---------------Test Result -----------------------
            var textProvider = new ClientRootTileHeaderTextProvider();
            textProvider.Execute(clientRootModel);
            Assert.IsNotNull(model.Text);
            Assert.AreEqual(textProvider.HeaderText, model.Text);
        }

        private ClientRootModel CreateClientRootModel()
        {
            var clientRootModel = new ClientRootModel
                {
                    LegalName = "Test Name",
                };
            return clientRootModel;
        }
    }
}
