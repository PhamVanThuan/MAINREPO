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

namespace SAHL.Web.Test.Presenters.Common.Employment
{
    [TestFixture]
    [Ignore]
    public class LegalEntityEmploymentAddPresenter : LegalEntityEmploymentBase
    {

        protected const string SaveButtonClicked = "SaveButtonClicked";

        /// <summary>
        /// </summary>
        [NUnit.Framework.Test]
        public void ViewInitialise()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            OnInitialiseCommon();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentAdd presenter = new LegalEntityEmploymentAdd(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void SaveButtonClickNoEmploymentTypeSelected()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser saveClickEventRaiser = dictEvents[SaveButtonClicked];

            Expect.Call(_view.GetCapturedEmployment()).Return(null);


            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentAdd presenter = new LegalEntityEmploymentAdd(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            saveClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewPreRender()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];

            IEmployment employment = _mockery.CreateMock<IEmployment>();
            EmploymentDetails employmentDetails = new EmploymentDetails();
            SetupResult.For(_view.EmploymentDetails).Return(employmentDetails);
            SetupResult.For(_view.GetCapturedEmployment()).Return(employment);
            SetupResult.For(_view.ShouldRunPage).Return(true);

            _view.SaveButtonVisible = true;
            _view.CancelButtonVisible = true;
            employmentDetails.EmploymentTypeReadOnly = false;
            employmentDetails.EmploymentStatusReadOnly = false;
            employmentDetails.RemunerationTypeReadOnly = false;
            employmentDetails.StartDateReadOnly = false;
            employmentDetails.EndDateReadOnly = false;
            employmentDetails.BasicIncomeReadOnly = false;
            employmentDetails.DepartmentReadOnly = false;

            SetupResult.For(employment.RequiresExtended).Return(true);
            _view.SaveButtonText = "Next";

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentAdd presenter = new LegalEntityEmploymentAdd(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void SaveButtonClickInvalidEmployment()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser saveClickEventRaiser = dictEvents[SaveButtonClicked];

            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IDomainMessageCollection dmc = _mockery.CreateMock<IDomainMessageCollection>();

            Expect.Call(_view.GetCapturedEmployment()).Return(employment);
            employment.LegalEntity = LegalEntity;

            List<string> rules = _mockery.CreateMock<List<string>>();
            Expect.Call(employment.RequiresExtended).Return(false).Repeat.Any();
            Expect.Call(employment.ValidateEntity()).Return(false);
            // Expect.Call(employment.ExcludedRules).Return(rules);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentAdd presenter = new LegalEntityEmploymentAdd(_view, controller);

            // add an error to the domain messages collection deliberately
            _view.Messages.Add(new Error("test", "test"));

            initEventRaiser.Raise(_view, new EventArgs());
            saveClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void SaveButtonClickValidEmployment()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser saveClickEventRaiser = dictEvents[SaveButtonClicked];

            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IDomainMessageCollection dmc = _mockery.CreateMock<IDomainMessageCollection>();

            Expect.Call(_view.GetCapturedEmployment()).Return(employment);
            employment.LegalEntity = LegalEntity;

            List<string> rules = _mockery.CreateMock<List<string>>();
            Expect.Call(employment.RequiresExtended).Return(false).Repeat.Any();
            Expect.Call(employment.ValidateEntity()).Return(false);
            // Expect.Call(employment.ExcludedRules).Return(rules);
            EmploymentRepository.SaveEmployment(employment);
            _view.ShouldRunPage = false;
            SetupNavigationMock(_view, "LegalEntityEmploymentDisplay");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentAdd presenter = new LegalEntityEmploymentAdd(_view, controller);

            initEventRaiser.Raise(_view, new EventArgs());
            saveClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Common initialisation procedure.
        /// </summary>
        /// <returns></returns>
        protected override IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            _view.GridPostBack = false;
            Expect.Call(_view.ShouldRunPage).Return(true);
            IDictionary<string, IEventRaiser> dict = base.OnInitialiseCommon();

            EmployerDetails employerDetails = new EmployerDetails();
            SetupResult.For(_view.EmployerDetails).Return(employerDetails);

            // save button click event
            _view.SaveButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erSaveButtonClicked = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(SaveButtonClicked, erSaveButtonClicked);

            return dict;
        }

    }
}
