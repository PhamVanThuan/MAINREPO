using System;
using System.Collections.Generic;
using System.Text;
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

namespace SAHL.Web.Test.Presenters.Life
{
    [Ignore]
    [TestFixture]
    public class ContactPresenterTest : LifePresenterBaseTest
    {
        private IContact _view;
        private IDomainMessageCollection _messages = new DomainMessageCollection();
        private IApplication _offer;
        private CacheManager _CM;
        private IApplicationRepository _AR;
        private SAHL.Common.BusinessModel.Interfaces.IAccount _account;
        private IReadOnlyEventList<ILegalEntity> _lstPersons;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<IContact>();
            base.SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _view = null;
            _messages = null;
            _offer = null;
            _CM = null;
            _AR = null;
            _account = null;
            _lstPersons = null;
        }

        #endregion

        [NUnit.Framework.Test]
        public void InitialiseTest()
        {
            // Hook up life cycle events and return the view initialized event raiser
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            //Hookup IContact view initialisation events.
            OnInitialiseCommon();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            Contact presenter = new Contact(_view, controller);
            presenter.ApplicationRepo = _AR;
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void PreRenderTest()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            Contact presenter = new Contact(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void OnAddAddressButtonClickedTest()
        {

            // Hook up life cycle events and return the view initialized event raiser
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IEventRaiser OnAddAddressButtonEventRaiser = OnInitialiseCommon()["OnAddAddressButtonClicked"];


            INavigator nav = _mockery.CreateMock<INavigator>();
            SAHLCommonBaseController controller = new SAHLCommonBaseController(nav);
            nav.Navigate("AddAddress");
            _mockery.ReplayAll();
            Contact presenter = new Contact(_view, controller);
            presenter.ApplicationRepo = _AR;
            initEventRaiser.Raise(_view, new EventArgs());
            OnAddAddressButtonEventRaiser.Raise( null);
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

            _AR = _mockery.CreateMock<IApplicationRepository>();
            _CM.Add(typeof(IApplicationRepository).ToString(), _AR);

            _offer = _mockery.CreateMock<IApplication>();

            Expect.Call(_AR.GetApplicationByKey(0)).IgnoreArguments().Return(_offer).Repeat.Any();

            _account = _mockery.CreateMock<SAHL.Common.BusinessModel.Interfaces.IAccount>();

            SetupResult.For(_offer.Account).Return(_account);

            _lstPersons = _mockery.CreateMock<IReadOnlyEventList<ILegalEntity>>();

            Expect.Call(_account.GetNaturalPersonLegalEntitiesByRoleType(null,  new int[0])).IgnoreArguments().Return(_lstPersons);

            

            SetupResult.For(_lstPersons.Count).Return(1);



            // make sure we are calling the binds from initialise            
            BindOnInitialise();


            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            _view.OnAddAddressButtonClicked += null;
            IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnAddAddressButtonClicked", EventRaiser);

            _view.OnUpdateContactDetailsButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnUpdateContactDetailsButtonClicked", EventRaiser);

            _view.OnLegalEntityGridSelectedIndexChanged += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
         dict.Add("OnLegalEntityGridSelectedIndexChanged", EventRaiser);

            _view.OnNextButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnNextButtonClicked", EventRaiser);

            _view.OnUpdateAddressButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnUpdateAddressButtonClicked", EventRaiser);
            return dict;
        }

        /// <summary>
        /// IContact view Binding during initialise.
        /// </summary>
        private void BindOnInitialise()
        {
            ILegalEntity le = _mockery.CreateMock<ILegalEntity>();
            SetupResult.For(_lstPersons[0]).IgnoreArguments().Return(le);
            _view.BindLegalEntityGrid(_lstPersons, _account.Key);
            LastCall.IgnoreArguments();

            _view.BindAssuredLivesDetails(le);
            LastCall.IgnoreArguments();
        }

        #endregion
    }
}

