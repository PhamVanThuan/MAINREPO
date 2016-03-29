using System.Linq;

using StructureMap;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.Config.UI.Halo.Convention;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloWizardTileConfigurationConvention
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
                            scanner.Convention<HaloWizardTileConfigurationConvention>();
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
            var convention = new HaloWizardTileConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloWizardTileConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = this.iocContainer.GetAllInstances<IHaloWizardTileConfiguration>();
            //---------------Test Result -----------------------
            Assert.IsTrue(configurations.Any());
        }

        [Test]
        public void Process_ShouldRegisterHaloWizardTileConfigurationsByName()
        {
            //---------------Set up test pack-------------------
            var wizardConfiguration = new ThirdPartyInvoiceCaptureWizardConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var wizardTileConfiguration = this.iocContainer.GetInstance<IHaloWizardTileConfiguration>(wizardConfiguration.Name);
            //---------------Test Result -----------------------
            Assert.IsNotNull(wizardTileConfiguration);
            Assert.IsInstanceOf<ThirdPartyInvoiceCaptureWizardConfiguration>(wizardTileConfiguration);
        }

        [Test]
        public void Process_ShouldThirdPartyInvoiceCaptureWizardconfiguration()
        {
            //---------------Set up test pack-------------------
            var wizardConfiguration = new ThirdPartyInvoiceCaptureWizardConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = this.iocContainer.GetAllInstances<IHaloWizardTileConfiguration>();
            //---------------Test Result -----------------------
            Assert.IsTrue(configurations.Any());
            var foundConfiguration = configurations.FirstOrDefault(configuration => configuration.Name == wizardConfiguration.Name);
            Assert.IsNotNull(foundConfiguration);
        }
    }
}
