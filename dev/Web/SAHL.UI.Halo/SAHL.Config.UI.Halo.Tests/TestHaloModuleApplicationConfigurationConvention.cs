using System.Linq;

using NUnit.Framework;
using StructureMap;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.UI.Halo.Configuration;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloModuleApplicationConfigurationConvention
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
                            scanner.Convention<HaloModuleApplicationConfigurationConvention>();
                        });

                    expression.For<IIocContainer>().Use<StructureMapIocContainer>();
                });

            var whatDoIHave = ObjectFactory.WhatDoIHave();

            this.iocContainer = ObjectFactory.GetInstance<IIocContainer>();
            Assert.IsNotNull(this.iocContainer);
        }

        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var convention = new HaloModuleApplicationConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloModuleConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var moduleApplicationConfigurations = this.iocContainer.GetAllInstances<IHaloModuleApplicationConfiguration<HomeHaloApplicationConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(moduleApplicationConfigurations.Any());
        }

        [Test]
        public void Process_ShouldRegisterClientsModuleAssociatedWithHomeApplication()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var moduleApplicationConfigurations = this.iocContainer.GetAllInstances<IHaloModuleApplicationConfiguration<HomeHaloApplicationConfiguration>>();

            //---------------Test Result -----------------------
            var clientsHomeConfiguration = new ClientHomeConfiguration();
            var registeredConfiguration = moduleApplicationConfigurations.FirstOrDefault(configuration => configuration.Name == clientsHomeConfiguration.Name);
            Assert.IsNotNull(registeredConfiguration);
        }
    }
}
