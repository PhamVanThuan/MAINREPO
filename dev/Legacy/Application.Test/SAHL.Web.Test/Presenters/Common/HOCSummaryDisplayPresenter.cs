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
    public class HOCSummaryDisplayPresenter : TestViewBase
    {
        private IHOCSummary _hocview;
        private IEventRaiser _initialiseEventRaiser;
        private IAccountRepository _accountRepo;
        private IAccountHOC _account;
        // private IReadOnlyEventList<IFinancialService> financialServices;


        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            // Always set up mock strategy AFTER setting up Principal

            _mockery = new MockRepository();
            _hocview = _mockery.CreateMock<IHOCSummary>();

            // Where we ask for HOCTestView, give us back Test as principal
            SetupMockedView(_hocview);
            SetupPrincipalCache(base.TestPrincipal);

            // We're not going to be hitting db, mocking the hit
            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            // Create fake Account Repository
            _accountRepo = _mockery.CreateMock<IAccountRepository>();

            base.ClearMockCache();
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
            HOCSummary presenter = new HOCSummary(_hocview, controller);
        }

        private void SetUpBaseEventExpectancies()
        {
            // Check to see if Presenter hooks Init, Load n PreRender (basic events) : ie - assigning event handlers for these

            _hocview.ViewInitialised += null;
            LastCall.IgnoreArguments();

            // From the last call, get the event raiser
            // equivalent to :  _initialiseEventRaiser = new EventRaiser((IMockedObject)_hocview, "ViewInitialised");
            _initialiseEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _hocview.ViewLoaded += null;
            LastCall.IgnoreArguments();

            _hocview.ViewPreRender += null;
            LastCall.IgnoreArguments();
        }

        [NUnit.Framework.Test]
        public void TestInitialiseRecordsFound()
        {
            SetUpBaseEventExpectancies();

            // This is the value to use for view.messages : whenver anyone asks for messages, return this
            SetupResult.For(_hocview.Messages).Return(new DomainMessageCollection());
            
            _account = _mockery.CreateMock<IAccountHOC>();
            SetupResult.For(_accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(_account);

            IHOC HOC = _mockery.CreateMock<IHOC>();

            SetupResult.For(_account.HOC).Return(HOC);
            
            _hocview.BindDetailDataGrid(null);
            LastCall.IgnoreArguments();

            _hocview.BindMasterDataControls(null);
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCSummary presenter = new HOCSummary(_hocview, controller);

            _initialiseEventRaiser.Raise(_hocview, new EventArgs());

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void TestInitialiseNoRecordsFound()
        {
            SetUpBaseEventExpectancies();

            // This is the value to use for view.messages : whenver anyone asks for messages, return this
            SetupResult.For(_hocview.Messages).Return(new DomainMessageCollection());

            _account = _mockery.CreateMock<IAccountHOC>();
            SetupResult.For(_accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(_account);

            SetupResult.For(_account.HOC).Return(null);

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCSummary presenter = new HOCSummary(_hocview, controller);

            _initialiseEventRaiser.Raise(_hocview, new EventArgs());

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void TestPreRender()
        {
            SetUpBaseEventExpectancies();

            IEventRaiser eventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
         
            HOCSummary presenter = new HOCSummary(_hocview, controller);
            eventRaiser.Raise(_hocview, new EventArgs());
          
            _mockery.VerifyAll();
        }
    }
}
