using System.Linq;

using StructureMap;

using NUnit.Framework;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.UI.Halo.MyHalo.Configuration;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.MyHalo.Configuration.Menu;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestApplicationMenuItemConvention
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
                            scanner.Convention<HaloApplicationMenuItemConvention>();
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
            var convention = new HaloApplicationMenuItemConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloApplicationMenuItems()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var applicationMenuItems = this.iocContainer.GetAllInstances<IHaloApplicationMenuItem<MyHaloHaloApplicationConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(applicationMenuItems.Any());
        }

        [Test]
        public void Process_ShouldRegisterMenuItemsAssociatedWithMyHaloApplication()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var applicationMenuItems = this.iocContainer.GetAllInstances<IHaloApplicationMenuItem<MyHaloHaloApplicationConfiguration>>();
            //---------------Test Result -----------------------
            var homeMenuItem            = new HomeMenuItem();
            var registeredConfiguration = applicationMenuItems.FirstOrDefault(configuration => configuration.Name == homeMenuItem.Name);
            Assert.IsNotNull(registeredConfiguration);
        }
    }
}
