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
    public class TestHaloWizardEWoekflowConfigurationConvention
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
                            scanner.Convention<HaloWizardWorkflowConfigurationConvention>();
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
            var convention = new HaloWizardWorkflowConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloWizardWorkflowConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = this.iocContainer.GetAllInstances<IHaloWizardWorkflowConfiguration>();
            //---------------Test Result -----------------------
            Assert.IsTrue(configurations.Any());
        }

        [Test]
        public void Process_ShouldThirdPartyAcceptInvoiceWizardWorkflowConfiguration()
        {
            //---------------Set up test pack-------------------
            var wizardConfiguration = new ThirdPartyAcceptInvoiceWorkflowWizardConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = this.iocContainer.GetAllInstances<IHaloWizardWorkflowConfiguration>();
            //---------------Test Result -----------------------
            Assert.IsTrue(configurations.Any());

            var workflowConfiguration = configurations.FirstOrDefault(configuration => configuration.ProcessName == wizardConfiguration.ProcessName &&
                                                                                       configuration.WorkflowName == wizardConfiguration.WorkflowName &&
                                                                                       configuration.ActivityName == wizardConfiguration.ActivityName);
            Assert.IsNotNull(workflowConfiguration);
            Assert.IsInstanceOf<ThirdPartyAcceptInvoiceWorkflowWizardConfiguration>(workflowConfiguration);
        }
    }
}
