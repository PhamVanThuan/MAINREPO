using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Core.Data;
using SAHL.Config.Core;
using SAHL.Core.Testing.Fakes;
using SAHL.Config.UI.Halo.Convention;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloWorkflowActionConditionalProviderConvention
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
                            scanner.Convention<HaloWorkflowActionConditionalProviderConvention>();
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
            var convention = new HaloWorkflowActionConditionalProviderConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloWorkflowActionConditionalProviders()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloMenuItems = this.iocContainer.GetAllInstances<IHaloWorkflowActionConditionalProvider>();
            //---------------Test Result -----------------------
            Assert.IsTrue(haloMenuItems.Any());
        }
    }
}
