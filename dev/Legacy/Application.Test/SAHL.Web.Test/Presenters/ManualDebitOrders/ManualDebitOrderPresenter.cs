using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Web;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Life.Interfaces;
using Rhino.Mocks;
using NUnit.Framework;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Presenters;
using Rhino.Mocks.Interfaces;
using SAHL.Test;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Collections;
using SAHL.Common.Globals;
using SAHL.Web.Views.Common.Presenters.Banking;
using SAHL.Common.BusinessModel.SearchCriteria;
using SAHL.Web.Views.Common.Presenters;
using SAHL.Web.Views.Common.Presenters.ManualDebitOrder;

namespace SAHL.Web.Test.Presenters.ManualDebitOrders
{
    [Ignore]
    [TestFixture]
    public class ManualDebitOrderPresenter : TestViewBase
    {
        private IManualDebitOrder _view;
        private CacheManager _CM;
        private IDomainMessageCollection _messages;
        private const string OnSubmitButtonClicked = "OnSubmitButtonClicked";
        private const string OnCancelButtonClicked = "OnCancelButtonClicked";
        private IEventList<IFinancialServiceRecurringTransaction> _recurringTransactions;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<IManualDebitOrder>();
            base.SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal, true);
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _view = null;
            _recurringTransactions = null;
            _messages = null;
            _CM = null;
        }

        #endregion

        [Test]
        public void InitialiseTest()
        {
            // Hook up life cycle events and return the view initialized event raiser
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            //Hookup IContact view initialisation events.
            OnInitialiseCommon();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            ManualDebitOrder presenter = new ManualDebitOrder(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [Test]
        public void PreRenderTest()
        {
            // Hook up life cycle events and return the view initialized event raiser

            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];

            _view.ShowButtons = false;
            _view.ArrearBalanceRowVisible = false;

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            ManualDebitOrder presenter = new ManualDebitOrder(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }


        [Test]
        public void CancelButtonClickedTest()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser cancelClickEventRaiser = dictEvents[OnCancelButtonClicked];

            //todo : Add cancel button logic

            //INavigator nav = _mockery.CreateMock<INavigator>();

            //Expect.Call(_view.Navigator).Return(nav).Repeat.Any();

            //nav.Navigate("CallSummary");


            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            ManualDebitOrder presenter = new ManualDebitOrder(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            cancelClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [Test]
        public void SubmitButtonClickedTest()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser submitClickEventRaiser = dictEvents[OnSubmitButtonClicked];

            //todo : Add submit button logic

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            ManualDebitOrder presenter = new ManualDebitOrder(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            submitClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        #region Private Helpers

        /// <summary>
        /// Hooks the IContact view events.
        /// </summary>
        /// <returns>A Dictionary collection of IEventRaiser objects, one for each IContact view event.</returns>
        private IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ClearMockCache();

            _CM = CacheFactory.GetCacheManager("MOCK");

            _messages = _mockery.CreateMock<IDomainMessageCollection>();

            IAccountRepository AR = _mockery.CreateMock<IAccountRepository>();
            _CM.Add(typeof(IAccountRepository).ToString(), AR);

            SAHL.Common.BusinessModel.Interfaces.IAccount account = _mockery.CreateMock<SAHL.Common.BusinessModel.Interfaces.IAccount>();
//TODO: wayne fix this not to use this call anymore
//            SetupResult.For(AR.GetAccountByFinancialServiceKey( -1)).IgnoreArguments().Return(account);

            IFinancialService fs = _mockery.CreateMock<IFinancialService>();

            SetupResult.For(account.GetFinancialServiceByType(FinancialServiceTypes.VariableLoan)).IgnoreArguments().Return(fs);

            _recurringTransactions = _mockery.CreateMock<IEventList<IFinancialServiceRecurringTransaction>>();

            //SetupResult.For(fs.FinancialServiceRecurringTransactions).Return(recurringTransactions);
            Expect.Call(fs.FinancialServiceRecurringTransactions).Return(_recurringTransactions);

            SetupResult.For(_recurringTransactions.Count).Return(1);


            // make sure we are calling the binds from initialise            
            BindOnInitialise();

            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            _view.OnCancelButtonClicked += null;
            IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnCancelButtonClicked", EventRaiser);

            _view.OnSubmitButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnSubmitButtonClicked", EventRaiser);

            _view.OnGridSelectedIndexChanged += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnGridSelectedIndexChanged", EventRaiser);

            return dict;
        }

        /// <summary>
        /// IContact view Binding during initialise.
        /// </summary>
        private void BindOnInitialise()
        {
            _view.BindOrdersToGrid(null);
            LastCall.IgnoreArguments();

            //IEventList<IFinancialServiceRecurringTransaction> lstTransactions = _mockery.CreateMock<IEventList<IFinancialServiceRecurringTransaction>>();


            //IFinancialServiceRecurringTransaction trans = _mockery.CreateMock<IFinancialServiceRecurringTransaction>();
            //IFinancialService fs = _mockery.CreateMock<IFinancialService>();
            //SetupResult.For(fs.FinancialServiceRecurringTransactions).Return(lstTransactions);
            //SetupResult.For(lstTransactions[0]).IgnoreArguments().Return(trans);

            IFinancialServiceRecurringTransaction trans = _mockery.CreateMock<IFinancialServiceRecurringTransaction>();
            SetupResult.For(_recurringTransactions[0]).IgnoreArguments().Return(trans);
            LastCall.IgnoreArguments();
        }

        #endregion

    }
}

