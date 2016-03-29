using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Presenters.Employment;
using Rhino.Mocks;
using SAHL.Web.Controls;
using Rhino.Mocks.Constraints;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common;
using SAHL.Common.Security;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel;
using System.Security.Principal;
using SAHL.Web.Views;
using SAHL.Common.CacheData;
using SAHL.Web.Controls.Interfaces;

namespace SAHL.Web.Test.Presenters.Common.Employment
{
    [TestFixture]
    [Ignore]
    public class LegalEntityEmploymentUpdatePresenter : LegalEntityEmploymentBase
    {

        protected const string SaveButtonClicked = "SaveButtonClicked";

        protected const string SubsidyDetailsClicked = "SubsidyDetailsClicked";

        /// <summary>
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewInitialise()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            OnInitialiseCommon();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentUpdate presenter = new LegalEntityEmploymentUpdate(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Tests the OnViewLoaded method when the cache contains an employment record.
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewLoadEmploymentCached()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWLOAD];

            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IEmploymentDetails employmentDetails = _mockery.CreateMock<IEmploymentDetails>();

            // add an employment object to the global cache
            this.CurrentPrincipalCache.GetGlobalData().Add(ViewConstants.Employment, employment, new List<ICacheObjectLifeTime>());

            SetupResult.For(_view.IsPostBack).Return(false);
            SetupResult.For(_view.ShouldRunPage).Return(true);
            SetupResult.For(_view.EmploymentDetails).Return(employmentDetails);
            MockPopulateEmploymentDetails(employment, employmentDetails);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentUpdate presenter = new LegalEntityEmploymentUpdate(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Tests the OnViewLoaded method when the cache does not contain an employment record.
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewLoadEmptyCache()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWLOAD];

            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IEmployer employer = _mockery.CreateMock<IEmployer>();
            IEmploymentDetails employmentDetails = _mockery.CreateMock<IEmploymentDetails>();

            SetupResult.For(_view.ShouldRunPage).Return(true);
            SetupResult.For(_view.IsPostBack).Return(false);
            SetupResult.For(_view.SelectedEmployment).Return(employment);
            SetupResult.For(_view.EmploymentDetails).Return(employmentDetails);
            MockPopulateEmploymentDetails(employment, employmentDetails);
            SetupResult.For(employment.Employer).Return(employer);
            SetupResult.For(employer.Key).Return(0);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentUpdate presenter = new LegalEntityEmploymentUpdate(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderEmployerCachedNullEmployment()
        {
            OnViewPreRenderCommon(true, null);
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderEmployerCachedSelectedEmployment()
        {
            IEmployment employment = _mockery.CreateMock<IEmployment>();
            OnViewPreRenderCommon(true, employment);
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderEmployerNotCachedNullEmployment()
        {
            OnViewPreRenderCommon(false, null);
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderEmployerNotCachedSelectedEmployment()
        {
            IEmployment employment = _mockery.CreateMock<IEmployment>();
            OnViewPreRenderCommon(false, employment);
        }

        [NUnit.Framework.Test]
        public void SubsidyDetailsButtonClick()
        {
            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser btnClickEventRaiser = dictEvents[SubsidyDetailsClicked];

            _view.ShouldRunPage = false;
            SetupNavigationMock(_view, "LegalEntitySubsidyDetailsUpdate");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentUpdate presenter = new LegalEntityEmploymentUpdate(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            btnClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void SaveButtonClickInvalidEmployment()
        {
            OnSaveButtonCommon(false, true);
        }

        [NUnit.Framework.Test]
        public void SaveButtonClickValidEmploymentExtendedNotRequired()
        {
            OnSaveButtonCommon(false, false);
        }

        [NUnit.Framework.Test]
        public void SaveButtonClickValidEmploymentRequiresExtended()
        {
            OnSaveButtonCommon(false, true);
        }



        #region Helper Methods

        /// <summary>
        /// Common initialisation procedure.
        /// </summary>
        /// <returns></returns>
        protected override IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            SetupResult.For(_view.ShouldRunPage).Return(true);
            IDictionary<string, IEventRaiser> dict = base.OnInitialiseCommon();

            EmployerDetails employerDetails = new EmployerDetails();
            SetupResult.For(_view.EmployerDetails).Return(employerDetails);
            _view.SelectedEmployment = null;
            LastCall.IgnoreArguments();

            // save button click event
            _view.SaveButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erSaveButtonClicked = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(SaveButtonClicked, erSaveButtonClicked);

            // save button click event
            _view.SubsidyDetailsClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erSubsidyButtonClicked = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(SubsidyDetailsClicked, erSubsidyButtonClicked);

            return dict;
        }

        private void OnViewPreRenderCommon(bool employerKeyCached, IEmployment employment)
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];
            SetupResult.For(_view.ShouldRunPage).Return(true);

            IEmployerDetails employerDetails = _mockery.CreateMock<IEmployerDetails>();
            IEmploymentDetails employmentDetails = _mockery.CreateMock<IEmploymentDetails>();
            IEmployer employer = _mockery.CreateMock<IEmployer>();

            SetupResult.For(_view.SelectedEmployment).Return(employment);
            SetupResult.For(_view.EmployerDetails).Return(employerDetails);
            SetupResult.For(_view.EmploymentDetails).Return(employmentDetails);
            if (employment != null)
                SetupResult.For(employment.Employer).Return(employer);

            if (employerKeyCached)
            {
                CurrentPrincipalCache.GetGlobalData().Add(ViewConstants.EmployerKey, 1, new List<ICacheObjectLifeTime>());
                Expect.Call(EmploymentRepository.GetEmployerByKey(0)).IgnoreArguments().Return(employer);
                employerDetails.Employer = employer;
            }
            else
            {
                if (employment != null)
                    employerDetails.Employer = employer;
            }

            if (employment != null)
            {
                employmentDetails.ConfirmedBasicIncomeReadOnly = false;
                employmentDetails.EmploymentStatusReadOnly = false;
                employmentDetails.EndDateReadOnly = false;
                employmentDetails.ContactPersonReadOnly = false;
                employmentDetails.ContactPhoneNumberReadOnly = false;
                employmentDetails.DepartmentReadOnly = false;
                _view.CancelButtonVisible = true;
                _view.SaveButtonVisible = true;

                // refresh some of the readonly fields that go missing on postback
                Expect.Call(employment.RemunerationType).Return(null);
                employmentDetails.RemunerationType = null;
                LastCall.IgnoreArguments();
                Expect.Call(employment.EmploymentStartDate).Return(null);
                employmentDetails.StartDate = null;
                LastCall.IgnoreArguments();
                Expect.Call(employment.BasicIncome).Return(null);
                employmentDetails.BasicIncome = null;
                LastCall.IgnoreArguments();
                Expect.Call(employment.ConfirmedBy).Return(null);
                employmentDetails.ConfirmedBy = null;
                LastCall.IgnoreArguments();

                Expect.Call(employment.RequiresExtended).Return(true);
                _view.SaveButtonText = "Test";
                LastCall.IgnoreArguments();
            }
            else
            {
                employerDetails.Visible = false;
                employmentDetails.Visible = false;
            }

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentUpdate presenter = new LegalEntityEmploymentUpdate(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();

        }

        private void OnSaveButtonCommon(bool validEmployment, bool requiresExtended)
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];
            
            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser saveClickEventRaiser = dictEvents[SaveButtonClicked];

            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IDomainMessageCollection dmc = _mockery.CreateMock<IDomainMessageCollection>();

            // add items to the cache
            CurrentPrincipalCache.GetGlobalData().Add(ViewConstants.Employment, employment, new List<ICacheObjectLifeTime>());
            CurrentPrincipalCache.GetGlobalData().Add(ViewConstants.EmployerKey, 1, new List<ICacheObjectLifeTime>());


            SetupResult.For(_view.SelectedEmployment).Return(employment);
            SetupResult.For(_view.GetCapturedEmployment()).Return(employment);
            employment.LegalEntity = LegalEntity;

            List<string> rules = _mockery.CreateMock<List<string>>();
            SetupResult.For(employment.RequiresExtended).Return(requiresExtended);
            SetupResult.For(employment.ValidateEntity()).Return(validEmployment);
            // SetupResult.For(employment.ExcludedRules).Return(rules);

            if (validEmployment)
            {
                if (requiresExtended)
                {
                    SetupNavigationMock(_view, "EmploymentExtended");
                }
                else
                {
                    EmploymentRepository.SaveEmployment(employment);
                    SetupNavigationMock(_view, "LegalEntityEmploymentDisplay");
                }
            }

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentUpdate presenter = new LegalEntityEmploymentUpdate(_view, controller);

            initEventRaiser.Raise(_view, new EventArgs());
            saveClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        #endregion

    }
}
