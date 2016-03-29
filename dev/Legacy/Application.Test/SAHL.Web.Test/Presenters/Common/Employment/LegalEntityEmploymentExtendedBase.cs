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
using SAHL.Common.BusinessModel.Interfaces.UI;

namespace SAHL.Web.Test.Presenters.Common.Employment
{
    public class LegalEntityEmploymentExtendedBase : TestViewBase
    {

        protected IEmploymentExtended _view;
        private ICBOService _cboService;
        //private ILegalEntityRepository _legalEntityRepository;
        private IEmploymentRepository _employmentRepository;
        //private ILegalEntity _legalEntity;

        //protected const string EmploymentSelected = "EmploymentSelected";

        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
            _view = _mockery.CreateMock<IEmploymentExtended>();
            SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);
            SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            ClearMockCache();

            //_legalEntity = _mockery.CreateMock<ILegalEntity>();


            // create repositories
            _employmentRepository = _mockery.CreateMock<IEmploymentRepository>();
            MockCache.Add(typeof(IEmploymentRepository).ToString(), _employmentRepository);
            //_legalEntityRepository = _mockery.CreateMock<ILegalEntityRepository>();
            //MockCache.Add(typeof(ILegalEntityRepository).ToString(), _legalEntityRepository);
            ////_lookupRepository = _mockery.CreateMock<ILookupRepository>();
            ////MockCache.Add(typeof(ILookupRepository).ToString(), _lookupRepository);
            _cboService = _mockery.CreateMock<ICBOService>();
            MockCache.Add(typeof(ICBOService).ToString(), _cboService);

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

        //protected ILegalEntity LegalEntity
        //{
        //    get
        //    {
        //        return _legalEntity;
        //    }
        //}

        //protected ILegalEntityRepository LegalEntityRepository
        //{
        //    get
        //    {
        //        return _legalEntityRepository;
        //    }
        //}

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

            // simulate getting the CBO node
            CBOMenuNode cboCurrentMenuNode = _mockery.CreateMock<CBOMenuNode>(new Dictionary<string, object>());
            SetupResult.For(cboCurrentMenuNode.GenericKey).Return(0);
            SetupResult.For(_cboService.GetCurrentCBONode(null, null)).IgnoreArguments().Return(cboCurrentMenuNode);

            return dict;
        }
        #endregion

    }
}
