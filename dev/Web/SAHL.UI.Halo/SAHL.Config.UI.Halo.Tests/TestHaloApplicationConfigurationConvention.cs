using System.Linq;

using NUnit.Framework;

using StructureMap;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.UI.Halo.Configuration;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloApplicationConfigurationConvention
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
                            scanner.Convention<HaloApplicationConfigurationConvention>();
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
            var convention = new HaloApplicationConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloApplicationConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var applicationConfigurations = this.iocContainer.GetAllInstances<IHaloApplicationConfiguration>();
            //---------------Test Result -----------------------
            Assert.IsTrue(applicationConfigurations.Any());
        }

        [Test]
        public void Process_ShouldRegisterHomeApplication()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var applicationConfigurations = this.iocContainer.GetAllInstances<IHaloApplicationConfiguration>();
            //---------------Test Result -----------------------
            Assert.Greater(applicationConfigurations.Count(), 0);

            var homeHaloApplicationConfiguration = new HomeHaloApplicationConfiguration();
            var foundConfiguration = applicationConfigurations.FirstOrDefault(configuration => configuration.Name == homeHaloApplicationConfiguration.Name);
            Assert.IsNotNull(foundConfiguration);
        }
    }
}
