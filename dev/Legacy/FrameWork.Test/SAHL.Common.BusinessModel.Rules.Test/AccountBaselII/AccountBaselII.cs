using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.AccountBaselII;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Test.AccountAccountBaselII
{
    [TestFixture]
    public class AccountBaselII : RuleBase
    {
        [SetUp]
        public override void Setup()
        {
            base.Setup();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void AccountBaselIIDeclineTestPass()
        {
            string sql = @" Select BehaviouralScore " +
                           "From [2AM]..behaviouralscorecategory " +
                           "Where Description = 'Moderate risk - Refer'";

            object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
            if (obj != null)
            {
                int iBehaviouralScore = Convert.ToInt32(obj);
                AccountBaselIIDecline rule = new AccountBaselIIDecline();
                ExecuteRule(rule, 0, GetAccountBaselIIHelper(iBehaviouralScore + 1));
                ExecuteRule(rule, 0, GetApplicationBaselIIHelper(iBehaviouralScore + 1));
            }
        }

        [Test]
        public void AccountBaselIIDeclineTestFail()
        {
            string sql = @" Select BehaviouralScore " +
                            "From [2AM]..behaviouralscorecategory " +
                            "Where Description = 'High risk - Decline'";

            object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
            if (obj != null)
            {
                int iBehaviouralScore = Convert.ToInt32(obj);
                AccountBaselIIDecline rule = new AccountBaselIIDecline();
                ExecuteRule(rule, 1, GetAccountBaselIIHelper(iBehaviouralScore - 1));
                ExecuteRule(rule, 1, GetApplicationBaselIIHelper(iBehaviouralScore - 1));
            }
        }

        [Test]
        public void AccountBaselIIReferTestPass()
        {
            string sql = @" Select BehaviouralScore " +
                           "From [2AM]..behaviouralscorecategory " +
                           "Where Description = 'Moderate risk - Refer'";

            object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
            if (obj != null)
            {
                int iBehaviouralScore = Convert.ToInt32(obj);
                AccountBaselIIRefer rule = new AccountBaselIIRefer();
                ExecuteRule(rule, 0, GetAccountBaselIIHelper(iBehaviouralScore + 1));
                ExecuteRule(rule, 0, GetApplicationBaselIIHelper(iBehaviouralScore + 1));
            }
        }

        [Test]
        public void AccountBaselIIReferTestFail()
        {
            string sql = @" Select BehaviouralScore " +
                            "From [2AM]..behaviouralscorecategory " +
                            "Where Description = 'High risk - Decline'";

            object obj = CastleTransactionsServiceHelper.ExecuteScalarOnCastleTran(sql, typeof(GeneralStatus_DAO), null);
            if (obj != null)
            {
                int iBehaviouralScore = Convert.ToInt32(obj);
                AccountBaselIIRefer rule = new AccountBaselIIRefer();
                ExecuteRule(rule, 1, GetAccountBaselIIHelper(iBehaviouralScore + 1));
                ExecuteRule(rule, 1, GetApplicationBaselIIHelper(iBehaviouralScore + 1));
            }
        }

        #region Helper Methods

        private IAccount GetAccountBaselIIHelper(int bhvlScore)
        {
            IAccount account = _mockery.StrictMock<IAccount>();
            IAccountBaselII accountBasel = _mockery.StrictMock<IAccountBaselII>();
            SetupResult.For(accountBasel.BehaviouralScore).Return(bhvlScore);
            SetupResult.For(account.GetLatestBehaviouralScore()).Return(accountBasel);
            return account;
        }

        private IApplication GetApplicationBaselIIHelper(int bhvlScore)
        {
            IAccount account = _mockery.StrictMock<IAccount>();
            IApplication application = _mockery.StrictMock<IApplication>();
            IApplicationType appType = _mockery.StrictMock<IApplicationType>();
            IAccountBaselII accountBasel = _mockery.StrictMock<IAccountBaselII>();

            SetupResult.For(appType.Key).Return((int)OfferTypes.ReAdvance);
            SetupResult.For(application.ApplicationType).Return(appType);
            SetupResult.For(accountBasel.BehaviouralScore).Return(bhvlScore);
            SetupResult.For(account.GetLatestBehaviouralScore()).Return(accountBasel);
            SetupResult.For(application.Account).Return(account);
            return application;
        }

        #endregion Helper Methods
    }
}