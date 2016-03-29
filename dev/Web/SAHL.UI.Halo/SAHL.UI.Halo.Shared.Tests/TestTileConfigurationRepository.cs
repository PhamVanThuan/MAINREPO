using System;
using System.Linq;
using System.Collections.Generic;

using StructureMap;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.UI.Halo.Configuration;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client;
using SAHL.UI.Halo.MyHalo.Configuration.Home;
using SAHL.UI.Halo.Configuration.Client.MortgageLoan;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan;
using SAHL.UI.Halo.Configuration.Task.ThirdPartyInvoices;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice;

namespace SAHL.UI.Halo.Shared.Tests
{
    [TestFixture]
    public class TestTileConfigurationRepository
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
            var localContainer = Substitute.For<IIocContainer>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var repository = new TileConfigurationRepository(localContainer);
            //---------------Test Result -----------------------
            Assert.IsNotNull(repository);
        }

        [TestCase("iocContainer")]
        public void Constructor_GivenNullConstuctorParameter_ShouldThrowExceptionWithParameterName(string parameterName)
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ConstructorTestUtils.CheckForExceptionWhenParameterIsNull<TileConfigurationRepository>(parameterName);
            //---------------Test Result -----------------------
        }

        [Test]
        public void FindApplicationConfiguration_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "test";
            var localContainer           = Substitute.For<IIocContainer>();
            var repository               = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindApplicationConfiguration(applicationName);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetAllInstances<IHaloApplicationConfiguration>();
        }

        [Test]
        public void FindApplicationConfiguration_GivenApplicationConfigurationDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configuration = repository.FindApplicationConfiguration("test");
            //---------------Test Result -----------------------
            Assert.IsNull(configuration);
        }

        [Test]
        public void FindApplicationConfiguration_GivenApplicationConfigurationExists_ShouldReturnConfiguration()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "Home";
            var repository               = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configuration = repository.FindApplicationConfiguration(applicationName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(configuration);
            Assert.AreEqual(applicationName, configuration.Name);
        }

        [Test]
        public void FindModuleConfiguration_GivenModuleConfigurationDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var repository               = new TileConfigurationRepository(this.iocContainer);
            var applicationConfiguration = repository.FindApplicationConfiguration("Home");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(applicationConfiguration);
            //---------------Execute Test ----------------------
            var moduleConfiguration = repository.FindModuleConfiguration(applicationConfiguration, "invalidModule");
            //---------------Test Result -----------------------
            Assert.IsNull(moduleConfiguration);
        }

        [Test]
        public void FindModuleConfiguration_GivenModuleConfigurationExists_ShouldReturnModuleConfiguration()
        {
            //---------------Set up test pack-------------------
            const string moduleName      = "Home";
            var repository               = new TileConfigurationRepository(this.iocContainer);
            var applicationConfiguration = repository.FindApplicationConfiguration("My Halo");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(applicationConfiguration);
            //---------------Execute Test ----------------------
            var moduleConfiguration = repository.FindModuleConfiguration(applicationConfiguration, moduleName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(moduleConfiguration);
            Assert.AreEqual(moduleName, moduleConfiguration.Name);
        }

        [Test]
        public void FindModuleLinkedTileConfigurationByName_GivenLinkedTileConfigurationDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var moduleConfiguration = new HomeModuleConfiguration();
            var repository          = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var linkedConfiguration = repository.FindModuleLinkedTileConfigurationByName(moduleConfiguration, "unknown");
            //---------------Test Result -----------------------
            Assert.IsNull(linkedConfiguration);
        }

        [Test]
        public void FindModuleLinkedTileConfigurationByName_GivenLinkedTileConfigurationExists_ShouldReturnLinkedRootTileConfiguration()
        {
            //---------------Set up test pack-------------------
            var linkedRootTileConfiguration = new ThirdPartyInvoicesLinkedRootTileConfiguration();
            var moduleConfiguration         = new TaskHomeConfiguration();
            var repository                  = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var linkedConfiguration = repository.FindModuleLinkedTileConfigurationByName(moduleConfiguration, linkedRootTileConfiguration.Name);
            //---------------Test Result -----------------------
            Assert.IsNotNull(linkedConfiguration);
            Assert.IsInstanceOf<ThirdPartyInvoicesLinkedRootTileConfiguration>(linkedConfiguration);
            Assert.AreEqual(linkedRootTileConfiguration.Name, linkedRootTileConfiguration.Name);
        }

        [Test]
        public void FindRootTileConfiguration_GivenNoTileConfiguration_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => repository.FindRootTileConfiguration(null, "invalidTile"));
            //---------------Test Result -----------------------
            Assert.AreEqual("moduleConfiguration", exception.ParamName);
        }

        [Test]
        public void FindRootTileConfiguration_GivenRootTileConfigurationDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);

            var applicationConfiguration = repository.FindApplicationConfiguration("Home");
            Assert.IsNotNull(applicationConfiguration);

            var moduleConfiguration = repository.FindModuleConfiguration(applicationConfiguration, "Clients");
            Assert.IsNotNull(moduleConfiguration);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var rootTileConfiguration = repository.FindRootTileConfiguration(moduleConfiguration, "invalidRootTile");
            //---------------Test Result -----------------------
            Assert.IsNull(rootTileConfiguration);
        }

        [Test]
        public void FindRootTileConfiguration_GivenRootTileConfigurationExists_ShouldReturnRootTileConfiguration()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);

            var applicationConfiguration = repository.FindApplicationConfiguration("Home");
            Assert.IsNotNull(applicationConfiguration);

            var moduleConfiguration = repository.FindModuleConfiguration(applicationConfiguration, "Clients");
            Assert.IsNotNull(moduleConfiguration);

            var rootTileName = "Client";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var rootTileConfiguration = repository.FindRootTileConfiguration(moduleConfiguration, rootTileName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(rootTileConfiguration);
            Assert.AreEqual(rootTileName, rootTileConfiguration.Name);
        }

        [Test]
        public void FindChildTileConfiguration_GivenNoTileConfiguration_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => repository.FindChildTileConfiguration(null, "invalidTile"));
            //---------------Test Result -----------------------
            Assert.AreEqual("rootTileConfiguration", exception.ParamName);
        }

        [Test]
        public void FindChildTileConfiguration_GivenChildTileConfigurationDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            var rootTileConfiguration = new ClientRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var childTileConfiguration = repository.FindChildTileConfiguration(rootTileConfiguration, "invalidChildTile");
            //---------------Test Result -----------------------
            Assert.IsNull(childTileConfiguration);
        }

        [Test]
        public void FindChildTileConfiguration_GivenChildTileConfigurationExists_ShouldReturnChildTileConfiguration()
        {
            //---------------Set up test pack-------------------
            var childTileName = "Mortgage Loan";
            var repository = new TileConfigurationRepository(this.iocContainer);
            var rootTileConfiguration = new ClientRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var childTileConfiguration = repository.FindChildTileConfiguration(rootTileConfiguration, childTileName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(childTileConfiguration);
            Assert.AreEqual(childTileName, childTileConfiguration.Name);
        }

        [Test]
        public void FindTileEditorConfiguration_GivenTileEditorConfigurationDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);

            var applicationConfiguration = repository.FindApplicationConfiguration("Home");
            Assert.IsNotNull(applicationConfiguration);

            var moduleConfiguration = repository.FindModuleConfiguration(applicationConfiguration, "Clients");
            Assert.IsNotNull(moduleConfiguration);

            var tileConfiguration = repository.FindRootTileConfiguration(moduleConfiguration, "Client");
            Assert.IsNotNull(tileConfiguration);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var rootTileConfiguration = repository.FindTileEditorConfiguration(tileConfiguration, "invalidEditorTile");
            //---------------Test Result -----------------------
            Assert.IsNull(rootTileConfiguration);
        }

        [Test]
        public void FindTileEditorConfiguration_GivenTileEditorConfigurationExists_ShouldReturnTileEditorConfiguration()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);

            var applicationConfiguration = repository.FindApplicationConfiguration("Home");
            Assert.IsNotNull(applicationConfiguration);

            var moduleConfiguration = repository.FindModuleConfiguration(applicationConfiguration, "Clients");
            Assert.IsNotNull(moduleConfiguration);

            var tileConfiguration = repository.FindRootTileConfiguration(moduleConfiguration, "Client");
            Assert.IsNotNull(tileConfiguration);

            var tileName = "Client Detail";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var editorConfiguration = repository.FindTileEditorConfiguration(tileConfiguration, tileName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(editorConfiguration);
            Assert.AreEqual(tileName, editorConfiguration.Name);
        }

        [Test]
        public void FindTileEditorConfigurations_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var legalEntityTileConfiguration = new ClientRootTileConfiguration();
            var localContainer = Substitute.For<IIocContainer>();
            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindTileEditorConfigurations(legalEntityTileConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetAllInstances(typeof(IHaloTileEditorConfiguration<ClientRootTileConfiguration>));
        }

        [Test]
        public void FindTileEditorConfigurations_GivenNoTilesEditorsExistsForTile_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetAllInstances(typeof(IHaloTileEditorConfiguration<ClientRootTileConfiguration>))
                            .Returns(new List<IHaloTileEditorConfiguration<ClientRootTileConfiguration>>());

            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = repository.FindTileEditorConfigurations(new ClientRootTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(configurations);
        }

        [Test]
        public void FindTileEditorConfigurations_GivenEditorsExistsForTile_ShouldReturnListOfEditors()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "Home";
            var repository = new TileConfigurationRepository(this.iocContainer);
            var applicationConfiguration = repository.FindApplicationConfiguration(applicationName);
            Assert.IsNotNull(applicationConfiguration);

            var allModuleConfigurations = repository.FindApplicationModuleConfigurations(applicationConfiguration);
            Assert.IsNotNull(allModuleConfigurations);

            var moduleConfiguration = allModuleConfigurations.FirstOrDefault(configuration => configuration.Name == "Clients");
            Assert.IsNotNull(moduleConfiguration);

            var allModuleTileConfigurations = repository.FindModuleRootTileConfigurations(moduleConfiguration);
            Assert.IsNotNull(moduleConfiguration);

            var tileConfiguration = allModuleTileConfigurations.FirstOrDefault(configuration => configuration.Name == "Client");
            Assert.IsNotNull(tileConfiguration);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var allTileEditorConfigurations = repository.FindTileEditorConfigurations(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(allTileEditorConfigurations);
            Assert.Greater(allTileEditorConfigurations.Count(), 0);
        }

        [Test]
        public void FindTileActionDrilldown_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new MortgageLoanChildTileConfiguration();
            var localContainer = Substitute.For<IIocContainer>();
            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindTileActionDrillDown(tileConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetInstance(typeof(IHaloTileActionDrilldown<MortgageLoanChildTileConfiguration>));
        }

        [Test]
        public void FindTileActionDrilldown_GivenNoTileActionDrilldown_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetInstance(typeof(IHaloTileActionDrilldown<MortgageLoanChildTileConfiguration>)).Returns(null);

            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileActionDrillDown = repository.FindTileActionDrillDown(new MortgageLoanChildTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(tileActionDrillDown);
        }

        [Test]
        public void FindTileActionDrilldown_GivenTileActionDrilldownExistForTile_ShouldReturnTileActionDrilldownForTile()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new MortgageLoanChildTileConfiguration();
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileActionDrillDown = repository.FindTileActionDrillDown(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(tileActionDrillDown);
            Assert.IsInstanceOf<MortgageLoanChildTileDrilldown>(tileActionDrillDown);
        }

        [Test]
        public void FindAllApplicationConfigurations_ShouldCallIocContainer()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindAllApplicationConfigurations();
            //---------------Test Result -----------------------
            localContainer.Received(1).GetAllInstances<IHaloApplicationConfiguration>();
        }

        [Test]
        public void FindAllApplicationConfigurations_GivenNoApplications_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetAllInstances<IHaloApplicationConfiguration>().Returns(new List<IHaloApplicationConfiguration>());

            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = repository.FindAllApplicationConfigurations();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, configurations.Count());
        }

        [Test]
        public void FindAllApplicationConfigurations_GivenConfiguredIocContainer_ShouldReturnAllApplications()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = repository.FindAllApplicationConfigurations();
            //---------------Test Result -----------------------
            Assert.Greater(configurations.Count(), 0);
        }

        [Test]
        public void FindApplicationModuleConfigurations_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var applicationConfiguration = new HomeHaloApplicationConfiguration();
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetAllInstances<IHaloApplicationConfiguration>().Returns(new List<IHaloApplicationConfiguration> { applicationConfiguration });

            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindApplicationModuleConfigurations(applicationConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetAllInstances(typeof(IHaloModuleApplicationConfiguration<HomeHaloApplicationConfiguration>));
        }

        [Test]
        public void FindApplicationModuleConfigurations_GivenNoModulesExistsForApplication_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var homeHaloApplicationConfiguration = new HomeHaloApplicationConfiguration();
            var allApplicationConfigurations = new List<IHaloApplicationConfiguration> { homeHaloApplicationConfiguration };

            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetAllInstances<IHaloApplicationConfiguration>().Returns(allApplicationConfigurations);

            var repository = new TileConfigurationRepository(localContainer);
            var applicationConfiguration = repository.FindApplicationConfiguration("Home");
            //---------------Assert Precondition----------------
            Assert.IsNotNull(applicationConfiguration);
            //---------------Execute Test ----------------------
            var configurations = repository.FindApplicationModuleConfigurations(applicationConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNull(configurations);
        }

        [Test]
        public void FindApplicationModuleConfigurations_GivenModulesExistForApplication_ShouldReturnListOfModules()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "Home";
            var repository = new TileConfigurationRepository(this.iocContainer);
            var applicationConfiguration = repository.FindApplicationConfiguration(applicationName);
            //---------------Assert Precondition----------------
            Assert.IsNotNull(applicationConfiguration);
            //---------------Execute Test ----------------------
            var allModules = repository.FindApplicationModuleConfigurations(applicationConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(allModules);
            Assert.Greater(allModules.Count(), 0);
        }

        [Test]
        public void FindModuleRootTileConfigurations_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var clientsHomeConfiguration = new ClientHomeConfiguration();
            var localContainer = Substitute.For<IIocContainer>();
            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindModuleRootTileConfigurations(clientsHomeConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetAllInstances(typeof(IHaloModuleTileConfiguration<ClientHomeConfiguration>));
        }

        [Test]
        public void FindModuleRootTileConfigurations_GivenNoTilesExistsForModule_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetAllInstances(typeof(IHaloModuleTileConfiguration<ClientHomeConfiguration>))
                            .Returns(new List<IHaloModuleTileConfiguration<ClientHomeConfiguration>>());

            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = repository.FindModuleRootTileConfigurations(new ClientHomeConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(configurations);
        }

        [Test]
        public void FindModuleRootTileConfigurations_GivenTilesExistForModule_ShouldReturnListOfTiles()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "Home";
            var repository = new TileConfigurationRepository(this.iocContainer);
            var applicationConfiguration = repository.FindApplicationConfiguration(applicationName);
            Assert.IsNotNull(applicationConfiguration);

            var allModuleConfigurations = repository.FindApplicationModuleConfigurations(applicationConfiguration);
            Assert.IsNotNull(allModuleConfigurations);

            var moduleConfiguration = allModuleConfigurations.FirstOrDefault(configuration => configuration.Name == "Clients");
            Assert.IsNotNull(moduleConfiguration);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var allTileConfigurations = repository.FindModuleRootTileConfigurations(moduleConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(allTileConfigurations);
            Assert.Greater(allTileConfigurations.Count(), 0);
        }

        [Test]
        public void FindModuleLinkedRootTileConfigurations_GivenTilesExistForModule_ShouldReturnListOfTiles()
        {
            //---------------Set up test pack-------------------
            const string applicationName = "Home";
            var repository = new TileConfigurationRepository(this.iocContainer);
            var applicationConfiguration = repository.FindApplicationConfiguration(applicationName);
            Assert.IsNotNull(applicationConfiguration);

            var allModuleConfigurations = repository.FindApplicationModuleConfigurations(applicationConfiguration);
            Assert.IsNotNull(allModuleConfigurations);

            var moduleConfiguration = allModuleConfigurations.FirstOrDefault(configuration => configuration.Name == "Task");
            Assert.IsNotNull(moduleConfiguration);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var allTileConfigurations = repository.FindAllModuleLinkedRootTileConfigurations(moduleConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(allTileConfigurations);
            Assert.Greater(allTileConfigurations.Count(), 0);
        }

        [Test]
        public void FindLinkedRootTileConfigurationRootTileConfigurations_GivenTilesExistForModule_ShouldReturnListOfTiles()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            var linkedRootTileConfiguration = new ThirdPartyInvoicesLinkedRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var rootTileConfiguration = repository.FindRootTileConfigurationForLinkedConfiguration(linkedRootTileConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(rootTileConfiguration);
            var expectedConfiguration = new ThirdPartyInvoiceRootTileConfiguration();
            Assert.IsInstanceOf<ThirdPartyInvoiceRootTileConfiguration>(rootTileConfiguration);
            Assert.AreEqual(expectedConfiguration.Name, rootTileConfiguration.Name);
        }

        [Test]
        public void FindChildTileConfigurations_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var legalEntityTileConfiguration = new ClientRootTileConfiguration();
            var localContainer = Substitute.For<IIocContainer>();
            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindChildTileConfigurations(legalEntityTileConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetAllInstances(typeof(IHaloChildTileConfiguration<ClientRootTileConfiguration>));
        }

        [Test]
        public void FindChildTileConfigurations_GivenNoRootTilesExistsForTile_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetAllInstances(typeof(IHaloChildTileConfiguration<ClientRootTileConfiguration>))
                          .Returns(new List<IHaloChildTileConfiguration<ClientRootTileConfiguration>>());

            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = repository.FindChildTileConfigurations(new ClientRootTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(configurations);
        }

        [Test]
        public void FindChildTileConfigurations_GivenTilesExistForTile_ShouldReturnListOfTiles()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var allChildTileConfigurations = repository.FindChildTileConfigurations(new ClientRootTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNotNull(allChildTileConfigurations);
            Assert.IsTrue(allChildTileConfigurations.Any());
        }

        [Test]
        public void FindAllMenuItemsForApplication_ShouldRetrieveFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var applicationConfiguration = new HomeHaloApplicationConfiguration();
            var localContainer = Substitute.For<IIocContainer>();
            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindAllMenuItemsForApplication(applicationConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetAllInstances(typeof(IHaloApplicationMenuItem<HomeHaloApplicationConfiguration>));
        }

        [Test]
        public void FindAllMenuItemsForApplication_GivenNoMenuItemsExistsForApplication_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetAllInstances(typeof(IHaloApplicationMenuItem<HomeHaloApplicationConfiguration>))
                          .Returns(new List<IHaloApplicationMenuItem<HomeHaloApplicationConfiguration>>());

            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var configurations = repository.FindAllMenuItemsForApplication(new HomeHaloApplicationConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(configurations);
        }

        [Test]
        public void FindAllMenuItemsForApplication_GivenMenuItemsExistForApplication_ShouldReturnListOfMenuItems()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            var applicationConfiguration = repository.FindApplicationConfiguration("My Halo");
            Assert.IsNotNull(applicationConfiguration);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var allMenuItemsForApplication = repository.FindAllMenuItemsForApplication(applicationConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(allMenuItemsForApplication);
            Assert.Greater(allMenuItemsForApplication.Count(), 0);
        }

        [Test]
        public void FindAllTileActions_ShouldRetrieveFromIocContainerForAllActionTypes()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new MortgageLoanChildTileConfiguration();
            var localContainer = Substitute.For<IIocContainer>();
            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindAllTileActions(tileConfiguration);
            //---------------Test Result -----------------------
            localContainer.Received(1).GetAllInstances(typeof(IHaloTileActionDrilldown<MortgageLoanChildTileConfiguration>));
            localContainer.Received(1).GetAllInstances(typeof(IHaloTileActionEdit<MortgageLoanChildTileConfiguration>));
        }

        [Test]
        public void FindAllTileActions_GivenNoTileActionsExistForTileConfiguration_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var localContainer = Substitute.For<IIocContainer>();
            localContainer.GetAllInstances(typeof(IHaloTileActionDrilldown<MortgageLoanChildTileConfiguration>))
                          .Returns(info => null);
            localContainer.GetAllInstances(typeof(IHaloTileActionEdit<MortgageLoanChildTileConfiguration>))
                          .Returns(info => null);

            var repository = new TileConfigurationRepository(localContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileActions = repository.FindAllTileActions(new MortgageLoanChildTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNotNull(tileActions);
            Assert.AreEqual(0, tileActions.Count());
        }

        [Test]
        public void FindAllTileActions_GivenTileActionDrilldownExistForTileConfiguration_ShouldReturnListOfTileActions()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            var tileConfiguration = new MortgageLoanChildTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileActions = repository.FindAllTileActions(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(tileActions);
            Assert.Greater(tileActions.Count(), 0);
            Assert.IsInstanceOf<IHaloTileActionDrilldown>(tileActions.First());
        }

        [Test]
        public void FindAllTileActions_GivenTileActionEditExistForRootTileConfiguration_ShouldReturnListOfTileActions()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            var tileConfiguration = new ClientRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileActions = repository.FindAllTileActions(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.IsNotNull(tileActions);
            Assert.Greater(tileActions.Count(), 0);
            Assert.IsInstanceOf<IHaloTileActionDrilldown>(tileActions.First());
        }

        [Test]
        public void FindAllDynamicTileActionProviders_GivenNoTileDynamicActionProvidersDoesNotExistForTileConfiguration_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var providers = repository.FindAllDynamicTileActionProviders(new MortgageLoanChildTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNotNull(providers);
            Assert.AreEqual(0, providers.Count());
        }

        [Test]
        public void FindAllDynamicTileActionProviders_GivenTileDynamicActionProvidersExistForTileConfiguration_ShouldReturnListOfTileActions()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            var tileConfiguration = new MortgageLoanRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var providers = repository.FindAllDynamicTileActionProviders(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.Greater(providers.Count(), 0);
            CollectionAssert.AllItemsAreInstancesOfType(providers, typeof(IHaloTileDynamicActionProvider));
        }

        [Test]
        public void FindAllWorkflowTileActionProviders_GivenNoTileWorkflowActionProvidersExistsForTileConfiguration_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var providers = repository.FindAllWorkflowTileActionProviders(new MortgageLoanChildTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNotNull(providers);
            Assert.AreEqual(0, providers.Count());
        }

        [Test]
        public void FindAllWorkflowTileActionProviders_GivenWorkflowTileActionProvidersExistForTileConfiguration_ShouldReturnListOfTileActions()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            var tileConfiguration = new ClientRootTileConfiguration();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var providers = repository.FindAllWorkflowTileActionProviders(tileConfiguration);
            //---------------Test Result -----------------------
            Assert.IsTrue(providers.Any());
            CollectionAssert.AllItemsAreInstancesOfType(providers, typeof(IHaloWorkflowTileActionProvider));
        }

        [Test]
        public void FindAllWorkflowActionConditionalProviders_GivenNoWorkflowActionConditionalProvidersExists_ShouldReturnEmptyList()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var providers = repository.FindAllWorkflowActionConditionalProviders("Process", "Workflow", "Activity");
            //---------------Test Result -----------------------
            Assert.IsNotNull(providers);
            Assert.AreEqual(0, providers.Count());
        }

        [Test]
        public void FindAllWorkflowActionConditionalProviders_GivenWorkflowActionConditionalProvidersExist_ShouldReturnListOfProviders()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);

            var whatDoIHave = ObjectFactory.WhatDoIHave();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var providers = repository.FindAllWorkflowActionConditionalProviders("Third Party Invoices", "Third Party Invoices", "Approve for Payment");
            //---------------Test Result -----------------------
            Assert.IsTrue(providers.Any());
            //CollectionAssert.AllItemsAreInstancesOfType(providers, typeof(IHaloWorkflowTileActionProvider));
        }

        [Test]
        public void FindTileHeader_ShouldRetrieveTileHeaderFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var container = Substitute.For<IIocContainer>();
            var repository = new TileConfigurationRepository(container);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindTileHeader(new ClientRootTileConfiguration());
            //---------------Test Result -----------------------
            container.Received(1).GetInstance(typeof(IHaloTileHeader<ClientRootTileConfiguration>));
        }

        [Test]
        public void FindTileHeader_GivenTileHeaderDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var container = Substitute.For<IIocContainer>();
            container.GetInstance<IHaloTileHeader<ClientRootTileConfiguration>>().Returns(info => null);

            var repository = new TileConfigurationRepository(container);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileHeader = repository.FindTileHeader(new ClientRootTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(tileHeader);
        }

        [Test]
        public void FindTileHeader_GivenTileHeaderExists_ShouldReturnTileHeader()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileHeader = repository.FindTileHeader(new ClientRootTileConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNotNull(tileHeader);
            Assert.IsInstanceOf<ClientRootTileHeaderConfiguration>(tileHeader);
        }

        [Test]
        public void FindAllTileHeaderIconProviders_ShouldRetrieveIconProvidersFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var container = Substitute.For<IIocContainer>();
            var repository = new TileConfigurationRepository(container);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindAllTileHeaderIconProviders(new ClientRootTileHeaderConfiguration());
            //---------------Test Result -----------------------
            container.Received(1).GetAllInstances(typeof(IHaloTileHeaderIconProvider<ClientRootTileHeaderConfiguration>));
        }

        [Test]
        public void FindAllTileHeaderIconProviders_GivenTileHeaderIconProvidersDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var container = Substitute.For<IIocContainer>();
            container.GetAllInstances<IHaloTileHeaderIconProvider<ClientRootTileHeaderConfiguration>>().Returns(info => null);

            var repository = new TileConfigurationRepository(container);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var iconProviders = repository.FindAllTileHeaderIconProviders(new ClientRootTileHeaderConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(iconProviders);
        }

        [Test]
        public void FindAllTileHeaderIconProviders_GivenTileHeaderIconProvidersExists_ShouldReturnListOfIconProviders()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var iconProviders = repository.FindAllTileHeaderIconProviders(new ClientRootTileHeaderConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNotNull(iconProviders);

            var containLeftIconProvider = false;
            var containRightIconProvider = false;

            foreach (var iconProvider in iconProviders)
            {
                if (iconProvider.GetType() == typeof(ClientRootTileHeaderLeftIconProvider))
                {
                    containLeftIconProvider = true;
                }

                if (iconProvider.GetType() == typeof(ClientRootTileHeaderRightIconProvider))
                {
                    containRightIconProvider = true;
                }
            }

            Assert.IsTrue(containLeftIconProvider);
            Assert.IsTrue(containRightIconProvider);
        }

        [Test]
        public void FindAllTileHeaderTextProviders_ShouldRetrieveTextProvidersFromIocContainer()
        {
            //---------------Set up test pack-------------------
            var container = Substitute.For<IIocContainer>();
            var repository = new TileConfigurationRepository(container);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            repository.FindAllTileHeaderTextProviders(new ClientRootTileHeaderConfiguration());
            //---------------Test Result -----------------------
            container.Received(1).GetAllInstances(typeof(IHaloTileHeaderTextProvider<ClientRootTileHeaderConfiguration>));
        }

        [Test]
        public void FindAllTileHeaderTextProviders_GivenTileHeaderTextProvidersDoesNotExist_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            var container = Substitute.For<IIocContainer>();
            container.GetAllInstances<IHaloTileHeaderTextProvider<ClientRootTileHeaderConfiguration>>().Returns(info => null);

            var repository = new TileConfigurationRepository(container);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var textProviders = repository.FindAllTileHeaderTextProviders(new ClientRootTileHeaderConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNull(textProviders);
        }

        [Test]
        public void FindAllTileHeaderTextProviders_GivenTileHeaderTextProvidersExists_ShouldReturnListOfTextProviders()
        {
            //---------------Set up test pack-------------------
            var repository = new TileConfigurationRepository(this.iocContainer);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var textProviders = repository.FindAllTileHeaderTextProviders(new ClientRootTileHeaderConfiguration());
            //---------------Test Result -----------------------
            Assert.IsNotNull(textProviders);
            Assert.IsInstanceOf<ClientRootTileHeaderTextProvider>(textProviders.First());
        }
    }
}