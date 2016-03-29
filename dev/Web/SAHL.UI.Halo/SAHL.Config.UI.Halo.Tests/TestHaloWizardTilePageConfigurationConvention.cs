using NUnit.Framework;
using SAHL.Config.Core;
using SAHL.Config.UI.Halo.Convention;
using SAHL.Core;
using SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice;
using SAHL.UI.Halo.Shared.Configuration;
using StructureMap;
using System.Linq;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloWizardTilePageConfigurationConvention
    {
        private IIocContainer iocContainer;

        [TestFixtureSetUp]
        public void Setup()
        {
            ObjectFactory.Initialize(expression =>
                {
                    expression.Scan(scanner =>
                        {
                            scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                            scanner.Convention<HaloWizardTilePageConfigurationConvention>();
                        });

                    expression.For<IIocContainer>().Use<StructureMapIocContainer>();
                });

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(this.iocContainer);
        }

        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var convention = new HaloWizardTilePageConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloWizardPageConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = this.iocContainer.GetAllInstances<IHaloWizardTilePageConfiguration<ThirdPartyInvoiceCaptureWizardConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(configurations.Any());
        }

        [Test]
        public void Process_ShouldRegisterThirdPartyInvoiceCapturePage1WizardAssociatedWithThirdPartyInvoiceCaptureWizardConfiguration()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = this.iocContainer.GetAllInstances<IHaloWizardTilePageConfiguration<ThirdPartyInvoiceCaptureWizardConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(configurations.Any());
            Assert.IsInstanceOf<ThirdPartyInvoiceCaptureWizardPageConfiguration>(configurations.OrderBy(configuration => configuration.Sequence)
                                                                                                .First());
        }
    }
}
