using NSubstitute;
using NUnit.Framework;
using SAHL.Core.BusinessModel;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Context;
using SAHL.UI.Halo.ContentProvider.Client;
using SAHL.UI.Halo.ContentProvider.Client.MortgageLoan;
using SAHL.UI.Halo.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.UI.Halo.Shared.Tests
{
    [TestFixture]
    public class TestHaloTileBaseChildDataProvider
    {
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            var dbFactory = Substitute.For<IDbFactory>();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var dataProvider = new MortgageLoanChildDataProvider(dbFactory);
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataProvider);
        }

        [Test]
        public void Constructor_GivenNullDbFactory_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => new MortgageLoanChildDataProvider(null));
            //---------------Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("dbFactory", exception.ParamName);
        }

        [Test]
        public void LoadSubKeys_ShouldUseInReadonlyAppContextConnection()
        {
            //---------------Set up test pack-------------------
            var dbFactory = this.CreateDbFactory();
            var dataProvider = new MortgageLoanChildDataProvider(dbFactory);
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 1);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            Assert.DoesNotThrow(() => dataProvider.LoadSubKeys(businessContext));
            //---------------Test Result -----------------------
            dbFactory.NewDb().Received(1).InReadOnlyAppContext();
        }

        [Test]
        public void LoadSubKeys_GivenSqlStatement_ShouldReturnRelevantModel()
        {
            //---------------Set up test pack-------------------
            var haloTileBusinessModel = new HaloTileBusinessModel
                {
                    BusinessKey = 1234,
                    Context = "contextOfModel",
                    BusinessKeyType = (int)GenericKeyType.Account,
                };

            var dbFactory = this.CreateDbFactory(haloTileBusinessModel);
            var dataProvider = new MortgageLoanChildDataProvider(dbFactory);
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 1);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var subKeys = dataProvider.LoadSubKeys(businessContext);
            //---------------Test Result -----------------------
            Assert.IsTrue(subKeys.Any());
            Assert.AreEqual(haloTileBusinessModel.Context, subKeys.First().Context);
            Assert.AreEqual(haloTileBusinessModel.BusinessKey, subKeys.First().BusinessKey.Key);
            Assert.AreEqual(haloTileBusinessModel.BusinessKeyType, (int)subKeys.First().BusinessKey.KeyType);
        }

        [Test]
        public void LoadSubKeys_GivenNoSqlStatementInDataProvider_ShouldReturnGivenBusinessContext()
        {
            //---------------Set up test pack-------------------
            var dbFactory = Substitute.For<IDbFactory>();
            var dataProvider = new ClientRootTileDataProvider(dbFactory);
            var businessContext = new BusinessContext("context", GenericKeyType.Account, 1);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var subKeys = dataProvider.LoadSubKeys(businessContext);
            //---------------Test Result -----------------------
            Assert.IsTrue(subKeys.Any());
            CollectionAssert.Contains(subKeys, businessContext);
        }

        private IDbFactory CreateDbFactory(HaloTileBusinessModel haloTileBusinessModel = null)
        {
            var dbFactory = Substitute.For<IDbFactory>();
            var readOnlyDbContext = Substitute.For<IReadOnlyDbContext>();
            if (haloTileBusinessModel == null)
            {
                readOnlyDbContext.Select<HaloTileBusinessModel>(Arg.Any<string>()).Returns(info => null);
            }
            else
            {
                var selectResult = new List<HaloTileBusinessModel> { haloTileBusinessModel };
                readOnlyDbContext.Select<HaloTileBusinessModel>(Arg.Any<string>()).Returns(selectResult);
            }


            dbFactory.NewDb().InReadOnlyAppContext().Returns(readOnlyDbContext);
            return dbFactory;
        }
    }
}
