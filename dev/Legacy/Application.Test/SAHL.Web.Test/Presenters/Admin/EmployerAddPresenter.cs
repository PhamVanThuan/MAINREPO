using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Presenters;
using SAHL.Web.Controls;
using Rhino.Mocks.Constraints;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.DomainMessages;
using Microsoft.ApplicationBlocks.UIProcess;

namespace SAHL.Web.Test.Presenters.Admin
{
    [Ignore]
    [TestFixture]
    public class EmployerAddPresenter : EmployerBasePresenter
    {

        #region Attributes and Constants

        private const string AddButtonClicked = "AddButtonClicked";

        #endregion

        /// <summary>
        /// Tests the add button click event, where a domain error message occurs.
        /// </summary>
        [NUnit.Framework.Test]
        public void TestAddClickedWithDomainError()
        {
            TestAddClicked(true);
        }

        /// <summary>
        /// Tests the add button click event, where no domain error message occurs.
        /// </summary>
        [NUnit.Framework.Test]
        public void TestAddClickedWithoutDomainError()
        {
            TestAddClicked(false);
        }

        /// <summary>
        /// Tests the add button click event.
        /// </summary>
        /// <param name="withDomainError">Whether a domain error message is returned when the save is attempted.</param>
        protected void TestAddClicked(bool withDomainError)
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(View)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser addClickEventRaiser = dictEvents[AddButtonClicked];

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
                navigator.Navigate("Display");
            }

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            EmployerAdd presenter = new EmployerAdd(View, controller);
            initEventRaiser.Raise(View, new EventArgs());
            addClickEventRaiser.Raise(View, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void TestCancelClicked()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(View)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser cancelClickEventRaiser = dictEvents[CancelButtonClicked];

            OnCancelClicked();
            
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();

            EmployerAdd presenter = new EmployerAdd(View, controller);
            initEventRaiser.Raise(View, new EventArgs());
            cancelClickEventRaiser.Raise(View, new EventArgs());
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
            EmployerAdd presenter = new EmployerAdd(View, controller);
            eventRaiser.Raise(View, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Ensures that the correct properties are set before rendering the display.
        /// </summary>
        [NUnit.Framework.Test]
        public void TestPreRender()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(this.View)[VIEWPRERENDER];

            // specific values that must be set
            View.EditMode = EmployerDetailsEditMode.EditAll;
            View.AddButtonVisible = true;
            View.CancelButtonVisible = true;

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            EmployerAdd presenter = new EmployerAdd(View, controller);
            eventRaiser.Raise(View, new EventArgs());
            _mockery.VerifyAll();
        }

        /// <summary>
        /// Overridden to add the AddButtonClicked event handler.
        /// </summary>
        /// <returns></returns>
        protected override IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            IDictionary<string, IEventRaiser> dict = base.OnInitialiseCommon();

            // cancel button click event
            View.AddButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erAddButton = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(AddButtonClicked, erAddButton);

            return dict;

        }

    }
}
