using System.Linq;

using StructureMap;

using NUnit.Framework;

using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Config.Core;
using SAHL.Core.Testing.Fakes;
using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Models.Client.MortgageLoan;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloTileContentDataProviderConvention
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
                            scanner.Convention<HaloTileContentDataProviderConvention>();
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
            var convention = new HaloTileContentDataProviderConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloTileContentProviders()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileContentProviders = this.iocContainer.GetAllInstances<IHaloTileContentDataProvider<ClientRootModel>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(tileContentProviders.Any());
        }

        [Test]
        public void Process_ShouldRegisterlegalEntityRootTileModelContentProvider()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = this.iocContainer.GetInstance<IHaloTileContentDataProvider<ClientRootModel>>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(contentProvider);
        }

        [Test]
        public void Process_ShouldRegisterMortgageLoanChildModelTileContentProvider()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = this.iocContainer.GetInstance<IHaloTileContentDataProvider<MortgageLoanChildModel>>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(contentProvider);
        }
    }
}
