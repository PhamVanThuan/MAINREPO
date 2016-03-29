using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using SAHL.Core.Testing;
using SAHL.Services.Halo.Server.MapProfiles;
using SAHL.Services.Interfaces.Halo.Models.Configuration;
using SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Shared.Repository;
using System.Linq;

namespace SAHL.Services.Halo.Server.Tests
{
    [TestFixture]
    public class TestMapHaloWizardConfigurationToHaloWizardModel : TestQueryHandlerBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var wizardConfigurationRepository = Substitute.For<ITileWizardConfigurationRepository>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloWizardConfigurationToHaloWizardModel(wizardConfigurationRepository);
            //---------------Test Result -----------------------
            Assert.IsNotNull(mapProfile);
        }

        [TestCase("wizardConfigurationRepository")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<MapHaloWizardConfigurationToHaloWizardModel>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void Profile_ShouldEqualTypeName()
        {
            //---------------Set up test pack-------------------
            var wizardConfigurationRepository = Substitute.For<ITileWizardConfigurationRepository>();
            var expectedName = typeof(MapHaloWizardConfigurationToHaloWizardModel).Name;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var mapProfile = new MapHaloWizardConfigurationToHaloWizardModel(wizardConfigurationRepository);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedName, mapProfile.ProfileName);
        }

        [Test]
        public void Map_GivenWizardConfiguration_ShouldMapProperties()
        {
            //---------------Set up test pack-------------------
            var wizardConfiguration = new ThirdPartyInvoiceCaptureWizardConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloWizardModel = Mapper.Map<HaloWizardModel>(wizardConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(haloWizardModel);
            Assert.AreEqual(wizardConfiguration.Name, haloWizardModel.Name);
            Assert.AreEqual(wizardConfiguration.WizardType.ToString(), haloWizardModel.WizardType);
        }

        [Test]
        public void Map_GivenWizardConfigurationWithPages_ShouldMapPages()
        {
            //---------------Set up test pack-------------------
            var wizardConfiguration = new ThirdPartyInvoiceCaptureWizardConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloWizardModel = Mapper.Map<HaloWizardModel>(wizardConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(haloWizardModel);
            Assert.IsNotNull(haloWizardModel.WizardPages);
            Assert.IsTrue(haloWizardModel.WizardPages.Any());

            CollectionAssert.AllItemsAreInstancesOfType(haloWizardModel.WizardPages, typeof(HaloWizardPageModel));
            CollectionAssert.AllItemsAreNotNull(haloWizardModel.WizardPages);
        }

        [Test]
        public void Map_GivenWizardConfigurationWithPagesAndDataModel_ShouldMapPagesAndDataModel()
        {
            //---------------Set up test pack-------------------
            var wizardConfiguration = new ThirdPartyInvoiceCaptureWizardConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloWizardModel = Mapper.Map<HaloWizardModel>(wizardConfiguration);
            //---------------Test Result -----------------------
            var pageConfiguration = new ThirdPartyInvoiceCaptureWizardPageConfiguration();
            var haloWizardPageModel = haloWizardModel.WizardPages.First(model => model.Name == pageConfiguration.Name);
            Assert.IsNotNull(haloWizardPageModel);
            Assert.IsNotNull(haloWizardPageModel.ContentModel);
            Assert.IsInstanceOf<ThirdPartyInvoiceRootModel>(haloWizardPageModel.ContentModel);
        }
    }
}
