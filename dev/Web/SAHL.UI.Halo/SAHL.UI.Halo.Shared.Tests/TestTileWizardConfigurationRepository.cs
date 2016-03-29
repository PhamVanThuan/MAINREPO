using System;
using System.Linq;

using StructureMap;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.UI.Halo.Shared.Repository;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Models.ThirdParty.Invoices;
using SAHL.UI.Halo.Configuration.Wizards.ThirdParty.Invoice;

namespace SAHL.UI.Halo.Shared.Tests
{
    [TestFixture]
    public class TestTileWizardConfigurationRepository
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
                            scanner.LookForRegistries();
                        });

                    expression.For<IDbFactory>().Use<FakeDbFactory>();
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
            var repository = new TileWizardConfigurationRepository(this.iocContainer);
            //---------------Test Result -----------------------
            Assert.IsNotNull(repository);
        }

        [TestCase("iocContainer")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<TileWizardConfigurationRepository>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void FindWizardConfiguration_GivenNoWizardName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => repository.FindWizardConfiguration(null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("wizardName", exception.ParamName);
        }

        [Test]
        public void FindWizardConfiguration_GivenWizardConfigurationDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var wizardConfiguration = repository.FindWizardConfiguration("invalidWizard");
            //---------------Test Result -----------------------
            Assert.IsNull(wizardConfiguration);
        }

        [Test]
        public void FindWizardWorkflowConfiguration_GivenNoProcessName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => repository.FindWizardWorkflowConfiguration(null, "workflow", "activity"));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("processName", exception.ParamName);
        }

        [Test]
        public void FindWizardWorkflowConfiguration_GivenNoWorkflowName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => repository.FindWizardWorkflowConfiguration("process", null, "activity"));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("workflowName", exception.ParamName);
        }

        [Test]
        public void FindWizardWorkflowConfiguration_GivenNoActivityName_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => repository.FindWizardWorkflowConfiguration("process", "workflow", null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("activityName", exception.ParamName);
        }

        [Test]
        public void FindWizardWorkflowConfiguration_GivenWizardConfigurationDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var wizardConfiguration = repository.FindWizardWorkflowConfiguration("process", "workflow", "activity");
            //---------------Test Result -----------------------
            Assert.IsNull(wizardConfiguration);
        }

        [Test]
        public void FindWizardWorkflowConfiguration_WhenWizardConfigurationExists_ShouldReturnConfiguration()
        {
            //---------------Set up test pack-------------------
            var configuration = new ThirdPartyAcceptInvoiceWorkflowWizardConfiguration();
            var repository    = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var wizardConfiguration = repository.FindWizardWorkflowConfiguration(configuration.ProcessName, 
                                                                                 configuration.WorkflowName,
                                                                                 configuration.ActivityName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(wizardConfiguration);
            Assert.IsInstanceOf<ThirdPartyAcceptInvoiceWorkflowWizardConfiguration>(wizardConfiguration);
        }

        [Test]
        public void FindWizardTilePageConfigurations_GivenWizardConfigurationAndWizardPagesExists_ShouldReturnPageConfigurations()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            var captureWizardConfiguration = new ThirdPartyInvoiceCaptureWizardConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var pageConfigurations = repository.FindWizardTilePageConfigurations((IHaloWizardTileConfiguration)captureWizardConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(pageConfigurations);
        }

        [Test]
        public void FindWizardTilePageConfigurations_GivenWorkflowWizardConfigurationAndWizardPagesExists_ShouldReturnPageConfigurations()
        {
            //---------------Set up test pack-------------------
            var repository          = this.CreateRepository();
            var wizardConfiguration = new ThirdPartyAcceptInvoiceWorkflowWizardConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var pageConfigurations = repository.FindWizardTilePageConfigurations((IHaloWizardTileConfiguration)wizardConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(pageConfigurations);

            var pageConfiguration = pageConfigurations.First();
            Assert.IsInstanceOf<ThirdPartyAcceptInvoiceWorkflowWizardPageConfiguration>(pageConfiguration);
        }

        [Test]
        public void FindWizardPageDataModel_GivenDataModelExists_ShouldReturnDataModelInstance()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dataModel = repository.FindWizardPageDataModel(new ThirdPartyInvoiceCaptureWizardPageConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataModel);
            Assert.IsInstanceOf<ThirdPartyInvoiceRootModel>(dataModel);
        }

        [Test]
        public void FindWizardPageState_GivenPageStateExists_ShouldReturnPageStateInstance()
        {
            //---------------Set up test pack-------------------
            var repository = this.CreateRepository();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var pageState = repository.FindWizardPageState(new ThirdPartyInvoiceCaptureWizardPageConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNotNull(pageState);
            Assert.IsInstanceOf<ThirdPartyInvoiceCaptureWizardPageState>(pageState);
        }

        private ITileWizardConfigurationRepository CreateRepository(IIocContainer iocContainer = null)
        {
            iocContainer = iocContainer ?? this.iocContainer;
            var repository = new TileWizardConfigurationRepository(iocContainer);
            return repository;
        }
    }
}
