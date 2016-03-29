using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Web.Views.Administration.Presenters;
using Rhino.Mocks.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using Rhino.Mocks.Constraints;
using Rhino.Mocks;
using SAHL.Web.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Test.Presenters.Admin
{
    [Ignore]
    [TestFixture]
    public class EmployerUpdatePresenter : EmployerBasePresenter
    {

        #region Attributes and Constants

        private const string ClearFormButtonClicked = "ClearFormButtonClicked";
        private const string UpdateButtonClicked = "UpdateButtonClicked";
        protected const string EmployerSelectedUpdate = "EmployerSelectedUpdate";

        #endregion

        [NUnit.Framework.Test]
        public void TestCancelClicked()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(View)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser cancelClickEventRaiser = dictEvents[CancelButtonClicked];

            OnCancelClicked();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();

            EmployerUpdate presenter = new EmployerUpdate(View, controller);
            initEventRaiser.Raise(View, new EventArgs());
            cancelClickEventRaiser.Raise(View, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Tests the clear form button click event.
        /// </summary>
        [NUnit.Framework.Test]
        public void TestClearFormClicked()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(View)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser clearFormClickEventRaiser = dictEvents[ClearFormButtonClicked];

            View.ClearEmployer();
            View.EditMode = EmployerDetailsEditMode.EditName;

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            EmployerUpdate presenter = new EmployerUpdate(View, controller);
            initEventRaiser.Raise(View, new EventArgs());
            clearFormClickEventRaiser.Raise(View, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void TestEmployerSelected()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(View)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser empSelectedEventRaiser = dictEvents[EmployerSelected];

            OnEmployerSelected();
            View.EditMode = EmployerDetailsEditMode.EditDetails;

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();

            // using the Add presenter - any of the employer presenters can be used 
            EmployerUpdate presenter = new EmployerUpdate(View, controller);
            initEventRaiser.Raise(View, new EventArgs());
            empSelectedEventRaiser.Raise(View, new KeyChangedEventArgs(1));
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Ensures that the correct properties are set on initialisation.
        /// </summary>
        [NUnit.Framework.Test]
        public void TestInit()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(View)[VIEWINIT];

            OnInitialiseCommon();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            EmployerUpdate presenter = new EmployerUpdate(View, controller);
            eventRaiser.Raise(View, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Tests the update button click event, where a domain error message occurs.
        /// </summary>
        [NUnit.Framework.Test]
        public void TestUpdateClickedWithDomainError()
        {
            TestUpdateClicked(true);
        }

        /// <summary>
        /// Tests the update button click event, where no domain error message occurs.
        /// </summary>
        [NUnit.Framework.Test]
        public void TestUpdateClickedWithoutDomainError()
        {
            TestUpdateClicked(false);
        }

        /// <summary>
        /// Tests the update button click event.
        /// </summary>
        /// <param name="withDomainError">Whether a domain error message is returned when the save is attempted.</param>
        protected void TestUpdateClicked(bool withDomainError)
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(View)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser addClickEventRaiser = dictEvents[UpdateButtonClicked];

            IEmployer employer = _mockery.CreateMock<IEmployer>();
            SetupResult.For(View.SelectedEmployer).IgnoreArguments().Return(employer);
            // Expect.Call(View.SelectedEmployer).IgnoreArguments();
            EmploymentRepository.SaveEmployer( null, null);
            LastCall.IgnoreArguments();

            // create the validation summary for the view - if we are testing that an error occurred then add an error 
            // domain message
            SAHLValidationSummary valSummary = new SAHLValidationSummary();
            if (withDomainError)
                valSummary.DomainMessages.Add(new Error("test", "test"));
            // SetupResult.For(View.ValidationSummary).Return(valSummary);

            if (!withDomainError)
            {
                ISimpleNavigator navigator = _mockery.CreateMock<ISimpleNavigator>();
                SetupResult.For(View.Navigator).Return(navigator);
                navigator.Navigate("EmployerDetails");
            }

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            EmployerUpdate presenter = new EmployerUpdate(View, controller);
            initEventRaiser.Raise(View, new EventArgs());
            addClickEventRaiser.Raise(View, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Overridden to add the extra event handlers.
        /// </summary>
        /// <returns></returns>
        protected override IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            IDictionary<string, IEventRaiser> dict = base.OnInitialiseCommon();

            // clear form button click event
            View.ClearFormButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erClearFormButton = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(ClearFormButtonClicked, erClearFormButton);

            // update button click event
            View.UpdateButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erUpdateButton = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(UpdateButtonClicked, erUpdateButton);

            // employer selected event (specific to update)
            View.EmployerSelected += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erEmployerSelectedUpdate = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(EmployerSelectedUpdate, erEmployerSelectedUpdate);

            return dict;

        }

    }
}
