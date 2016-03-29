using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using Rhino.Mocks.Interfaces;
using SAHL.Web.Views.Administration.Presenters;
using SAHL.Web.Controls;
using NUnit.Framework;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Test.Presenters.Admin
{
    [TestFixture]
    [Ignore]
    public class EmployerDisplayPresenter : EmployerBasePresenter
    {

        [NUnit.Framework.Test]
        public void TestEmployerSelected()
        {
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(View)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = OnInitialiseCommon();
            IEventRaiser empSelectedEventRaiser = dictEvents[EmployerSelected];

            OnEmployerSelected();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();

            // using the Add presenter - any of the employer presenters can be used 
            EmployerDisplay presenter = new EmployerDisplay(View, controller);
            initEventRaiser.Raise(View, new EventArgs());
            empSelectedEventRaiser.Raise(View, new KeyChangedEventArgs(1));
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
            View.EditMode = EmployerDetailsEditMode.EditName;

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            EmployerDisplay presenter = new EmployerDisplay(View, controller);
            eventRaiser.Raise(View, new EventArgs());
            _mockery.VerifyAll();
        }
    }
}
