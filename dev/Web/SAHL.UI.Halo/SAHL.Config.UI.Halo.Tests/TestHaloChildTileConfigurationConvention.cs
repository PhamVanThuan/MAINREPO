using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client;
using SAHL.UI.Halo.Configuration.Client.MortgageLoan;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloChildTileConfigurationConvention
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
                            scanner.Convention<HaloChildTileConfigurationConvention>();
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
            var convention = new HaloChildTileConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloModuleConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var childTileConfigurations = this.iocContainer.GetAllInstances<IHaloChildTileConfiguration<SAHL.UI.Halo.Configuration.Client.ClientRootTileConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(childTileConfigurations.Any());
        }

        [Test]
        public void Process_ShouldRegisterLegalEntityRootTileAssociatedWithLegalEntityTile()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var childTileConfigurations = this.iocContainer.GetAllInstances<IHaloChildTileConfiguration<ClientRootTileConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(childTileConfigurations.Any());
            var mortgageLoanChildTileConfiguration = new MortgageLoanChildTileConfiguration();
            var registeredConfiguration = childTileConfigurations.FirstOrDefault(configuration => configuration.Name == mortgageLoanChildTileConfiguration.Name);
            Assert.IsNotNull(registeredConfiguration);
        }
    }
}
