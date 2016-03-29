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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel;
using SAHL.Web.Test.Presenters.Common;
using SAHL.Common.Factories;

namespace SAHL.Web.Test.Presenters.HOC
{
    [Ignore]
    [TestFixture]
    public class HOCFSSummaryUpdatePresenter : TestViewBase
    {
        protected IHOCFSSummary _hOCView;
        protected IAccountRepository _accountRepo;
        protected IHOCRepository hocRepo;
        protected IEventRaiser eventRaiser;
        protected IEventRaiser eventRaiserPreRender;
        protected IEventRaiser eventRaiserInitialise;
        protected IAccountHOC _account;
        protected IReadOnlyEventList<IFinancialService> financialServices;
        protected IHOC _HOC;

        private const string UpdateButtonClicked = "UpdateButtonClicked";
        private const string CancelButtonClicked = "CancelButtonClicked";

        [NUnit.Framework.Test]
        public void TestEventsHookUp()
        {
            SetUpBaseEventExpectancies();

            _mockery.ReplayAll(); // Records everything done in View, where I mocked above in Set Up

            // Simulates somebody calling my presenter
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCFSSummaryUpdate presenter = new HOCFSSummaryUpdate(_hOCView, controller);
        }

        #region SetUp/TearDown
        [SetUp]
        public void FixtureSetup()
        {
            base.ClearMockCache();

            _mockery = new MockRepository();
            _hOCView = _mockery.CreateMock<IHOCFSSummary>();

            SetupMockedView(_hOCView);
            SetupPrincipalCache(base.TestPrincipal);

            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            _accountRepo = _mockery.CreateMock<IAccountRepository>();
            hocRepo = _mockery.CreateMock<IHOCRepository>();
            base.MockCache.Add(typeof(IAccountRepository).ToString(), _accountRepo);
                       
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _hOCView = null;
            _mockery = null;
        }

        #endregion

       

        [NUnit.Framework.Test]
        public void TestInitialise()
        {
            SetUpBaseEventExpectancies();

            SetupResult.For(_hOCView.Messages).Return(new DomainMessageCollection());
            _account = _mockery.CreateMock<IAccountHOC>();

            SetupResult.For(_accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(_account);

            _HOC = _mockery.CreateMock<IHOC>();
            SetupResult.For(_account.HOC).IgnoreArguments().Return(_HOC);


            IHOCInsurer hocinsurer = _mockery.CreateMock<IHOCInsurer>();
            SetupResult.For(_HOC.HOCInsurer).Return(hocinsurer);

            _hOCView.BindHOCSummaryData(null);
            LastCall.IgnoreArguments();

            IDictionary<int, string> fakeLookup = new Dictionary<int, string>();

            //SetupResult.For(base.LookupRepository.HOCInsurers.BindableDictionary).Return(fakeLookup);
            //SetupResult.For(base.LookupRepository.HOCStatus.BindableDictionary).Return(fakeLookup);
            //SetupResult.For(base.LookupRepository.HOCSubsidence.BindableDictionary).Return(fakeLookup);
            //SetupResult.For(base.LookupRepository.HOCConstruction.BindableDictionary).Return(fakeLookup);       

            _mockery.ReplayAll();
            
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCFSSummaryUpdate presenter = new HOCFSSummaryUpdate(_hOCView, controller);
            presenter.HOCAccount = _HOC;

            eventRaiserInitialise.Raise(_hOCView, new EventArgs());

            _mockery.VerifyAll();
        }


        private void SetUpBaseEventExpectancies()
        {
            _hOCView.ViewInitialised += null;
            LastCall.IgnoreArguments();
            eventRaiserInitialise = LastCall.IgnoreArguments().GetEventRaiser();

            _hOCView.ViewLoaded += null;
            LastCall.IgnoreArguments();

            _hOCView.ViewPreRender += null;
            LastCall.IgnoreArguments();
            eventRaiserPreRender = LastCall.IgnoreArguments().GetEventRaiser();
        }

        [NUnit.Framework.Test]
        public void TestPreRender()
        {
            _hOCView.HOCCancelButtonVisible = false;
            _hOCView.HOCUpdateButtonVisible = false;

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            HOCFSSummaryUpdate presenter = new HOCFSSummaryUpdate(_hOCView, controller);

            eventRaiserPreRender.Raise(_hOCView, new EventArgs());
            _mockery.VerifyAll();
        }

    

        protected IAccountRepository AccountRepository
        {
            get
            {
                return _accountRepo;
            }
        }

        [NUnit.Framework.Test]
        public void TestUpdateClicked()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_hOCView)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser addClickEventRaiser = dictEvents[UpdateButtonClicked];

            IHOC _HOC = _mockery.CreateMock<IHOC>();

            SetupResult.For(_hOCView.GetCapturedHOC(null)).IgnoreArguments().Return(_HOC);

            hocRepo.SaveHOC(null);
        
            LastCall.IgnoreArguments();

            SetupNavigationMock(_hOCView, "HOCFSSummary");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();

            HOCFSSummaryUpdate presenter = new HOCFSSummaryUpdate(_hOCView, controller);
            initEventRaiser.Raise(_hOCView, new EventArgs());
            addClickEventRaiser.Raise(_hOCView, new EventArgs());
            _mockery.VerifyAll();

        }

        private IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            return dict;
        }

    
    }
}
