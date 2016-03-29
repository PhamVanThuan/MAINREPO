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
using Microsoft.ApplicationBlocks.UIProcess;
using Rhino.Mocks.Constraints;

namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class ReOpenAccountPresenter : TestViewBase
    {
        private IAccountRepository _accountRepo;
        private IAccount _account;
        private IReOpenAccount _view;
        private IEventRaiser _initialiseEventRaiser;
        private IMortgageLoan mlFixed;
        private IEventList<IMortgageLoanInfo> mlinfo;
        private IReadOnlyEventList<IFinancialService> fsVarLst;
        private IReadOnlyEventList<IFinancialService> fsFixedLst;
        protected IEventRaiser _cancelButtonClickRaiser;
        protected IEventRaiser _submitButtonClickRaiser;
        private IMortgageLoan mlVar;
        private IFinancialService fsVar;
        private IFinancialService fsFixed;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            base.ClearMockCache();

            _mockery = new MockRepository();
            _view = _mockery.CreateMock<IReOpenAccount>();

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

            _view.CancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            _cancelButtonClickRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.SubmitButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            _submitButtonClickRaiser = LastCall.IgnoreArguments().GetEventRaiser();

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
            ReOpenAccount presenter = new ReOpenAccount(_view, controller);

        }

        [Test]
        public void InitialiseTest()
        {
            SetUpBaseEventExpectancies();

            _account = _mockery.CreateMock<IAccount>();
            SetupResult.For(_accountRepo.GetAccountByKey(0)).Return(_account);

            fsVarLst = _mockery.CreateMock<IReadOnlyEventList<IFinancialService>>();
            fsFixedLst = _mockery.CreateMock<IReadOnlyEventList<IFinancialService>>();

            mlVar = _mockery.CreateMock<IMortgageLoan>();
            fsVar = _mockery.CreateMock<IFinancialService>();
            fsFixed = _mockery.CreateMock<IFinancialService>();

            SetupResult.For(_account.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Closed})).Return(fsVarLst);
            SetupResult.For(_account.GetFinancialServicesByType(FinancialServiceTypes.FixedLoan, new AccountStatuses[] { AccountStatuses.Closed })).Return(fsFixedLst);

            SetupResult.For(fsVarLst[0]).Return(fsVar);
            SetupResult.For(fsFixedLst[0]).Return(fsFixed);

            SetupResult.For(fsVarLst[0]).Return(mlVar as IMortgageLoan);
            SetupResult.For(fsFixedLst[0]).Return(mlFixed as IMortgageLoan);


        }

        [NUnit.Framework.Test]
        public void CancelButtonClick()
        {
            SetUpBaseEventExpectancies();

            ISimpleNavigator navigator = _mockery.CreateMock<ISimpleNavigator>();
            SetupResult.For(_view.Navigator).Return(navigator);

            navigator.Navigate("ReOpenAccount");

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            ReOpenAccount presenter = new ReOpenAccount(_view, controller);
            _cancelButtonClickRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void SubmitButtonClick()
        {
            SetUpBaseEventExpectancies();

            IFinancialServiceRepository fsRepo = _mockery.CreateMock<IFinancialServiceRepository>();
            IMortgageLoanRepository mlRepo = _mockery.CreateMock<IMortgageLoanRepository>();

            _accountRepo.SaveAccount(null);
            fsRepo.SaveFinancialService(null);
            mlRepo.SaveMortgageLoan(null);
            _accountRepo.RemoveDetailByAccountKeyAndDetailTypeKey(0,0);

            ISimpleNavigator navigator = _mockery.CreateMock<ISimpleNavigator>();
            SetupResult.For(_view.Navigator).Return(navigator);

            navigator.Navigate("ReOpenAccount");

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            ReOpenAccount presenter = new ReOpenAccount(_view, controller);
            _submitButtonClickRaiser.Raise(_view, new EventArgs());

            _mockery.VerifyAll();
        }

    }
}
