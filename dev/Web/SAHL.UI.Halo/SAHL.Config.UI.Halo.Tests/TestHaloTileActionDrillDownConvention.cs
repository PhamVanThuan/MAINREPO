using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using StructureMap;
using NUnit.Framework;

using SAHL.Core;
using SAHL.Config.Core;
using SAHL.Config.UI.Halo.Convention;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.UI.Halo.Configuration.Client.MortgageLoan;

namespace SAHL.Config.UI.Halo.Tests
{
    [TestFixture]
    public class TestHaloTileActionDrillDownConvention
    {
        private IIocContainer iocContainer;

        [TestFixtureSetUp]
        public void Setup()
        {
            StructureMap.ObjectFactory.Initialize(expression =>
                {
                    expression.Scan(scanner =>
                        {
                            scanner.AssembliesFromApplicationBaseDirectory(assembly => assembly.FullName.StartsWith("SAHL"));
                            scanner.Convention<HaloTileActionDrillDownConvention>();
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
            var convention = new HaloTileActionDrillDownConvention();
            //---------------Test Result -----------------------
            Assert.IsNotNull(convention);
        }

        [Test]
        public void Process_ShouldRegisterHaloTileActionDrilldowns()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileActionDrilldowns = this.iocContainer.GetAllInstances<IHaloTileActionDrilldown<MortgageLoanChildTileConfiguration>>();
            //---------------Test Result -----------------------
            Assert.IsTrue(tileActionDrilldowns.Any());
        }

        [Test]
        public void Process_ShouldRegisterMortgageLoanChildTileDrilldownAssociatedWithMortgageLoanChildDrilldown()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileActionDrilldowns = this.iocContainer.GetAllInstances<IHaloTileActionDrilldown<MortgageLoanChildTileConfiguration>>();
            //---------------Test Result -----------------------
            var tileDrilldown           = new MortgageLoanChildTileDrilldown();
            var registeredConfiguration = tileActionDrilldowns.FirstOrDefault(configuration => ((IHaloTileActionDrilldown)configuration).Name == tileDrilldown.Name);
            Assert.IsNotNull(registeredConfiguration);
        }
    }
}
