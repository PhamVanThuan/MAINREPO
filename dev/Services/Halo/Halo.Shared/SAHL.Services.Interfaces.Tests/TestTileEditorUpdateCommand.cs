using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.Interfaces.Halo;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.Services.Interfaces.Tests
{
    [TestFixture]
    public class TestTileEditorUpdateCommand
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var tileConfiguration = Substitute.For<IHaloTileConfiguration>();
            var businessKey       = new BusinessContext("context", GenericKeyType.Account, 0);
            var tileDataModel     = Substitute.For<IHaloTileModel>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var updateCommand = new TileEditorUpdateCommand(tileConfiguration, businessKey, tileDataModel);
            //---------------Test Result -----------------------
            Assert.IsNotNull(updateCommand);
        }

        [Test]
        public void Constructor_GivenNullTileConfiguration_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 0);
            var haloTileModel   = Substitute.For<IHaloTileModel>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new TileEditorUpdateCommand(null, businessContext, haloTileModel));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("tileConfiguration", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenNullBusinessContext_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var haloTileConfiguration = Substitute.For<IHaloTileConfiguration>();
            var haloTileModel         = Substitute.For<IHaloTileModel>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new TileEditorUpdateCommand(haloTileConfiguration, null, haloTileModel));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("businessContext", exception.ParamName);
        }

        [Test]
        public void Constructor_GivenNullTileDataModel_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            var haloTileConfiguration = Substitute.For<IHaloTileConfiguration>();
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 0);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new TileEditorUpdateCommand(haloTileConfiguration, businessContext, null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("tileDataModel", exception.ParamName);
        }
    }
}
