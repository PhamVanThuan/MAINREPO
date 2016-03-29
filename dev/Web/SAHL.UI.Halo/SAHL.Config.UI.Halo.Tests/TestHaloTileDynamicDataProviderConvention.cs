using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using StructureMap;

using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Config.Core;
using SAHL.Core.Testing.Fakes;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Application;
using SAHL.UI.Halo.DataProvider.Application;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloTileDynamicDataProviderConvention
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
                            scanner.Convention<HaloTileDynamicDataProviderConvention>();
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
            var convention = new HaloTileDynamicDataProviderConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloTileSubDataProviders()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var allProviders = this.iocContainer.GetAllInstances<IHaloTileDynamicDataProvider<ApplicationRootTileConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(allProviders.Any());
        }

        [Test]
        public void Process_ShouldRegisterApplicationRootTileDynamicDataProvider()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var provider = this.iocContainer.GetInstance<IHaloTileDynamicDataProvider<ApplicationRootTileConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(provider);
            Assert.IsInstanceOf<ApplicationDynamicDataProvider>(provider);
        }
    }
}
