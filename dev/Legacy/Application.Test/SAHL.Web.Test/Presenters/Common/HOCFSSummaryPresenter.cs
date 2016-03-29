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
namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class HOCFSSummaryPresenter : TestViewBase
    {
        protected IHOCFSSummary _hOCView;
        protected IAccountRepository _accountRepo;
        protected IEventRaiser eventRaiser;
        protected IEventRaiser eventRaiserPreRender;
        protected IEventRaiser eventRaiserInitialise;
        protected IAccountHOC _account;
        protected IReadOnlyEventList<IFinancialService> financialServices;
        protected  IHOC _HOC;

        #region Setup/TearDown

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
        public void TestEventsHookUp()
        {
            SetUpBaseEventExpectancies();

            _mockery.ReplayAll(); // Records everything done in View, where I mocked above in Set Up

            // Simulates somebody calling my presenter
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCFSSummary presenter = new HOCFSSummary(_hOCView, controller);
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
        public void TestInitialiseNoRecords()
        {
            SetUpBaseEventExpectancies();

            SetupResult.For(_hOCView.Messages).Return(new DomainMessageCollection());
            _account = _mockery.CreateMock<IAccountHOC>();

            SetupResult.For(_accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(_account);

            IFinancialService financialService = _mockery.CreateMock<IHOC>();
            SetupResult.For(_account.HOC).IgnoreArguments().Return(null);

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCFSSummary presenter = new HOCFSSummary(_hOCView, controller);
            presenter.FinancialService = financialService as IHOC;

            eventRaiserInitialise.Raise(_hOCView, new EventArgs());

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void TestInitialiseRecordsFound()
        {
            SetUpBaseEventExpectancies();

            SetupResult.For(_hOCView.Messages).Return(new DomainMessageCollection());
            _account = _mockery.CreateMock<IAccountHOC>();

            SetupResult.For(_accountRepo.GetAccountByKey(0)).IgnoreArguments().Return(_account);

            _HOC = _mockery.CreateMock<IHOC>();
            SetupResult.For(_account.HOC).IgnoreArguments().Return(_HOC);

            IHOCInsurer hocInsurer = _mockery.CreateMock<IHOCInsurer>();
            SetupResult.For(_HOC.HOCInsurer).IgnoreArguments().Return(hocInsurer);
         
            _hOCView.BindHOCSummaryData(null);
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            HOCFSSummary presenter = new HOCFSSummary(_hOCView, controller);
            presenter.HOCInsurer = hocInsurer;

            eventRaiserInitialise.Raise(_hOCView, new EventArgs());

            _mockery.VerifyAll();

        }



    }
}
