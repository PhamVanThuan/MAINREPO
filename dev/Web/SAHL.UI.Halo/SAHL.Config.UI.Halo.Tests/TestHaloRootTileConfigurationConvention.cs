using System.Linq;

using NUnit.Framework;
using StructureMap;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Home.Configuration.Clients;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloRootTileConfigurationConvention
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
                            scanner.Convention<HaloRootTileConfigurationConvention>();
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
            var convention = new HaloRootTileConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloModuleConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileRootConfigurations = this.iocContainer.GetAllInstances<IHaloRootTileConfiguration>();
            //---------------Test Result -----------------------
            Assert.IsTrue(tileRootConfigurations.Any());
        }

        [Test]
        public void Process_ShouldRegisterLegalEntityRootTileAssociatedWithLegalEntityTile()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileRootConfigurations = this.iocContainer.GetAllInstances<IHaloRootTileConfiguration<ClientTileConfiguration>>();
            //---------------Test Result -----------------------
            var legalEntityRootTileConfiguration = new ClientRootTileConfiguration();
            var registeredConfiguration          = tileRootConfigurations.FirstOrDefault(configuration => ((IHaloTileConfiguration)configuration).Name == legalEntityRootTileConfiguration.Name);
            Assert.IsNotNull(registeredConfiguration);
        }
    }
}
