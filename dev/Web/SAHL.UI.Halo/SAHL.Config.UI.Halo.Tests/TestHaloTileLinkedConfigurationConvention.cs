using NUnit.Framework;

using StructureMap;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices;
using SAHL.UI.Halo.Configuration.Task.ThirdPartyInvoices;
using SAHL.UI.Halo.Shared.Configuration.LinkedRootTileConfiguration;
using SAHL.UI.Halo.Configuration.ThirdParty.Invoices.AggregatedInvoices.Invoices.Invoice;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloTileLinkedConfigurationConvention
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
                    scanner.Convention<HaloTileLinkedConfigurationConvention>();
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
            var convention = new HaloTileLinkedConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterThirdPartyInvoiceRootTileConfiguration()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = new ThirdPartyInvoicesLinkedRootTileConfiguration();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(tileConfiguration);
            //---------------Execute Test ----------------------
            var linkedConfiguration = this.iocContainer.GetInstance<IHaloRootTileLinkedConfiguration<ThirdPartyInvoiceRootTileConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(linkedConfiguration);
        }
    }
}
