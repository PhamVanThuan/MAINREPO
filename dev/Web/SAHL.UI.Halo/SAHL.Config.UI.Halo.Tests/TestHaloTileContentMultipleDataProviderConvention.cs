using System.Linq;

using StructureMap;

using NUnit.Framework;

using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Config.Core;
using SAHL.Core.Testing.Fakes;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Models.Common.LoanTransaction;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloTileContentMultipleDataProviderConvention
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
                            scanner.Convention<HaloTileContentMultipleDataProviderConvention>();
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
            var convention = new HaloTileContentMultipleDataProviderConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloTileContentMultipleDataProviders()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProviders = this.iocContainer.GetAllInstances<IHaloTileContentMultipleDataProvider<LoanTransactionTileModel>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(contentProviders.Any());
        }

        [Test]
        public void Process_ShouldRegisterLoanTransactionRootTileModelContentProvider()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = this.iocContainer.GetInstance<IHaloTileContentMultipleDataProvider<LoanTransactionTileModel>>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(contentProvider);
        }
    }
}
