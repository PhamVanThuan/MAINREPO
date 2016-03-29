using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SAHL.UI.Halo.MyHalo.Configuration.Menu;
using StructureMap;

using NUnit.Framework;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloMenuItemConvention
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
                            scanner.Convention<HaloMenuItemConvention>();
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
            var convention = new HaloModuleConfigurationConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloMenuItemConfigurations()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloMenuItems = this.iocContainer.GetAllInstances<IHaloMenuItem>();
            //---------------Test Result -----------------------
            Assert.IsTrue(haloMenuItems.Any());
        }

        [Test]
        public void Process_ShouldRegisterHomeMenuItem()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var haloMenuItems = this.iocContainer.GetAllInstances<IHaloMenuItem>();
            //---------------Test Result -----------------------
            Assert.IsTrue(haloMenuItems.Any());

            var homeMenuItem       = new HomeMenuItem();
            var foundConfiguration = haloMenuItems.FirstOrDefault(configuration => configuration.Name == homeMenuItem.Name);
            Assert.IsNotNull(foundConfiguration);
        }
    }
}
