using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;
using SAHL.UI.Halo.Configuration.Client.MortgageLoan;
using SAHL.UI.Halo.Configuration.Account.MortgageLoan;

namespace SAHL.UI.Halo.Shared.Tests
{
    [TestFixture]
    public class TestHaloTileActionDrilldownBase
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileDrilldown = new MortgageLoanChildTileDrilldown();
            //---------------Test Result -----------------------
            Assert.IsNotNull(tileDrilldown);
        }

        [Test]
        public void Constructor_ShouldSetProperties()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var tileDrilldown = new MortgageLoanChildTileDrilldown();
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(MortgageLoanRootTileConfiguration), tileDrilldown.RootTileType);
            Assert.IsInstanceOf<MortgageLoanRootTileConfiguration>(tileDrilldown.RootTileConfiguration);

            Assert.AreEqual(typeof(MortgageLoanChildTileConfiguration), tileDrilldown.TileConfigurationType);
            Assert.IsInstanceOf<MortgageLoanChildTileConfiguration>(tileDrilldown.TileConfiguration);
        }
    }
}
