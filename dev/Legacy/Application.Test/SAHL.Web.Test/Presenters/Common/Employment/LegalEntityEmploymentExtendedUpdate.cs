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
using SAHL.Web.Controls.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views;
using SAHL.Common.CacheData;
using SAHL.Common.Globals;

namespace SAHL.Web.Test.Presenters.Common.Employment
{
    [TestFixture]
    [Ignore]
    public class LegalEntityEmploymentExtendedUpdatePresenter : LegalEntityEmploymentExtendedBase
    {

        protected const string CancelClicked = "CancelClicked";

        protected const string BackClicked = "BackClicked";

        protected const string SaveClicked = "SaveClicked";

        private IEmployment _employment;

        [SetUp]
        protected void Setup()
        {
            base.Setup();
            // add an employment object to the cache - we'll aways need it
            _employment = _mockery.CreateMock<IEmployment>();
            this.CurrentPrincipalCache.GetGlobalData().Add(ViewConstants.Employment, _employment, new List<ICacheObjectLifeTime>());

        }

        /// <summary>
        /// Ensures that the following methods are called on initialisation.
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewInitialise()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            OnInitialiseCommon();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentExtendedUpdate presenter = new LegalEntityEmploymentExtendedUpdate(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [Test]
        public void ViewPreRender()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];
            SetupResult.For(_view.ShouldRunPage).Return(true);

            _view.SetEmployment(_employment);
            LastCall.IgnoreArguments();
            SetupResult.For(_employment.Key).Return(1);
            _view.SaveButtonText = "Update";

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentExtendedUpdate presenter = new LegalEntityEmploymentExtendedUpdate(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();

        }

        [NUnit.Framework.Test]
        public void CancelButtonClick()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser btnClickEventRaiser = dictEvents[CancelClicked];

            _view.ShouldRunPage = false;
            SetupNavigationMock(_view, "Cancel");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentExtendedUpdate presenter = new LegalEntityEmploymentExtendedUpdate(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            btnClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void BackButtonClick()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser btnClickEventRaiser = dictEvents[BackClicked];

            _view.GetExtendedDetails(_employment);
            _view.ShouldRunPage = false;
            SetupNavigationMock(_view, "Back");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentExtendedUpdate presenter = new LegalEntityEmploymentExtendedUpdate(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            btnClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void SaveButtonClickAddInvalid()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser btnClickEventRaiser = dictEvents[SaveClicked];

            OnSaveCommon(true, false, false);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentExtendedUpdate presenter = new LegalEntityEmploymentExtendedUpdate(_view, controller);

            _view.Messages.Add(new Error("test", "test"));

            initEventRaiser.Raise(_view, new EventArgs());
            btnClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void SaveButtonClickAddValidSubsidised()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser btnClickEventRaiser = dictEvents[SaveClicked];

            OnSaveCommon(true, true, false);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentExtendedUpdate presenter = new LegalEntityEmploymentExtendedUpdate(_view, controller);

            initEventRaiser.Raise(_view, new EventArgs());
            btnClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void SaveButtonClickUpdateValid()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser btnClickEventRaiser = dictEvents[SaveClicked];

            OnSaveCommon(false, true, false);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentExtendedUpdate presenter = new LegalEntityEmploymentExtendedUpdate(_view, controller);

            initEventRaiser.Raise(_view, new EventArgs());
            btnClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
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

            // set up event handlers
            _view.BackButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erBackClicked = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(BackClicked, erBackClicked);

            _view.CancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erCancelClicked = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(CancelClicked, erCancelClicked);

            _view.SaveButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erSaveClicked = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(SaveClicked, erSaveClicked);


            // other properties
            double? basicIncome = new double?(100);
            Expect.Call(_employment.IsConfirmed).Return(true);
            Expect.Call(_employment.BasicIncome).Return(basicIncome).Repeat.Any();
            _view.MonthlyIncomeReadOnly = true;
            _view.ConfirmedIncomeReadOnly = false;

            return dict;
        }

        private void OnSaveCommon(bool newEmployment, bool valid, bool subsidised)
        {
            IEmployment source = _employment;
            IEmployment target = _mockery.CreateMock<IEmployment>();

            if (newEmployment)
            {
                SetupResult.For(source.Key).Return(0);
                IEmploymentType employmentType = _mockery.CreateMock<IEmploymentType>();
                Expect.Call(source.EmploymentType).Return(employmentType);
                Expect.Call(EmploymentRepository.GetEmptyEmploymentByType(null)).IgnoreArguments().Return(target);
            }
            else
            {
                SetupResult.For(source.Key).Return(1);
                Expect.Call(EmploymentRepository.GetEmploymentByKey(0)).IgnoreArguments().Return(target);

            }

            SetupResult.For(target.ConfirmedBasicIncome).Return(new double?(100));
            // simulate the copying of the cached values
            MockCopyCachedValues(source, target);

            _view.GetExtendedDetails(target);

            Expect.Call(target.ValidateEntity()).Return(valid);

            if (valid)
            {
                IEmploymentType employmentType = _mockery.CreateMock<IEmploymentType>();
                Expect.Call(target.EmploymentType).Return(employmentType);
                if (subsidised)
                {
                    Expect.Call(employmentType.Key).Return((int)EmploymentTypes.Subsidised);
                    SetupNavigationMock(_view, "SubsidyDetails");
                }
                else
                {
                    Expect.Call(employmentType.Key).Return((int)EmploymentTypes.Salaried);
                    List<string> rules = _mockery.CreateMock<List<string>>();
                    Expect.Call(target.RequiresExtended).Return(false).Repeat.Any();
                    EmploymentRepository.SaveEmployment(target);
                    _view.ShouldRunPage = false;
                    SetupNavigationMock(_view, "LegalEntityEmploymentDisplay");
                }

            }


        }

        private void MockCopyCachedValues(IEmployment source, IEmployment target)
        {

            Expect.Call(source.EmploymentStatus).Return(null);
            target.EmploymentStatus = null;
            LastCall.IgnoreArguments();

            Expect.Call(source.EmploymentStartDate).Return(null);
            target.EmploymentStartDate = null;
            LastCall.IgnoreArguments();

            Expect.Call(source.EmploymentEndDate).Return(null);
            target.EmploymentEndDate = null;
            LastCall.IgnoreArguments();

            // Expect.Call(source.BasicIncome).Return(null); -> expectancy already exists
            target.BasicIncome = null;
            LastCall.IgnoreArguments();

            Expect.Call(source.ConfirmedBasicIncome).Return(null);
            target.ConfirmedBasicIncome = null;
            LastCall.IgnoreArguments();

            Expect.Call(source.ConfirmedBy).Return(null);
            target.ConfirmedBy = null;
            LastCall.IgnoreArguments();

            Expect.Call(source.ContactPerson).Return(null);
            target.ContactPerson = null;
            LastCall.IgnoreArguments();

            Expect.Call(source.ContactPhoneCode).Return(null);
            target.ContactPhoneCode = null;
            LastCall.IgnoreArguments();

            Expect.Call(source.ContactPhoneNumber).Return(null);
            target.ContactPhoneNumber = null;
            LastCall.IgnoreArguments();

            Expect.Call(source.Department).Return(null);
            target.Department = null;
            LastCall.IgnoreArguments();

            IRemunerationType remunType = _mockery.CreateMock<IRemunerationType>();
            Expect.Call(source.RemunerationType).Return(remunType);
            Expect.Call(remunType.Key).Return(0);
            LastCall.IgnoreArguments();

            target.LegalEntity = null;
            LastCall.IgnoreArguments();

            Expect.Call(source.RequiresExtended).Return(false);
            /*

            // legal entity
            target.LegalEntity = GetLegalEntity(principal);

            // employer
            if (CachedEmployerKey > 0)
                target.Employer = EmploymentRepository.GetEmployerByKey(CachedEmployerKey);

            // extended information
            if (source != null && source.RequiresExtended)
            {
                target.ExtendedEmployment.Allowances = source.ExtendedEmployment.Allowances;
                target.ExtendedEmployment.BasicIncome = source.ExtendedEmployment.BasicIncome;
                target.ExtendedEmployment.Commission = source.ExtendedEmployment.Commission;
                target.ExtendedEmployment.ConfirmedAllowances = source.ExtendedEmployment.ConfirmedAllowances;
                target.ExtendedEmployment.ConfirmedBasicIncome = source.ExtendedEmployment.ConfirmedBasicIncome;
                target.ExtendedEmployment.ConfirmedCommission = source.ExtendedEmployment.ConfirmedCommission;
                target.ExtendedEmployment.ConfirmedMedicalAid = source.ExtendedEmployment.ConfirmedMedicalAid;
                target.ExtendedEmployment.ConfirmedOvertime = source.ExtendedEmployment.ConfirmedOvertime;
                target.ExtendedEmployment.ConfirmedPAYE = source.ExtendedEmployment.ConfirmedPAYE;
                target.ExtendedEmployment.ConfirmedPensionProvident = source.ExtendedEmployment.ConfirmedPensionProvident;
                target.ExtendedEmployment.ConfirmedPerformance = source.ExtendedEmployment.ConfirmedPerformance;
                target.ExtendedEmployment.ConfirmedShift = source.ExtendedEmployment.ConfirmedShift;
                target.ExtendedEmployment.ConfirmedUIF = source.ExtendedEmployment.ConfirmedUIF;
                target.ExtendedEmployment.MedicalAid = source.ExtendedEmployment.MedicalAid;
                target.ExtendedEmployment.Overtime = source.ExtendedEmployment.Overtime;
                target.ExtendedEmployment.PAYE = source.ExtendedEmployment.PAYE;
                target.ExtendedEmployment.PensionProvident = source.ExtendedEmployment.PensionProvident;
                target.ExtendedEmployment.Performance = source.ExtendedEmployment.Performance;
                target.ExtendedEmployment.Shift = source.ExtendedEmployment.Shift;
                target.ExtendedEmployment.UIF = source.ExtendedEmployment.UIF;
            }
            */
        }

        #endregion



    }
}
