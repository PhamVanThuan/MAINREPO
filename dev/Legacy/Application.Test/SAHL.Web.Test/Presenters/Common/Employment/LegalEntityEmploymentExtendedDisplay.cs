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
    public class LegalEntityEmploymentExtendedDisplayPresenter : LegalEntityEmploymentExtendedBase
    {

        protected const string CancelClicked = "CancelClicked";

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
            LegalEntityEmploymentExtendedDisplay presenter = new LegalEntityEmploymentExtendedDisplay(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void ViewLoad()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWLOAD];
            SetupResult.For(_view.ShouldRunPage).Return(true);

            IEmployment employment = _mockery.CreateMock<IEmployment>();

            _view.SetEmployment(employment);
            LastCall.IgnoreArguments();
            _view.BackButtonVisible = false;
            _view.SaveButtonVisible = false;

            _view.ConfirmedIncomeReadOnly = true;
            _view.MonthlyIncomeReadOnly = true;

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentExtendedDisplay presenter = new LegalEntityEmploymentExtendedDisplay(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void CancelButtonClick()
        {
            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser btnClickEventRaiser = dictEvents[CancelClicked];

            _view.ShouldRunPage = false;
            SetupNavigationMock(_view, "Cancel");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntityEmploymentExtendedDisplay presenter = new LegalEntityEmploymentExtendedDisplay(_view, controller);
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

            _view.CancelButtonClicked += null;
            LastCall.Constraints(Is.NotNull());
            IEventRaiser erCancelClicked = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add(CancelClicked, erCancelClicked);

            return dict;
        }

        #endregion



    }
}
