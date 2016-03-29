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
    public class FixedDebitOrderDisplayPresenter : TestViewBase
    {
        private IFixedDebitOrderSummary _view;
        protected IEventRaiser _initialiseEventRaiser;
        protected IEventRaiser _preRenderEventRaiser;
        protected IAccount _account;
        protected IFutureDatedChange _futureDatedChange;
        protected IAccountRepository _accountRepo;
        protected IFutureDatedChangeRepository _fdcRepo;
        protected IMortgageLoan _mlVar;
        protected IMortgageLoan _mlFixed;
        protected bool IsProspectIntOnly;
        protected IEventList<IAccount> accountList;
        protected IList<IFutureDatedChange> _futureDatedChangeLst;
        protected IReadOnlyEventList<IFinancialService> _mlFsVar;
        protected IReadOnlyEventList<IFinancialService> _mlFsFixed;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _mockery = new MockRepository();
            _view = _mockery.CreateMock<IFixedDebitOrderSummary>();

            SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);

            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            _accountRepo = _mockery.CreateMock<IAccountRepository>();
            _fdcRepo = _mockery.CreateMock<IFutureDatedChangeRepository>();

            base.ClearMockCache();
            base.MockCache.Add(typeof(IAccountRepository).ToString(), _accountRepo);
            base.MockCache.Add(typeof(IFutureDatedChangeRepository).ToString(), _fdcRepo);

        }

        [TearDown]
        public void FixtureTearDown()
        {
            _view = null;
            _mockery = null;
        }

        #endregion

        [NUnit.Framework.Test]
        private void TestEventsHookUp()
        {
            SetUpBaseEventExpectancies();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            FixedDebitOrderSummary presenter = new FixedDebitOrderSummary(_view, controller);

        }

        private void SetUpBaseEventExpectancies()
        {
            _view.ViewInitialised += null;
            LastCall.IgnoreArguments();
            _initialiseEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewLoaded += null;
            LastCall.IgnoreArguments();

            _view.ViewPreRender += null;
            LastCall.IgnoreArguments();
            _preRenderEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
        }


        [NUnit.Framework.Test]
        private void TestInitialise()
        {
            SetUpBaseEventExpectancies();

            OnInitialiseCommon();

            SetupResult.For(_account.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Open })).IgnoreArguments().Return(_mlFsVar);

            SetupResult.For(_account.GetFinancialServicesByType(FinancialServiceTypes.FixedLoan, new AccountStatuses[] { AccountStatuses.Open })).IgnoreArguments().Return(_mlFsFixed);

            SetupResult.For(_mlFsVar[0]).IgnoreArguments().Return(_mlVar as IMortgageLoan);
            SetupResult.For(_mlFsFixed.Count).IgnoreArguments().Return(0);
            SetupResult.For(_mlVar.HasInterestOnly()).Return(false);

            SetupResult.For(_fdcRepo.GetFutureDatedChangesByGenericKey(0, 0)).IgnoreArguments().Return(_futureDatedChangeLst);

            SetupResult.For(_account.GetNonProspectRelatedAccounts()).Return(accountList);

            _view.selectedFirstRow = false;
            _view.BindFutureDatedDOGrid(_futureDatedChangeLst);
            _view.ShowInterestOnly = IsProspectIntOnly;
            _view.BindAccountSummaryGrid(accountList);
            _view.BindFixedDebitOrderData(accountList, _mlVar);

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            FixedDebitOrderSummary presenter = new FixedDebitOrderSummary(_view, controller);
            _initialiseEventRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void TestPreRender()
        {
            SetUpBaseEventExpectancies();
            _view.ShowButtons = false;
            _view.ShowUpdateableControl = false;

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            FixedDebitOrderSummary presenter = new FixedDebitOrderSummary(_view, controller);

            _preRenderEventRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();
        }

        public void OnInitialiseCommon()
        {
            SetupResult.For(_view.Messages).Return(new DomainMessageCollection());
            _account = _mockery.CreateMock<IAccount>();
            SetupResult.For(_accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(_account);
            _futureDatedChange = _mockery.CreateMock<IFutureDatedChange>();
            SetupResult.For(_fdcRepo.GetFutureDatedChangesByGenericKey( 0, 0)).IgnoreArguments().Return(_futureDatedChangeLst);
            _mlVar = _mockery.CreateMock<IMortgageLoan>();
            _mlFixed = _mockery.CreateMock<IMortgageLoan>();
            _futureDatedChangeLst = _mockery.CreateMock<IList<IFutureDatedChange>>();
            _mlFsVar = _mockery.CreateMock<IReadOnlyEventList<IFinancialService>>();
            _mlFsFixed = _mockery.CreateMock<IReadOnlyEventList<IFinancialService>>();

        }

    }
}
