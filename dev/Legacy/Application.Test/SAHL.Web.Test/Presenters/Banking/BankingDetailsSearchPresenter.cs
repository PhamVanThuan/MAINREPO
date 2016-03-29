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
using SAHL.Web.Views.Common.Presenters;
using SAHL.Web.Views.Common.Presenters.Banking;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common;

namespace SAHL.Web.Test.Presenters.Banking
{
    [Ignore]
    [TestFixture]
    public class BankingDetailsSearchPresenter : TestViewBase
    {

        private IBankingDetailsSearch _view;
        private CacheManager _CM;
        private const string UseButtonClicked = "OnUseButtonClicked";

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<IBankingDetailsSearch>();
            base.SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal, true);
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _view = null;
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
            BankingDetailsSearch presenter = new BankingDetailsSearch();          
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [Test]
        public void OnUseButtonClickedTest()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser updateClickEventRaiser = dictEvents[UseButtonClicked];

            IBankAccountRepository BAR = _mockery.CreateMock<IBankAccountRepository>();
            IBankAccount ba = _mockery.CreateMock<IBankAccount>();
            SetupResult.For(_view.BankAcccount).Return(ba);

            ILegalEntityRepository LER = _mockery.CreateMock<ILegalEntityRepository>();

            INavigator nav = _mockery.CreateMock<INavigator>();

            Expect.Call(_view.Navigator).Return(nav).Repeat.Any();

        //    nav.Navigate("");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            BankingDetailsSearch presenter = new BankingDetailsSearch();
            initEventRaiser.Raise(_view, new EventArgs());
            updateClickEventRaiser.Raise(_view, new KeyChangedEventArgs(null));
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

            IEventList<ILegalEntityBankAccount> lstAccounts = _mockery.CreateMock<IEventList<ILegalEntityBankAccount>>();            
           
            // make sure we are calling the binds from initialise            
            BindOnInitialise();

            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            _view.OnCancelButtonClicked += null;
            IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnCancelButtonClicked", EventRaiser);

            _view.OnSearchGridSelectedIndexChanged += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnSearchGridSelectedIndexChanged", EventRaiser);

            _view.OnUseButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnUseButtonClicked", EventRaiser);

            return dict;
        }

        /// <summary>
        /// IContact view Binding during initialise.
        /// </summary>
        private void BindOnInitialise()
        {
            _view.BindSearchGrid(null);
            LastCall.IgnoreArguments();

        }

        #endregion


    }
}
