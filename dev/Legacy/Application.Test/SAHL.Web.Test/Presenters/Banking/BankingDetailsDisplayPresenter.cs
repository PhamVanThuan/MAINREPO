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

namespace SAHL.Web.Test.Presenters.Banking
{
    [Ignore]
    [TestFixture]
    public class BankingDetailsDisplayPresenter : TestViewBase
    {
        private IBankingDetails _view;
        private CacheManager _CM;
        private ILegalEntity _legalEntity;
        private const string OnSubmitButtonClicked = "OnSubmitButtonClicked";
        private const string OnSearchBankAccountClicked = "OnSearchBankAccountClicked";
        private IDomainMessageCollection _messages;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<IBankingDetails>();
            base.SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal, true);
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _view = null;
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
            BankingDetailsDisplay presenter = new BankingDetailsDisplay(_view, controller);
            presenter.LegalEntity = _legalEntity;
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [Test]
        public void PreRenderTest()
        {
            // Hook up life cycle events and return the view initialized event raiser

            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];

            _view.ControlsVisible = false;
            _view.ShowButtons = false;
            _view.ShowStatus = false;
            _view.BankAccountGridEnabled = false;

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            BankingDetailsDisplay presenter = new BankingDetailsDisplay(_view, controller);
            presenter.LegalEntity = _legalEntity;
            initEventRaiser.Raise(_view, new EventArgs());
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

            ILegalEntityRepository LER = _mockery.CreateMock<ILegalEntityRepository>();
            _CM.Add(typeof(ILegalEntityRepository).ToString(), LER);

            _messages = _mockery.CreateMock<IDomainMessageCollection>();

            IEventList<ILegalEntityBankAccount> lstBanksAccounts = _mockery.CreateMock<IEventList<ILegalEntityBankAccount>>();
            _legalEntity = _mockery.CreateMock<ILegalEntity>();

            SetupResult.For(LER.GetLegalEntityByKey(-1)).IgnoreArguments().Return(_legalEntity);

            SetupResult.For(_legalEntity.LegalEntityBankAccounts).Return(lstBanksAccounts);

            ILookupRepository lookups = _mockery.CreateMock<ILookupRepository>();

            _CM.Add(typeof(ILookupRepository).ToString(), lookups);

            IEventList<IACBBank> banks = _mockery.CreateMock<IEventList<IACBBank>>();
            SetupResult.For(lookups.Banks).Return(banks);

            IEventList<IACBType> types = _mockery.CreateMock<IEventList<IACBType>>();
            SetupResult.For(lookups.BankAccountTypes).Return(types);


            IEventList<IGeneralStatus> statuses = _mockery.CreateMock<IEventList<IGeneralStatus>>();
            SetupResult.For(lookups.GeneralStatuses).Return(statuses);



            // make sure we are calling the binds from initialise            
            BindOnInitialise();

            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            _view.OnCancelClicked += null;
            IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnCancelClicked", EventRaiser);

            _view.OnSearchBankAccountClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnSearchBankAccountClicked", EventRaiser);

            _view.OnSubmitButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnSubmitButtonClicked", EventRaiser);

            return dict;
        }

        /// <summary>
        /// IContact view Binding during initialise.
        /// </summary>
        private void BindOnInitialise()
        {
            _view.BindGridForLegalEntityBankAccounts(null);
            LastCall.IgnoreArguments();

            _view.BindBankAccountControls(null, null, null);
            LastCall.IgnoreArguments();

        }

        #endregion

    }
}