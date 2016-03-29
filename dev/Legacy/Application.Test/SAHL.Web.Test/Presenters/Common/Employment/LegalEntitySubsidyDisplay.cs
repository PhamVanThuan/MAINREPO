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

namespace SAHL.Web.Test.Presenters.Common.Employment
{
    [TestFixture]
    [Ignore]
    public class LegalEntitySubsidyDisplayPresenter : LegalEntitySubsidyBasePresenter
    {

        [NUnit.Framework.Test]
        public void BackButtonClick()
        {
            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = base.OnInitialiseCommon();
            IEventRaiser btnClickEventRaiser = dictEvents[BackButtonClicked];

            SetupNavigationMock(_view, "Test");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntitySubsidyBase presenter = new LegalEntitySubsidyBase(_view, controller);

            this.CurrentPrincipalCache.GetGlobalData().Add(ViewConstants.NavigateTo, "Test", new List<ICacheObjectLifeTime>());
            initEventRaiser.Raise(_view, new EventArgs());
            btnClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void CancelButtonClick()
        {
            IEmployment employment = _mockery.CreateMock<IEmployment>();
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            IDictionary<string, IEventRaiser> dictEvents = base.OnInitialiseCommon();
            IEventRaiser btnClickEventRaiser = dictEvents[CancelButtonClicked];

            _view.ShouldRunPage = false;
            SetupNavigationMock(_view, "Cancel");

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            LegalEntitySubsidyBase presenter = new LegalEntitySubsidyBase(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            btnClickEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        // <summary>
        // </summary>
        //[NUnit.Framework.Test]
        //public void ViewInitialise()
        //{
        //    IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

        //    OnInitialiseCommon();

        //    SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
        //    _mockery.ReplayAll();
        //    LegalEntitySubsidyDetails presenter = new LegalEntitySubsidyDetails(_view, controller);
        //    eventRaiser.Raise(_view, new EventArgs());
        //    _mockery.VerifyAll();
        //}

        #region Helper Methods

        /// <summary>
        /// Common initialisation procedure.
        /// </summary>
        /// <returns></returns>
        protected override IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            Expect.Call(_view.ShouldRunPage).Return(true);
            IDictionary<string, IEventRaiser> dict = base.OnInitialiseCommon();
            IEmploymentSubsidised employmentSubsidised = _mockery.CreateMock<IEmploymentSubsidised>();
            ISubsidy subsidy = _mockery.CreateMock<ISubsidy>();

            _view.GridVisible = false;
            _view.ReadOnly = true;
            _view.ShowButtons = false;

            Expect.Call(CachedEmployment.Key).Return(0);
            Expect.Call(EmploymentRepository.GetEmploymentByKey(0)).IgnoreArguments().Return(employmentSubsidised);
            Expect.Call(employmentSubsidised.Subsidy).Return(subsidy);
            _view.SetSubsidy(subsidy);

            return dict;
        }

        #endregion



    }
}
