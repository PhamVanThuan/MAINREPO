using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Web.Views.Common.Interfaces;
using Rhino.Mocks.Interfaces;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using SAHL.Web.Views.Common.Presenters.LegalEntityDetails;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Presenters;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Test.Presenters.LegalEntityDetailsTest
{
    public class LegalEntityDetailsUpdateTestBase : TestViewBase
    {
        protected ILegalEntityDetails _view;
        protected ILegalEntityRepository _legalEntityRepository;
        protected IEventRaiser _viewInitialisedRaiser = null;
        protected IEventRaiser _viewPreRenderRaiser = null;
        protected IEventRaiser _cancelButtonClickRaiser;
        protected IEventRaiser _submitButtonClickRaiser;
        protected IEventRaiser _legalEntityTypeChangeRaiser;
        protected ILookupRepository _lookupRepository;

        // This doesn't work!
        protected ILookupRepository LookupRepository
        {
            set { _lookupRepository = value; }
            get { return _lookupRepository; }
        }

        [SetUp]
        protected void FixtureSetup()
        {
            _view = _mockery.CreateMock<ILegalEntityDetails>();
            base.SetupMockedView(_view);
            base.SetupPrincipalCache(base.TestPrincipal);

            // set our typefactory stratergy to be mock based, 
            // in order to force the TypeFactory to return mocks rather than
            // concrete classes.
            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);

            // Create a mocked ILegalEntityRepository object and add it to the MOCK Cache. Clear teh Cache just in case one exists already
            ClearMockCache();
            _legalEntityRepository = _mockery.CreateMock<ILegalEntityRepository>();
            base.MockCache.Add(typeof(ILegalEntityRepository).ToString(), _legalEntityRepository);

            _lookupRepository = _mockery.CreateMock<ILookupRepository>();
            base.MockCache.Add(typeof(ILookupRepository).ToString(), _lookupRepository);
        }

        [TearDown]
        protected void FixtureTearDown()
        {
            _view = null;
            _legalEntityRepository = null;
            ClearMockCache();
        }

        protected virtual void SetupBaseEventsExpectations()
        {
            // setup expectations
            _view.ViewInitialised += null;
            LastCall.Constraints(Is.NotNull());
            _viewInitialisedRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewLoaded += null;
            LastCall.Constraints(Is.NotNull());

            _view.ViewPreRender += null;
            LastCall.Constraints(Is.NotNull());
            _viewPreRenderRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.onCancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            _cancelButtonClickRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.onSubmitButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            _submitButtonClickRaiser = LastCall.IgnoreArguments().GetEventRaiser();

        }

        protected void BindLookupsExpectancies()
        {
            _view.BindLegalEntityTypes(null, null);
            LastCall.IgnoreArguments();

            _view.BindRoleTypes(null, null);
            LastCall.IgnoreArguments();

            _view.BindSalutation(null, null);
            LastCall.IgnoreArguments();

            _view.BindGender(null, null);
            LastCall.IgnoreArguments();

            _view.BindMaritalStatus(null, null);
            LastCall.IgnoreArguments();

            _view.BindPopulationGroup(null, null);
            LastCall.IgnoreArguments();

            _view.BindEducation(null, null);
            LastCall.IgnoreArguments();

            _view.BindCitizenType(null, null);
            LastCall.IgnoreArguments();

            _view.BindHomeLanguage(null, null);
            LastCall.IgnoreArguments();

            _view.BindDocumentLanguage(null, null);
            LastCall.IgnoreArguments();

            _view.BindLegalEntityStatus(null, null);
            LastCall.IgnoreArguments();

            _view.BindResidenceStatus(null, null);
            LastCall.IgnoreArguments();

        }

        protected void SetupBasicAddExpectancies()
        {
            _view.PanelAddVisible = true;
            _view.UpdateControlsVisible = true;
            // Additionally, on binding LegalEntityType, expect the default option to be NaturalPerson
            _view.PanelNaturalPersonAddVisible = true;

            _view.PanelCompanyAddVisible = false;
            _view.LockedUpdateControlsVisible = false;
            _view.NonContactDetailsDisabled = false;

        }

    }
}