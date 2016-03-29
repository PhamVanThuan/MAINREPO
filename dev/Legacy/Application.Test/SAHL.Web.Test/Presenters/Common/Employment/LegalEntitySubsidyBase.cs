using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Service.Interfaces;
using Rhino.Mocks.Interfaces;
using Rhino.Mocks.Constraints;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common;
using SAHL.Common.Security;
using System.Security.Principal;
using SAHL.Web.Controls.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Presenters.Employment;
using SAHL.Common.CacheData;
using SAHL.Web.Views;
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Web.Test.Presenters.Common.Employment
{
    public class LegalEntitySubsidyBasePresenter : TestViewBase
    {

        protected ISubsidyDetails _view;
        private ICBOService _cboService;
        private ILegalEntityRepository _legalEntityRepository;
        private IEmploymentRepository _employmentRepository;
        private ILegalEntity _legalEntity;
        private IEmployment _cachedEmployment;

        protected const string BackButtonClicked = "BackButtonClicked";

        protected const string CancelButtonClicked = "CancelButtonClicked";

        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
            _view = _mockery.CreateMock<ISubsidyDetails>();
            SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            ClearMockCache();

            _legalEntity = _mockery.CreateMock<ILegalEntity>();


            // create repositories
            _employmentRepository = _mockery.CreateMock<IEmploymentRepository>();
            MockCache.Add(typeof(IEmploymentRepository).ToString(), _employmentRepository);
            _legalEntityRepository = _mockery.CreateMock<ILegalEntityRepository>();
            MockCache.Add(typeof(ILegalEntityRepository).ToString(), _legalEntityRepository);
            ////_lookupRepository = _mockery.CreateMock<ILookupRepository>();
            ////MockCache.Add(typeof(ILookupRepository).ToString(), _lookupRepository);
            _cboService = _mockery.CreateMock<ICBOService>();
            MockCache.Add(typeof(ICBOService).ToString(), _cboService);

            // add an employment object to the cache - we'll aways need it
            _cachedEmployment = _mockery.CreateMock<IEmployment>();
            this.CurrentPrincipalCache.GetGlobalData().Add(ViewConstants.Employment, _cachedEmployment, new List<ICacheObjectLifeTime>());


        }

        [TearDown]
        public void TearDown()
        {
            ClearMockCache();

            // clear the global cache
            this.CurrentPrincipalCache.GetGlobalData().Clear();

            _mockery = null;
            _cboService = null;
        }

        #region Properties

        protected IEmployment CachedEmployment
        {
            get
            {
                return _cachedEmployment;
            }
        }

        protected ILegalEntity LegalEntity
        {
            get
            {
                return _legalEntity;
            }
        }

        protected ILegalEntityRepository LegalEntityRepository
        {
            get
            {
                return _legalEntityRepository;
            }
        }

        protected IEmploymentRepository EmploymentRepository
        {
            get
            {
                return _employmentRepository;
            }
        }

        
        #endregion

        #region Helper Methods

        protected virtual IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            IEventList<IEmployment> leEmployment = _mockery.CreateMock<IEventList<IEmployment>>();

            // simulate getting the CBO node
            CBOMenuNode cboCurrentMenuNode = _mockery.CreateMock<CBOMenuNode>(new Dictionary<string, object>());
            SetupResult.For(cboCurrentMenuNode.GenericKey).Return(0);
            SetupResult.For(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(cboCurrentMenuNode);

            // simulate mocked objects
            SetupResult.For(LegalEntityRepository.GetLegalEntityByKey(0)).IgnoreArguments().Return(_legalEntity);
            SetupResult.For(_legalEntity.Employment).Return(leEmployment);

            // existing address selected event
            _view.BackButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erBackButtonClicked = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(BackButtonClicked, erBackButtonClicked);

            // existing address selected event
            _view.CancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erCancelButtonClicked = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(CancelButtonClicked, erCancelButtonClicked);

            // always make IsMenuPostBack false - we want to test everything
            SetupResult.For(_view.IsMenuPostBack).Return(false);
            SetupResult.For(_view.ShouldRunPage).Return(true);

            return dict;
        }


        #endregion

    }
}
