using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Test;
using NUnit.Framework;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Rhino.Mocks.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Web.Views.Common.Presenters;
using SAHL.Common.Web.UI;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using System.Collections.Generic;
using SAHL.Common.Collections.Interfaces;
using System.Reflection;
using SAHL.Common.Web.UI.Events;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Web.Test.Presenters.Common
{
    [Ignore]
    [TestFixture]
    public class ApplicantsRemovePresenter : TestViewBase
    {

        private IApplicants _view;
        private ILegalEntityRepository _legalEntityRepository;
        private IApplicationRepository _applicationRepository;
        private IResourceService _resourceService;
        private IEventRaiser _viewInitialisedRaiser;
        private IEventRaiser _viewPreRenderRaiser;
        private IEventRaiser _viewGridLegalEntitySelected;
        private IEventRaiser _viewCancelButtonClicked;
        private IEventRaiser _viewRemoveButtonClicked;

        private ICBOService _cboService;

        // private IEventRaiser _onSubmitButtonClickRaiser;
        // private IEventRaiser _onCancelButtonClickRaiser;

        // private ILookupRepository _lookupRepository;


        [SetUp]
        protected void FixtureSetup()
        {
            _view = _mockery.CreateMock<IApplicants>();

            base.SetupMockedView(_view);
            base.SetupPrincipalCache(base.TestPrincipal);

            // set our typefactory stratergy to be mock based, 
            // in order to force the TypeFactory to return mocks rather than
            // concrete classes.
            base.SetRepositoryStrategy(TypeFactoryStrategy.Mock);

            // Create a mocked ILegalEntityRepository object and add it to the MOCK Cache. Clear teh Cache just in case one exists already
            ClearMockCache();

            _cboService = _mockery.CreateMock<ICBOService>();
            base.MockCache.Add(typeof(ICBOService).ToString(), _cboService);

            _applicationRepository = _mockery.CreateMock<IApplicationRepository>();
            base.MockCache.Add(typeof(IApplicationRepository).ToString(), _applicationRepository);

            _legalEntityRepository = _mockery.CreateMock<ILegalEntityRepository>();
            base.MockCache.Add(typeof(ILegalEntityRepository).ToString(), _legalEntityRepository);

            /*
            _resourceService = _mockery.CreateMock<IResourceService>();
            base.MockCache.Add(typeof(IResourceService).ToString(), _resourceService);

            _lookupRepository = _mockery.CreateMock<ILookupRepository>();
            base.MockCache.Add(typeof(ILookupRepository).ToString(), _lookupRepository);
            */
        }

        [TearDown]
        protected void FixtureTearDown()
        {
            _view = null;
            _legalEntityRepository = null;
            _cboService = null;
            ClearMockCache();
        }


        #region Unit Tests
        [NUnit.Framework.Test]
        public void EventsHookup()
        {
            SetupBaseEventsExpectations();
            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            ApplicantsRemove ApplicantsRemove = new ApplicantsRemove(_view, controller);
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewInitializeNoException()
        {
            SetupBaseEventsExpectations();

            // Expect it to get the selected node from the CBO
            CBOMenuNode cboNode = _mockery.CreateMock<CBOMenuNode>(new Dictionary<string, object>());
            Expect.Call(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(cboNode);

            // Expect the application to be obtained from the app repository
            IApplication application = _mockery.CreateMock<IApplication>();
            SetupResult.For(_view.Messages).Return(new DomainMessageCollection());
            SetupResult.For(cboNode.Description).Return(String.Empty);
            Expect.Call(cboNode.GenericKey).Return((Int64)1);
            Expect.Call(_applicationRepository.GetApplicationByKey(1)).IgnoreArguments().Return(application);

            // Expect the bind methods to be called
            IAccount account = _mockery.CreateMock<IAccount>();
            IEventList<IRole> roles = _mockery.CreateMock<IEventList<IRole>>();
            Expect.Call(application.Account).Return(account);
            Expect.Call(account.Roles).Return(roles);
            _view.BindApplicantsGrid(null);
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            ApplicantsRemove ApplicantsRemove = new ApplicantsRemove(_view, controller);
            _viewInitialisedRaiser.Raise(null, null);

            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        [NUnit.Framework.ExpectedException(typeof(ArgumentNullException))]
        public void ViewInitializeException()
        {
            SetupBaseEventsExpectations();

            // Expect it to get the selected node from the CBO
            Expect.Call(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(null);

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            ApplicantsRemove ApplicantsRemove = new ApplicantsRemove(_view, controller);

            try
            {
                _viewInitialisedRaiser.Raise(null, null);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }

            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewGridLegalEntitySelected()
        {
            SetupBaseEventsExpectations();

            // Expect it to get the selected node from the CBO
            CBOMenuNode cboNode = _mockery.CreateMock<CBOMenuNode>(new Dictionary<string, object>());
            Expect.Call(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(cboNode);

            // Expect the application to be obtained from the app repository
            IApplication application = _mockery.CreateMock<IApplication>();
            SetupResult.For(_view.Messages).Return(new DomainMessageCollection());
            SetupResult.For(cboNode.Description).Return(String.Empty);
            Expect.Call(cboNode.GenericKey).Return((Int64)1);
            Expect.Call(_applicationRepository.GetApplicationByKey(1)).IgnoreArguments().Return(application);

            // Expect the bind methods to be called
            IAccount account = _mockery.CreateMock<IAccount>();
            IEventList<IRole> roles = _mockery.CreateMock<IEventList<IRole>>();
            IRole role = _mockery.CreateMock<IRole>();
            ILegalEntity legalEntity = _mockery.CreateMock<ILegalEntity>();

            Expect.Call(application.Account).Return(account);
            Expect.Call(account.Roles).Return(roles);
            Expect.Call(roles[1]).IgnoreArguments().Return(role);
            Expect.Call(role.LegalEntity).Return(legalEntity);

            _view.BindLegalEntityDetails(null);
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            ApplicantsRemove ApplicantsRemove = new ApplicantsRemove(_view, controller);

            _viewGridLegalEntitySelected.Raise(null, new KeyChangedEventArgs(1));

            _mockery.VerifyAll();

        }


        [NUnit.Framework.Test]
        public void ViewPreRenderNoLEsFound()
        {
            SetupBaseEventsExpectations();

            IApplication application = _mockery.CreateMock<IApplication>();
            IAccount account = _mockery.CreateMock<IAccount>();
            IEventList<IRole> roles = _mockery.CreateMock<IEventList<IRole>>();

            SetupResult.For(application.Account).Return(account);
            SetupResult.For(account.Roles).Return(roles);
            SetupResult.For(roles.Count).Return(0);

            _view.ApplicantDetailsVisible = false;
            _view.ButtonsEnabled = false;
            _view.ApplicantDetailsCompanyVisible = false;
            _view.ApplicantDetailsNaturalPersonVisible = false;
            _view.ButtonsVisible = true;

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            ApplicantsRemove ApplicantsRemove = new ApplicantsRemove(_view, controller);
            ApplicantsRemove.ApplicationMocked = application;

            _viewPreRenderRaiser.Raise(null, null);

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void ViewPreRenderLEsFoundNaturalSelected()
        {
            SetupBaseEventsExpectations();

            IApplication application = _mockery.CreateMock<IApplication>();
            IAccount account = _mockery.CreateMock<IAccount>();
            IEventList<IRole> roles = _mockery.CreateMock<IEventList<IRole>>();
            ILegalEntityNaturalPerson legalEntity = _mockery.CreateMock<ILegalEntityNaturalPerson>();

            SetupResult.For(application.Account).Return(account);
            SetupResult.For(account.Roles).Return(roles);
            SetupResult.For(roles.Count).Return(1);

            _view.ApplicantDetailsVisible = true;
            _view.ButtonsEnabled = true;
            _view.ApplicantDetailsNaturalPersonVisible = true;
            _view.ApplicantDetailsCompanyVisible = false;
            _view.ButtonsVisible = true;

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            ApplicantsRemove ApplicantsRemove = new ApplicantsRemove(_view, controller);

            ApplicantsRemove.ApplicationMocked = application;
            ApplicantsRemove.LegalEntityMocked = legalEntity;

            _viewPreRenderRaiser.Raise(null, null);

            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderLEsFoundNonNaturalSelected()
        {
            SetupBaseEventsExpectations();

            IApplication application = _mockery.CreateMock<IApplication>();
            IAccount account = _mockery.CreateMock<IAccount>();
            IEventList<IRole> roles = _mockery.CreateMock<IEventList<IRole>>();
            ILegalEntityCompany legalEntity = _mockery.CreateMock<ILegalEntityCompany>();

            SetupResult.For(application.Account).Return(account);
            SetupResult.For(account.Roles).Return(roles);
            SetupResult.For(roles.Count).Return(1);

            _view.ApplicantDetailsVisible = true;
            _view.ButtonsEnabled = true;
            _view.ApplicantDetailsNaturalPersonVisible = false;
            _view.ApplicantDetailsCompanyVisible = true;
            _view.ButtonsVisible = true;

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            ApplicantsRemove ApplicantsRemove = new ApplicantsRemove(_view, controller);

            ApplicantsRemove.ApplicationMocked = application;
            ApplicantsRemove.LegalEntityMocked = legalEntity;

            _viewPreRenderRaiser.Raise(null, null);

            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void ViewCancelButtonClicked()
        {
            SetupBaseEventsExpectations();

            // Expect the view to Navigate to Cancel
            ISimpleNavigator navigator = _mockery.CreateMock<ISimpleNavigator>();
            Expect.Call(_view.Navigator).Return(navigator);
            navigator.Navigate("Cancel");

            _mockery.ReplayAll();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            ApplicantsRemove ApplicantsRemove = new ApplicantsRemove(_view, controller);

            _viewCancelButtonClicked.Raise(null, null);

            _mockery.VerifyAll();

        }





        #endregion


        #region Helper Functions
        protected virtual void SetupBaseEventsExpectations()
        {
            // setup expectations
            _view.ViewInitialised += null;
            LastCall.Constraints(Is.NotNull());
            _viewInitialisedRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewPreRender += null;
            LastCall.Constraints(Is.NotNull());
            _viewPreRenderRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.OnGridLegalEntitySelected += null;
            LastCall.Constraints(Is.NotNull());
            _viewGridLegalEntitySelected = LastCall.IgnoreArguments().GetEventRaiser();

            _view.OnCancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            _viewCancelButtonClicked = LastCall.IgnoreArguments().GetEventRaiser();

            _view.OnRemoveButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            _viewRemoveButtonClicked = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewLoaded += null;
            LastCall.Constraints(Is.NotNull());

        }
        #endregion

    }
}
