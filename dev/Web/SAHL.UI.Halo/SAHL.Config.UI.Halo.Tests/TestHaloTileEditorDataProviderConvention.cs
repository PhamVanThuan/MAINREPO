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
using SAHL.Config.UI.Halo.Convention;
using SAHL.UI.Halo.Models.Client;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.DataProvider.Clients;
using SAHL.UI.Halo.DataProvider.Client;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloTileEditorDataProviderConvention
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
        public void Process_ShouldRegisterHaloTileEditorConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var editorDataProviders = this.iocContainer.GetAllInstances<IHaloTileEditorDataProvider<ClientRootModel>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(editorDataProviders.Any());
        }

        [Test]
        public void Process_ShouldRegisterLegalEntityEditorDataProviderAssociatedWithLegalEntityRootModel()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var editorDataProviders = this.iocContainer.GetAllInstances<IHaloTileEditorDataProvider<ClientRootModel>>();
            //---------------Test Result -----------------------
            var dbFactory              = this.iocContainer.GetInstance<IDbFactory>();
            var tileEditorDataProvider = new ClientRootEditorTileDataProvider(dbFactory);

            var registeredConfiguration = editorDataProviders.FirstOrDefault(configuration => configuration.GetType().Name == tileEditorDataProvider.GetType().Name);
            Assert.IsNotNull(registeredConfiguration);
        }
    }
}
