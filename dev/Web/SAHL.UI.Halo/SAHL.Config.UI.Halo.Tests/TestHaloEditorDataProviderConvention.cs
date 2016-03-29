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
using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloEditorDataProviderConvention
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
                            scanner.Convention<HaloTileEditorDataProviderConvention>();
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
            var convention = new HaloTileEditorDataProviderConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloTileSubDataProviders()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileContentProviders = this.iocContainer.GetAllInstances<IHaloTileEditorDataProvider<ClientRootModel>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(tileContentProviders.Any());
        }

        [Test]
        public void Process_ShouldRegisterLegalEntityRootTileSubDataProvider()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var contentProvider = this.iocContainer.GetInstance<IHaloTileEditorDataProvider<ClientRootModel>>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(contentProvider);
        }
    }
}
