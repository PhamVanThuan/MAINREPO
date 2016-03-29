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
    public class RateChangeBasePresenter : TestViewBase
    {
        protected IRateChange _view;
        protected IAccountRepository _accountRepo;
        protected IEventRaiser _initialiseEventRaiser;
        protected IAccount _account;
        protected ILifePolicy _life;
        protected IAccountLifePolicy _lifeAccount;
        protected IMortgageLoan _mlVar;
        protected IMortgageLoan _mlFixed;
        protected IMortgageLoanRepository _mlRepo;
        protected IReadOnlyEventList<IFinancialService> _mlVarLst;
        protected IReadOnlyEventList<IFinancialService> _mlFixedLst;
        protected IEventList<IMortgageLoan> _lstMortgageLoans;
        protected IEventRaiser _cancelButtonClickRaiser;
        protected IEventRaiser _submitButtonClickRaiser;
        protected IMortgageLoanAccount mlLifeAccount;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _mockery = new MockRepository();
            _view = _mockery.CreateMock<IRateChange>();

            SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);
      
            base.ClearMockCache();
      
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

        [NUnit.Framework.Test]
        public void TestEventsHookUp()
        {
            SetUpBaseEventExpectancies();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            RateChangeBase presenter = new RateChangeBase(_view, controller);

        }

        public void SetUpBaseEventExpectancies()
        {
            _view.ViewInitialised += null;
            LastCall.IgnoreArguments();
            _initialiseEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewLoaded += null;
            LastCall.IgnoreArguments();

            _view.ViewPreRender += null;
            LastCall.IgnoreArguments();

            _view.CancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            _cancelButtonClickRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.SubmitButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            _submitButtonClickRaiser = LastCall.IgnoreArguments().GetEventRaiser();
        }

        [NUnit.Framework.Test]
        public void TestInitialise()
        {
            SetUpBaseEventExpectancies();

            SetupResult.For(_view.Messages).Return(new DomainMessageCollection());
            _account = _mockery.CreateMock<IAccount>();
            SetupResult.For(_accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(_account);
           
            mlLifeAccount = _mockery.CreateMock<IMortgageLoanAccount>();

            _lifeAccount = _mockery.CreateMock<IAccountLifePolicy>();

            _life = _mockery.CreateMock<ILifePolicy>();
            SetupResult.For(mlLifeAccount.LifePolicyAccount).IgnoreArguments().Return(_lifeAccount);
            SetupResult.For(_lifeAccount.LifePolicy).IgnoreArguments().Return(_life);

            _mlVarLst = _mockery.CreateMock<IReadOnlyEventList<IFinancialService>>();
            _mlFixedLst = _mockery.CreateMock<IReadOnlyEventList<IFinancialService>>();

            SetupResult.For(_account.GetFinancialServicesByType(FinancialServiceTypes.VariableLoan, new AccountStatuses[] { AccountStatuses.Open })).IgnoreArguments().Return(_mlVarLst);
            SetupResult.For(_account.GetFinancialServicesByType(FinancialServiceTypes.FixedLoan,new AccountStatuses[] {AccountStatuses.Open})).IgnoreArguments().Return(_mlFixedLst);

            _mlVar = _mockery.CreateMock<IMortgageLoan>();
            SetupResult.For(_mlVarLst[0]).IgnoreArguments().Return(_mlVar as IMortgageLoan);

            SetupResult.For(_mlFixedLst.Count).IgnoreArguments().Return(0);


            _lstMortgageLoans = _mockery.CreateMock<IEventList<IMortgageLoan>>();
            
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            RateChangeBase presenter = new RateChangeBase(_view, controller);
            //presenter._mlLife = mlLifeAccount;

            _initialiseEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
          
        }



    }
}
