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
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class HOCHistoryDisplayPresenter : TestViewBase
    {
        private IHOCHistoryView _hocHistoryView;
        private IAccountRepository _accountRepo;
        private IEventList<IHOCHistory> hocHistory;
        private IEventRaiser _preRenderEventRaiser;
        private IEventRaiser _initialiseEventRaiser;
        private IAccountHOC _hocAccount;
        private IHOC _HOC;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _mockery = new MockRepository();
            _hocHistoryView = _mockery.CreateMock<IHOCHistoryView>();

            SetupMockedView(_hocHistoryView);
            SetupPrincipalCache(base.TestPrincipal);

            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            _accountRepo = _mockery.CreateMock<IAccountRepository>();

            base.ClearMockCache();
            // Add to mock cache
            base.MockCache.Add(typeof(IAccountRepository).ToString(), _accountRepo);

        }

       
        [TearDown]
        public void FixtureTearDown()
        {
            _hocHistoryView = null;
            _hocHistoryView = null;
        }

        #endregion

        private void SetUpBaseEventExpectancies()
        {
            _hocHistoryView.ViewInitialised += null;
            LastCall.IgnoreArguments();
            _initialiseEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _hocHistoryView.ViewLoaded += null;
            LastCall.IgnoreArguments();

            _hocHistoryView.ViewPreRender += null;
            LastCall.IgnoreArguments();
            _preRenderEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
        }

        [NUnit.Framework.Test]
        public void TestEventsHookUp()
        {
            SetUpBaseEventExpectancies();

            _mockery.ReplayAll(); 

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCHistory presenter = new HOCHistory(_hocHistoryView, controller);
        }

        [NUnit.Framework.Test]
        public void TestInitialise()
        {
            SetUpBaseEventExpectancies();

            SetupResult.For(_hocHistoryView.Messages).Return(new DomainMessageCollection());

            InitialiseCommon();

            _hocHistoryView.SetPostBackType();

            _hocHistoryView.BindHOCHistoryGrid(null);
            LastCall.IgnoreArguments();

            _hocHistoryView.OnHOCHistoryGridsSelectedIndexChanged += null;
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCHistory presenter = new HOCHistory(_hocHistoryView, controller);

            _initialiseEventRaiser.Raise(_hocHistoryView, new EventArgs());

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void TestPreRender()
        {
            SetUpBaseEventExpectancies();
            SetupResult.For(_hocHistoryView.Messages).Return(new DomainMessageCollection());

            InitialiseCommon();

            int _gridIndexSelected = 0;

            IEventList<IHOCHistoryDetail> hocHistoryDetail = _mockery.CreateMock<IEventList<IHOCHistoryDetail>>();
            
            IHOCHistory hochist = _mockery.CreateMock<IHOCHistory>();
            SetupResult.For(hocHistory[_gridIndexSelected]).IgnoreArguments().Return(hochist);

            SetupResult.For(hochist.HOCHistoryDetails).Return(hocHistoryDetail);

            _hocHistoryView.BindHOCDetailGrid(null);
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCHistory presenter = new HOCHistory(_hocHistoryView, controller);
            presenter.HOCAccount = _hocAccount;

            _preRenderEventRaiser.Raise(_hocHistoryView, new EventArgs());

            _mockery.VerifyAll();
        }

        private void InitialiseCommon()
        {
            _hocAccount = _mockery.CreateMock<IAccountHOC>();
            SetupResult.For(_accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(_hocAccount);

            _HOC = _mockery.CreateMock<IHOC>();
            SetupResult.For(_hocAccount.HOC).Return(_HOC);

            hocHistory = _mockery.CreateMock<IEventList<IHOCHistory>>();
            SetupResult.For(_HOC.HOCHistories).Return(hocHistory);

        }


    }
}
