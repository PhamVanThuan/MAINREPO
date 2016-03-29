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

namespace SAHL.Web.Test.Presenters.Common.Employment
{
    [TestFixture]
    [Ignore]
    public class LegalEntityEmploymentDisplayPresenter : LegalEntityEmploymentBase
    {

        protected const string SubsidyDetailsClicked = "SubsidyDetailsClicked";

        protected const string ExtendedDetailsClicked = "ExtendedDetailsClicked";

        /// <summary>
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewInitialise()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            OnInitialiseCommon();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentDisplay presenter = new LegalEntityEmploymentDisplay(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderNoEmployerSelected()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];
            // OnInitialiseCommon();

            EmploymentDetails employmentDetails = new EmploymentDetails();
            EmployerDetails employerDetails = new EmployerDetails();

            SetupResult.For(_view.ShouldRunPage).Return(true);
            Expect.Call(_view.SelectedEmployment).Return(null);
            SetupResult.For(_view.EmploymentDetails).Return(employmentDetails);
            SetupResult.For(_view.EmployerDetails).Return(employerDetails);

            employmentDetails.Visible = false;
            employerDetails.Visible = false;

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentDisplay presenter = new LegalEntityEmploymentDisplay(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewPreRenderEmployerSelected()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];
            // OnInitialiseCommon();

            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IEmployer employer = _mockery.CreateMock<IEmployer>();
            IEmployerDetails employerDetails = _mockery.CreateMock<IEmployerDetails>();
            IEmploymentDetails employmentDetails = _mockery.CreateMock<IEmploymentDetails>();

            SetupResult.For(_view.ShouldRunPage).Return(true);
            SetupResult.For(_view.SelectedEmployment).Return(employment);
            SetupResult.For(_view.EmployerDetails).Return(employerDetails);
            SetupResult.For(_view.EmploymentDetails).Return(employmentDetails);
            SetupResult.For(employment.Employer).Return(employer);

            employerDetails.Employer = employer;
            MockPopulateEmploymentDetails(employment, employmentDetails);

            _view.SubsidyDetailsButtonVisible = false;
            Expect.Call(employment.RequiresExtended).Return(false);
            _view.ExtendedDetailsButtonVisible = false;

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentDisplay presenter = new LegalEntityEmploymentDisplay(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void SubsidyDetailsButtonClick()
        {
            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser btnClickEventRaiser = dictEvents[SubsidyDetailsClicked];

            SetupResult.For(_view.SelectedEmployment).Return(employment);
            _view.ShouldRunPage = false;
            SetupNavigationMock(_view, "SubsidyDetails");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentDisplay presenter = new LegalEntityEmploymentDisplay(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            btnClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ExtendedDetailsButtonClick()
        {
            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser btnClickEventRaiser = dictEvents[ExtendedDetailsClicked];

            SetupResult.For(_view.SelectedEmployment).Return(employment);
            _view.ShouldRunPage = false;
            SetupNavigationMock(_view, "EmploymentExtended");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentDisplay presenter = new LegalEntityEmploymentDisplay(_view, controller);
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
            Expect.Call(_view.ShouldRunPage).Return(true);
            IDictionary<string, IEventRaiser> dict = base.OnInitialiseCommon();

            EmployerDetails employerDetails = new EmployerDetails();
            SetupResult.For(_view.EmployerDetails).Return(employerDetails);

            // existing address selected event
            _view.SubsidyDetailsClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erSubsidyDetailsClicked = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(SubsidyDetailsClicked, erSubsidyDetailsClicked);

            _view.ExtendedDetailsClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erExtendedDetailsClicked = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(ExtendedDetailsClicked, erExtendedDetailsClicked);

            return dict;
        }

        #endregion



    }
}
