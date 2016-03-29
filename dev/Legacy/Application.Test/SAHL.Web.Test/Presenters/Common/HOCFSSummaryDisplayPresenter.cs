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

namespace SAHL.Web.Test.Presenters.HOC
{
    [Ignore]
    [TestFixture]
    public class HOCFSSummaryDisplayPresenter : TestViewBase
    {
        private IHOCFSSummary _hocview;
        private IEventRaiser _initialiseEventRaiser;
        private IEventRaiser _preRenderEventRaiser;
        private IAccountRepository _accountRepo;
        private IAccountHOC _account;
        private IFinancialService financialService;


        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            base.ClearMockCache();

            // Always set up mock strategy AFTER setting up Principal

            _mockery = new MockRepository();
            _hocview = _mockery.CreateMock<IHOCFSSummary>();

            // Where we ask for HOCTestView, give us back Test as principal
            SetupMockedView(_hocview);
            SetupPrincipalCache(base.TestPrincipal);

            // We're not going to be hittinh db, mocking the hit
            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            // Create fake Account Repository
            _accountRepo = _mockery.CreateMock<IAccountRepository>();
           
            // Add to mock cache
            base.MockCache.Add(typeof(IAccountRepository).ToString(), _accountRepo);
                       
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _hocview = null;
            _mockery = null;
        }

        #endregion


        [NUnit.Framework.Test]
        public void TestEventsHookUp()
        {
            SetUpBaseEventExpectancies();

            _mockery.ReplayAll(); // Records everything done in View, where I mocked above in Set Up

            // Simulates somebody calling my presenter
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCFSSummary presenter = new HOCFSSummary(_hocview, controller);

        }

        private void SetUpBaseEventExpectancies()
        {
            // Test 1 : Check to see if Presenter hooks Init, Load n PreRender (basic events) : ie - assigning event handlers for these

            _hocview.ViewInitialised += null;
            LastCall.IgnoreArguments();
            // From the last call, get the event raiser
            // equivalent to :  _initialiseEventRaiser = new EventRaiser((IMockedObject)_hocview, "ViewInitialised");
            _initialiseEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
           
            _hocview.ViewLoaded += null;
            LastCall.IgnoreArguments();

            _hocview.ViewPreRender += null;
            LastCall.IgnoreArguments();
            _preRenderEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
        }

        

        [NUnit.Framework.Test]
        public void TestInitialiseRecordsFound()
        {
            SetUpBaseEventExpectancies();
            SetupResult.For(_hocview.Messages).Return(new DomainMessageCollection());

            _account = _mockery.CreateMock<IAccountHOC>();
            SetupResult.For(_accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(_account);

            IHOC hoc = _mockery.CreateMock<IHOC>();

            SetupResult.For(_account.HOC).Return(hoc);

            IHOCInsurer hocinsurer = _mockery.CreateMock<IHOCInsurer>();
            SetupResult.For(hoc.HOCInsurer).Return(hocinsurer);

            _hocview.BindHOCSummaryData(null);
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCFSSummary presenter = new HOCFSSummary(_hocview, controller);
          
            presenter.FinancialService = financialService as IHOC;
            _initialiseEventRaiser.Raise(_hocview, new EventArgs());

            _mockery.VerifyAll();
            
        }

        [NUnit.Framework.Test]
        public void TestInitialiseNoRecordsFound()
        {
            SetUpBaseEventExpectancies();

            SetupResult.For(_hocview.Messages).Return(new DomainMessageCollection());
            
            _account = _mockery.CreateMock<IAccountHOC>();
            SetupResult.For(_accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(_account);

            SetupResult.For(_account.HOC).Return(null);

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCFSSummary presenter = new HOCFSSummary(_hocview, controller);

            presenter.FinancialService = financialService as IHOC;
            _initialiseEventRaiser.Raise(_hocview, new EventArgs());

            _mockery.VerifyAll();

        }


        [NUnit.Framework.Test]
        public void TestPreRenderNonSAHLInsurer()
        {
            SetUpBaseEventExpectancies();

            IHOCInsurer hocInsurer = _mockery.CreateMock<IHOCInsurer>();

            SetupResult.For(hocInsurer.Key).Return(0);

            // specific values that must be set
            _hocview.HOCDetailsDisplay = true;
            _hocview.HOCDetailsUpdate = false;
            _hocview.HOCCancelButtonVisible = false;
            _hocview.HOCUpdateButtonVisible = false;
            _hocview.HOCDetailsUpdateDisplay = true;

            _hocview.HOCPremiumPanelVisible = false;

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCFSSummary presenter = new HOCFSSummary(_hocview, controller);

            presenter.HOCInsurer = hocInsurer;
            _preRenderEventRaiser.Raise(_hocview, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void TestPreRenderSAHLHOC()
        {
            SetUpBaseEventExpectancies();

            IHOCInsurer hocInsurer = _mockery.CreateMock<IHOCInsurer>();

            SetupResult.For(hocInsurer.Key).Return(2);

            // specific values that must be set
            _hocview.HOCDetailsDisplay = true;
            _hocview.HOCDetailsUpdate = false;
            _hocview.HOCCancelButtonVisible = false;
            _hocview.HOCUpdateButtonVisible = false;
            _hocview.HOCDetailsUpdateDisplay = true;

            _hocview.HOCPremiumPanelVisible = true;

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCFSSummary presenter = new HOCFSSummary(_hocview, controller);

            presenter.HOCInsurer = hocInsurer;
            
            _preRenderEventRaiser.Raise(_hocview, new EventArgs());
            _mockery.VerifyAll();
        }
    }
}
