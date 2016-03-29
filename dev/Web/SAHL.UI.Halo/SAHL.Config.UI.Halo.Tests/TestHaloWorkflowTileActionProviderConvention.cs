using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using StructureMap;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.Config.UI.Halo.Convention;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloWorkflowTileActionProviderConvention
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
                            scanner.Convention<HaloWorkflowTileActionProviderConvention>();
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
            var convention = new HaloWorkflowTileActionProviderConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloWorkflowTileActionProviders()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dynamicActionProviders = this.iocContainer.GetAllInstances<IHaloWorkflowTileActionProvider<HaloWorkflowTileActionProvider>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(dynamicActionProviders.Any());
        }
    }
}
