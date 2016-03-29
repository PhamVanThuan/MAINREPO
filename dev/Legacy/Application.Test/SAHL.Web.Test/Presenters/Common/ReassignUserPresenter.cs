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
using SAHL.Web.Views.Common.Presenters;
using SAHL.Common.BusinessModel;

namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class ReassignUserPresenter : TestViewBase
    {
        private IReassignUser _view;
        private CacheManager _CM;
        private IDomainMessageCollection _messages;
        private const string OnSubmitButtonClicked = "OnSubmitButtonClicked";
        private const string OnCancelButtonClicked = "OnCancelButtonClicked";
        private IOrganisationStructureRepository _OSR;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<IReassignUser>();
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
            ReassignUser presenter = new ReassignUser(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [Test]
        public void PreRenderTest()
        {
            // Hook up life cycle events and return the view initialized event raiser

            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];

            //Does nothing for now

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            ReassignUser presenter = new ReassignUser(_view, controller);            
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

            INavigator nav = _mockery.CreateMock<INavigator>();

            Expect.Call(_view.Navigator).Return(nav).Repeat.Any();

            nav.Navigate("CallSummary");


            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            ReassignUser presenter = new ReassignUser(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            cancelClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [Test]
        public void SubmitButtonClickedTest()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];
            throw new Exception("DB structure changed");
            //IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            //IEventRaiser submitClickEventRaiser = dictEvents[OnSubmitButtonClicked];
           
            //IUserGroupAssignment uga = _mockery.CreateMock<IUserGroupAssignment>();
            //SetupResult.For(_OSR.GetEmptyUserGroupAssignment(_messages)).IgnoreArguments().Return(uga);
            //SetupResult.For(_view.SelectedConsultantKey).Return(1);
            //IADUser adUser = _mockery.CreateMock<IADUser>();
            //Expect.Call(_OSR.GetADUserByKey( 1)).IgnoreArguments().Return(adUser);
            //Expect.Call(_OSR.CreateUserGroupAssignment( uga)).IgnoreArguments().Return(uga);

            //_OSR.SaveIUserGroupAssignment( null);
            //LastCall.IgnoreArguments();

            //INavigator nav = _mockery.CreateMock<INavigator>();

            //Expect.Call(_view.Navigator).Return(nav).Repeat.Any();

            //nav.Navigate("RouteToConsultant");

            //SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            //_mockery.ReplayAll();
            //RouteToConsultant presenter = new RouteToConsultant(_view, controller);            
            //initEventRaiser.Raise(_view, new EventArgs());
            //submitClickEventRaiser.Raise(_view, new EventArgs());
            //_mockery.VerifyAll();
        }
        
        #region Private Helpers

        /// <summary>
        /// Hooks the IContact view events.
        /// </summary>
        /// <returns>A Dictionary collection of IEventRaiser objects, one for each IContact view event.</returns>
        private IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            throw new Exception("DB structure changed");
            //SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            //ClearMockCache();

            //_CM = CacheFactory.GetCacheManager("MOCK");

            //_messages = _mockery.CreateMock<IDomainMessageCollection>();

            //_OSR = _mockery.CreateMock<IOrganisationStructureRepository>();
            //_CM.Add(typeof(IOrganisationStructureRepository).ToString(), _OSR);

            //IUserGroupMapping ugm = _mockery.CreateMock<IUserGroupMapping>();
            //SetupResult.For(_OSR.GetOrganisationStructureKeyForFunctionalGroupName( "FurtherLendingProcessor")).IgnoreArguments().Return(ugm);

            //IOrganisationStructure os = _mockery.CreateMock<IOrganisationStructure>();
            //SetupResult.For(ugm.OrganisationStructure).IgnoreArguments().Return(os);
            //SetupResult.For(os.Key).Return(1);
            //IEventList<IADUser> users = _mockery.CreateMock<IEventList<IADUser>>();
            //SetupResult.For(_OSR.GetUsersForOrganisationStructureKey( 1)).IgnoreArguments().Return(users);

            //// make sure we are calling the binds from initialise            
            //BindOnInitialise();

            //Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            //_view.OnCancelButtonClicked += null;
            //IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            //dict.Add("OnCancelButtonClicked", EventRaiser);

            //_view.OnSubmitButtonClicked += null;
            //EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            //dict.Add("OnSubmitButtonClicked", EventRaiser);

            //return dict;
        }

        /// <summary>
        /// IContact view Binding during initialise.
        /// </summary>
        private void BindOnInitialise()
        {
            _view.BindConsultantList(null);
            LastCall.IgnoreArguments();           
        }

        #endregion

    }
}

