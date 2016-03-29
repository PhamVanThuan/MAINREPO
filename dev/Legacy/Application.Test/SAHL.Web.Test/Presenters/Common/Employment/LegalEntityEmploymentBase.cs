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
    public class LegalEntityEmploymentBase : TestViewBase
    {

        protected IEmploymentView _view;
        private ICBOService _cboService;
        private ILegalEntityRepository _legalEntityRepository;
        private IEmploymentRepository _employmentRepository;
        private ILegalEntity _legalEntity;

        protected const string EmploymentSelected = "EmploymentSelected";

        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
            _view = _mockery.CreateMock<IEmploymentView>();
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
            //_lookupRepository = _mockery.CreateMock<ILookupRepository>();
            //MockCache.Add(typeof(ILookupRepository).ToString(), _lookupRepository);
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
            _view.EmploymentSelected += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erEmploymentSelected = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(EmploymentSelected, erEmploymentSelected);

            // always make IsMenuPostBack false - we want to test everything
            SetupResult.For(_view.IsMenuPostBack).Return(false);

            _view.BindEmploymentDetails(leEmployment, true);

            return dict;
        }

        /// <summary>
        /// Helper method to populate an IEmploymentDetails object from an IEmployment object
        /// </summary>
        /// <param name="employment"></param>
        /// <param name="employmentDetails"></param>
        protected void MockPopulateEmploymentDetails(IEmployment employment, IEmploymentDetails employmentDetails)
        {
            Expect.Call(employment.EmploymentType).Return(null);
            employmentDetails.EmploymentType = null;
            LastCall.IgnoreArguments();
            Expect.Call(employment.EmploymentStatus).Return(null);
            employmentDetails.EmploymentStatus = null;
            LastCall.IgnoreArguments();
            Expect.Call(employment.RemunerationType).Return(null);
            employmentDetails.RemunerationType = null;
            LastCall.IgnoreArguments();
            Expect.Call(employment.EmploymentStartDate).Return(null);
            employmentDetails.StartDate = null;
            LastCall.IgnoreArguments();
            Expect.Call(employment.EmploymentEndDate).Return(null);
            employmentDetails.EndDate = null;
            LastCall.IgnoreArguments();
            Expect.Call(employment.BasicIncome).Return(null);
            employmentDetails.BasicIncome = null;
            LastCall.IgnoreArguments();
            Expect.Call(employment.ConfirmedBasicIncome).Return(null);
            employmentDetails.ConfirmedBasicIncome = null;
            LastCall.IgnoreArguments();
            Expect.Call(employment.ContactPerson).Return(null);
            employmentDetails.ContactPerson = null;
            LastCall.IgnoreArguments();
            Expect.Call(employment.ContactPhoneCode).Return(null);
            employmentDetails.ContactPhoneCode = null;
            LastCall.IgnoreArguments();
            Expect.Call(employment.ContactPhoneNumber).Return(null);
            employmentDetails.ContactPhoneNumber = null;
            LastCall.IgnoreArguments();
            Expect.Call(employment.Department).Return(null);
            employmentDetails.Department = null;
            LastCall.IgnoreArguments();
            Expect.Call(employment.ConfirmedBy).Return(null);
            employmentDetails.ConfirmedBy = null;
            LastCall.IgnoreArguments();
        }


        #endregion

    }
}
