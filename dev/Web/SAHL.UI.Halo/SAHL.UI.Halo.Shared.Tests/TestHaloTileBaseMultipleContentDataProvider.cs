using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NSubstitute;
using NUnit.Framework;

using SAHL.Core.Data;
using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.UI.Halo.DataProvider.Common.LoanTransactions;

namespace SAHL.UI.Halo.Shared.Tests
{
    [TestFixture]
    public class TestHaloTileBaseMultipleContentDataProvider
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var dbFactory = Substitute.For<IDbFactory>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dataProvider = new LoanTransactionsChildTileContentDataProvider(dbFactory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataProvider);
        }

        [Test]
        public void Constructor_GivenNullDbFactory_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new LoanTransactionsChildTileContentDataProvider(null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("dbFactory", exception.ParamName);
        }

        [Test]
        public void LoadData_ShouldUseInReadonlyAppContextConnection()
        {
            //---------------Set up test pack-------------------
            var dbFactory    = Substitute.For<IDbFactory>();
            var dataProvider = new LoanTransactionsChildTileContentDataProvider(dbFactory);
            var businessKey  = new BusinessContext("context", GenericKeyType.Account, 1);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => dataProvider.Load(businessKey));
            //---------------Test Result -----------------------
            dbFactory.Received(1).NewDb();
            dbFactory.NewDb().Received(1).InReadOnlyAppContext();
        }
    }
}
