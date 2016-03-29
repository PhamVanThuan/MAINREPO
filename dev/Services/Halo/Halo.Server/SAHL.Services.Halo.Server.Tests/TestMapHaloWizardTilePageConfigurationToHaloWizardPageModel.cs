
using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using SAHL.Core.Testing;
using SAHL.Services.Halo.Server.MapProfiles;
using SAHL.Services.Interfaces.Halo.Models.Configuration;
using SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Repository;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestMapHaloWizardTilePageConfigurationToHaloWizardPageModel : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var wizardConfigurationRepository = Substitute.For<ITileWizardConfigurationRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloWizardTilePageConfigurationToHaloWizardPageModel(wizardConfigurationRepository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mapProfile);
        }

        [TestCase("wizardConfigurationRepository")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<MapHaloWizardTilePageConfigurationToHaloWizardPageModel>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Profile_ShouldEqualTypeName()
        {
            //---------------Set up test pack-------------------
            var wizardConfigurationRepository = Substitute.For<ITileWizardConfigurationRepository>();
            var expectedName = typeof(MapHaloWizardTilePageConfigurationToHaloWizardPageModel).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloWizardTilePageConfigurationToHaloWizardPageModel(wizardConfigurationRepository);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedName, mapProfile.ProfileName);
        }

        [Test]
        public void Map_GivenWizardConfiguration_ShouldMapProperties()
        {
            //---------------Set up test pack-------------------
            var pageConfiguration = new ThirdPartyInvoiceCaptureWizardPageConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloWizardModel = Mapper.Map<HaloWizardPageModel>(pageConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(haloWizardModel);
            Assert.AreEqual(pageConfiguration.Name, haloWizardModel.Name);
            Assert.AreEqual(pageConfiguration.WizardPageType.ToString(), haloWizardModel.WizardPageType);
            Assert.AreEqual(pageConfiguration.Sequence, haloWizardModel.Sequence);
            Assert.AreEqual(pageConfiguration.ContentMessage, haloWizardModel.ContentMessage);
        }

        [Test]
        public void Map_GivenDataModelAssociatedWithPage_ShouldSetPageStateAndContentModel()
        {
            //---------------Set up test pack-------------------
            var pageConfiguration = new ThirdPartyInvoiceCaptureWizardPageConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloWizardModel = Mapper.Map<HaloWizardPageModel>(pageConfiguration);
            //---------------Test Result -----------------------
            var pageState = new ThirdPartyInvoiceCaptureWizardPageState();

            Assert.AreEqual(pageState.GetType().FullName, haloWizardModel.PageState);
            Assert.IsNotNull(haloWizardModel.ContentModel);
            Assert.IsInstanceOf<ThirdPartyInvoiceRootModel>(haloWizardModel.ContentModel);
        }
    }
}
