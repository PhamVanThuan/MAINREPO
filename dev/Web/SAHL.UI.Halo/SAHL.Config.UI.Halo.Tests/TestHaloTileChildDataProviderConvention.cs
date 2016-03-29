using System.Linq;

using NUnit.Framework;
using StructureMap;

using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Config.Core;
using SAHL.Core.Testing.Fakes;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloTileChildDataProviderConvention
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
                            scanner.Convention<HaloTileChildDataProviderConvention>();
                        });

                    expression.For<IIocContainer>().Use<StructureMapIocContainer>();
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
            var convention = new HaloTileChildDataProviderConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloTileSubDataProviders()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileContentProviders = this.iocContainer.GetAllInstances<IHaloTileChildDataProvider<ClientRootTileConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(tileContentProviders.Any());
        }

        [Test]
        public void Process_ShouldRegisterLegalEntityRootTileSubDataProvider()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = this.iocContainer.GetInstance<IHaloTileChildDataProvider<ClientRootTileConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(contentProvider);
        }
    }
}
