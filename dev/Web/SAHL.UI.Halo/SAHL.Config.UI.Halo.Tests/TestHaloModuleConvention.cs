using System.Linq;

using StructureMap;

using NUnit.Framework;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.UI.Halo.Configuration;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloModuleConvention
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
                            scanner.Convention<HaloModuleConfigurationConvention>();
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
            var convention = new HaloModuleConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloModuleConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloModuleConfigurations = this.iocContainer.GetAllInstances<IHaloModuleConfiguration>();
            //---------------Test Result -----------------------
            Assert.IsTrue(haloModuleConfigurations.Any());
        }

        [Test]
        public void Process_ShouldRegisterClientsModule()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloModuleConfigurations = this.iocContainer.GetAllInstances<IHaloModuleConfiguration>();
            //---------------Test Result -----------------------
            Assert.IsTrue(haloModuleConfigurations.Any());

            var clientsHomeConfiguration = new ClientHomeConfiguration();
            var foundConfiguration = haloModuleConfigurations.FirstOrDefault(configuration => configuration.Name == clientsHomeConfiguration.Name);
            Assert.IsNotNull(foundConfiguration);
        }
    }
}
