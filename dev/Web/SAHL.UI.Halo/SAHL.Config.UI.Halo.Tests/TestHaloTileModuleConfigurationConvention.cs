using NUnit.Framework;
using SAHL.Config.Core;
using SAHL.Core;
using SAHL.UI.Halo.Configuration;
using SAHL.UI.Halo.Configuration.Client;
using SAHL.UI.Halo.Configuration.Task.ThirdPartyInvoices;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Shared.Configuration.LinkedRootTileConfiguration;
using StructureMap;
using System.Linq;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloTileModuleConfigurationConvention
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
                            scanner.Convention<HaloTileModuleConfigurationConvention>();
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
            var convention = new HaloTileModuleConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloModuleConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var moduleApplicationConfigurations = this.iocContainer.GetAllInstances<IHaloModuleTileConfiguration<ClientHomeConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(moduleApplicationConfigurations.Any());
        }

        [Test]
        public void Process_ShouldRegisterClientRootTileAssociatedWithClientsHomeModule()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloTileModuleConfigurations = this.iocContainer.GetAllInstances<IHaloModuleTileConfiguration<ClientHomeConfiguration>>();
            //---------------Test Result -----------------------
            var rootTileConfiguration = new ClientRootTileConfiguration();
            var registeredConfiguration = haloTileModuleConfigurations.Where(configuration => configuration is IHaloTileConfiguration)
                                                                      .FirstOrDefault(configuration => ((IHaloTileConfiguration)configuration).Name == rootTileConfiguration.Name);
            Assert.IsNotNull(registeredConfiguration);
            Assert.IsInstanceOf<ClientRootTileConfiguration>(registeredConfiguration);
        }

        [Test]
        public void Process_ShouldRegisterAssociatedLinkedRootTileWithClientsHomeModule()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloTileModuleConfigurations = this.iocContainer.GetAllInstances<IHaloModuleTileConfiguration<TaskHomeConfiguration>>();
            //---------------Test Result -----------------------
            var linkedRootTileConfiguration = new ThirdPartyInvoicesLinkedRootTileConfiguration();
            var registeredConfiguration = haloTileModuleConfigurations.Where(configuration => configuration is IHaloRootTileLinkedConfiguration)
                                                                          .FirstOrDefault(configuration => ((IHaloRootTileLinkedConfiguration)configuration).Name == linkedRootTileConfiguration.Name);
            Assert.IsNotNull(registeredConfiguration);
            Assert.IsInstanceOf<ThirdPartyInvoicesLinkedRootTileConfiguration>(registeredConfiguration);
        }
    }
}
