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
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Test.Presenters.LegalEntityDetailsTest
{
    public class LegalEntityDetailsAddPresenterTestBase : TestViewBase
    {
        private ILookupRepository _lookupRepository;
        protected ILegalEntityDetails _view;
        protected IEventRaiser _viewInitialisedRaiser = null;
        protected ILegalEntityRepository _legalEntityRepository;

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

            _lookupRepository = _mockery.CreateMock<ILookupRepository>();
            base.MockCache.Add(typeof(ILookupRepository).ToString(), _lookupRepository);
            
            _legalEntityRepository = _mockery.CreateMock<ILegalEntityRepository>();
            base.MockCache.Add(typeof(ILegalEntityRepository).ToString(), _legalEntityRepository);
        }

        [TearDown]
        protected void FixtureTearDown()
        {
            _view = null;
            _legalEntityRepository = null;
            ClearMockCache();
        }

        protected void SetupBaseEventsExpectations()
        {
            // setup expectations
            _view.ViewInitialised += null;
            LastCall.Constraints(Is.NotNull());
            _viewInitialisedRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewLoaded += null;
            LastCall.Constraints(Is.NotNull());

            _view.ViewPreRender += null;
            LastCall.Constraints(Is.NotNull());
        }
    }
}
