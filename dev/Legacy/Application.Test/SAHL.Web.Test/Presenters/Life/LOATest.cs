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
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace SAHL.Web.Test.Presenters.Life
{
    [Ignore]
    [TestFixture]
    public class LOATest : LifePresenterBaseTest
    {
        private ILOA _view;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<ILOA>();
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
        public void Initialise()
        {
            // Hook up life cycle events and return the view initialized event raiser
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            SetupLifeOfferCBONode();

            //Hookup ILOA view initialisation events.
            OnInitialiseCommon();

            // make sure we are calling the binds from initialise
            BindOnInitialise();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            LOA presenter = new LOA(_view, controller);
            _mockery.ReplayAll();

            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();

        }

        #region Private Helpers

        /// <summary>
        /// Hooks the ILOA view events.
        /// </summary>
        /// <returns>A Dictionary collection of IEventRaiser objects, one for each ILOA view event.</returns>
        private IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            _view.OnNextButtonClicked += null;
            IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnNextButtonClicked", EventRaiser);

            return dict;
        }

        /// <summary>
        /// ILOA view Binding during initialise.
        /// </summary>
        private void BindOnInitialise()
        {
            _view.BindLOADetails(null,null,null,null,null,null,null);
            LastCall.IgnoreArguments();

            _view.BindLoanConditions(null);
            LastCall.IgnoreArguments();
        }

        #endregion


    }
}
