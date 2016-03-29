using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Web.Views.Life.Interfaces;
using Rhino.Mocks;
using NUnit.Framework;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Presenters;
using Rhino.Mocks.Interfaces;
using SAHL.Test;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace SAHL.Web.Test.Presenters.Life
{
    [Ignore]
    [TestFixture]
    public class BenefitTest : LifePresenterBaseTest
    {
        private IBenefit _view;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<IBenefit>();
            base.SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _view = null;
        }

        #endregion

        [Test]
        public void InitialiseTest()
        {
            #region Expectation
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];
            //SetupLifeOfferCBONode();

            //Hookup IFAIS view initialisation events.
            OnInitialiseCommon();

            // make sure we are calling the binds from initialise
            BindOnInitialise();

            _mockery.ReplayAll(); // stop recording expectations
            #endregion

            #region Actual

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            Benefit presenter = new Benefit(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll(); // Checking to see if expected did happen

            #endregion
        }

        [NUnit.Framework.Test]
        public void PreRenderTest()
        {
            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];

            SetupLifeOfferCBONode();

            // specific values that must be set
            //none yet for this view

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            Benefit presenter = new Benefit(_view, controller);
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        #region Private Helpers
        /// <summary>
        /// Hooks the IDeclaration view events.
        /// </summary>
        /// <returns>A Dictionary collection of IEventRaiser objects, one for each IBenefit view event.</returns>

        private IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.OnNextButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnNextButtonClicked", EventRaiser);

            return dict;
        }

        private void BindOnInitialise()
        {
            _view.BindBenefit(null);
            LastCall.IgnoreArguments();

        }

        #endregion

    }
}

