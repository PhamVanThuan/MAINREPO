using System;
using System.Linq;

using NUnit.Framework;
using StructureMap;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.Config.UI.Halo.Convention;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloTileHeaderConvention
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
                            scanner.Convention<HaloTileHeaderConvention>();
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
            var convention = new HaloTileHeaderConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloHeaderConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileHeaders = this.iocContainer.GetAllInstances<IHaloTileHeader<ClientRootTileConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(tileHeaders.Any());
        }
    }
}
