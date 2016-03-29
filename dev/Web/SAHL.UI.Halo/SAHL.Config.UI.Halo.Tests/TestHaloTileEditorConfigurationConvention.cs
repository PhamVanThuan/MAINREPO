using System.Linq;

using NUnit.Framework;

using StructureMap;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client;
using SAHL.UI.Halo.Configuration.Client.Detail;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloTileEditorConfigurationConvention
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
                            scanner.Convention<HaloTileEditorConfigurationConvention>();
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
            var convention = new HaloTileEditorConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloTileEditorConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileEditorConfigurations = this.iocContainer.GetAllInstances<IHaloTileEditorConfiguration<ClientRootTileConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(tileEditorConfigurations.Any());
        }

        [Test]
        public void Process_ShouldRegisterLegalEntityEditorTileAssociatedWithLegalEntityTile()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileRootConfigurations = this.iocContainer.GetAllInstances<IHaloTileEditorConfiguration<ClientRootTileConfiguration>>();
            //---------------Test Result -----------------------
            var legalEntityEditorConfiguration = new ClientDetailEditorTileConfiguration();
            var registeredConfiguration = tileRootConfigurations.FirstOrDefault(configuration => ((IHaloTileConfiguration)configuration).Name == legalEntityEditorConfiguration.Name);
            Assert.IsNotNull(registeredConfiguration);
        }
    }
}
