using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using Rhino.Mocks.Interfaces;
using SAHL.Web.Views.Common.Presenters;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks.Impl;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;


namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class SuperLoLoyaltyInfoPresenter  : TestViewBase
    {
        private IAccountRepository _accountRepo;
        private IAccount _account;
        private ISuperLoLoyaltyInfo _view;
        private IEventRaiser _initialiseEventRaiser;
        private IMortgageLoan _mlVar;
        private IEventList<IMortgageLoanInfo> mlinfo;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            base.ClearMockCache();

            _mockery = new MockRepository();
            _view = _mockery.CreateMock<ISuperLoLoyaltyInfo>();

            SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);

            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            _accountRepo = _mockery.CreateMock<IAccountRepository>();

            base.MockCache.Add(typeof(IAccountRepository).ToString(), _accountRepo);

        }

        [TearDown]
        public void FixtureTearDown()
        {
            _view = null;
            _mockery = null;
        }

        #endregion


        private void SetUpBaseEventExpectancies()
        {
            _view.ViewInitialised += null;
            LastCall.IgnoreArguments();
            _initialiseEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewLoaded += null;
            LastCall.IgnoreArguments();

            _view.ViewPreRender += null;
            LastCall.IgnoreArguments();
        }

        [NUnit.Framework.Test]
        public void TestEventsHookUp()
        {
            SetUpBaseEventExpectancies();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            SuperLoLoyaltyInfo presenter = new SuperLoLoyaltyInfo(_view, controller);

        }

        [Test]
        public void InitialiseTest()
        {
            SetUpBaseEventExpectancies();

            SetupResult.For(_view.Messages).Return(new DomainMessageCollection());
            _account = _mockery.CreateMock<IAccount>();
            SetupResult.For(_accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(_account);

            IEventList<IFinancialService> financialServices = _mockery.CreateMock<IEventList<IFinancialService>>();
            SetupResult.For(_account.FinancialServices).IgnoreArguments().Return(financialServices);

            SetupResult.For(financialServices.Count).IgnoreArguments().Return(1);
            


            //for (int i = 0; i < account.FinancialServices.Count; i++)
            //{
            //    if (account.FinancialServices[i].FinancialServiceType.Key == 1)
            //    {
            //        List<int> tranTypes = new List<int>();
            //        tranTypes.Add(237); // Halo is currently using 560 and 1560 - why ???
            //        tranTypes.Add(1237);
            //        DataTable dtLoanTransactions = account.FinancialServices[i].GetTransactions(_view.Messages, (int)GeneralStatuses.Active, tranTypes);
            //        _view.BindLoyaltyBenefitPaymentGrid(dtLoanTransactions);
            //    }
            //}

            //_mlVar = _mockery.CreateMock<IMortgageLoan>();
            //SetupResult.For(_account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan)).Return(_mlVar);

            //mlinfo = _mockery.CreateMock<IEventList<IMortgageLoanInfo>>();
            //SetupResult.For(_mlVar.MortgageLoanInfoes).Return(mlinfo);

            //IMortgageLoanInfo mlInfoOpen = _mockery.CreateMock<IMortgageLoanInfo>();
            //SetupResult.For(mlinfo[0]).Return(mlInfoOpen);

            //SetupResult.For(mlinfo.Count).Return(1);
            //SetupResult.For(_mlVar.MortgageLoanInfoes[0].GeneralStatusKey).Return(1);

            //_view.BindLoyaltyBenefitInfo(mlInfoOpen);
            //LastCall.IgnoreArguments();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            SuperLoLoyaltyInfo presenter = new SuperLoLoyaltyInfo(_view, controller);
            _initialiseEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();

        }

    }
}
