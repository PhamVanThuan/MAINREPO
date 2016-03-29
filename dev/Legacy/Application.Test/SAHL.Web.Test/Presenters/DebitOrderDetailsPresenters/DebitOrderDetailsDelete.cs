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
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Collections;
using SAHL.Common.Globals;
using SAHL.Web.Views.Common.Presenters.Banking;
using SAHL.Web.Views.Common.Presenters;
using SAHL.Web.Views.Common.Presenters.DebitOrderDetailsFinancialService;
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Web.Test.Presenters.DebitOrderDetailsPresenters
{
    [Ignore]
    [TestFixture]
    public class DebitOrderDetailsDeletePresenter : TestViewBase
    {
        private IDebitOrderDetails _view;
        private CacheManager _CM;
        CBOMenuNode _node;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<IDebitOrderDetails>();
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
            DebitOrderDetailsFSDelete presenter = new DebitOrderDetailsFSDelete(_view, controller);
            presenter.MenuNode = _node;
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [Test]
        public void PreRenderTest()
        {

            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];

            _view.ShowButtons = true;
            _view.ShowControls = false;
            _view.ShowLabels = false;
            _view.SubmitButtonText = "Delete";

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            DebitOrderDetailsFSDelete presenter = new DebitOrderDetailsFSDelete(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ClearMockCache();

            _CM = CacheFactory.GetCacheManager("MOCK");

            _node = _mockery.CreateMock<CBOMenuNode>(new Dictionary<string, object>());

            Int64 mInt = 1;
            SetupResult.For(_node.GenericKey).Return(mInt);

            IFinancialServiceRepository FSR = _mockery.CreateMock<IFinancialServiceRepository>();
            _CM.Add(typeof(IFinancialServiceRepository).ToString(), FSR);

            IFinancialService fs = _mockery.CreateMock<IFinancialService>();

            IEventList<IFinancialServiceBankAccount> lstBankAccounts = _mockery.CreateMock<IEventList<IFinancialServiceBankAccount>>();

            SetupResult.For(FSR.GetFinancialServiceByKey( -1)).IgnoreArguments().Return(fs);

            SetupResult.For(fs.FinancialServiceBankAccounts).Return(lstBankAccounts);

            SetupResult.For(lstBankAccounts.Count).Return(1);

            IFinancialServiceBankAccount fsBA = _mockery.CreateMock<IFinancialServiceBankAccount>();


            // make sure we are calling the binds from initialise            
            BindOnInitialise();

            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            _view.OnCancelButtonClicked += null;
            IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnCancelButtonClicked", EventRaiser);

            _view.OnSubmitButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnSubmitButtonClicked", EventRaiser);

            return dict;
        }

        private void BindOnInitialise()
        {
            _view.BindGrid(null);
            LastCall.IgnoreArguments();


        }

    }
}
