using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Web.Views.Life.Interfaces;
using Rhino.Mocks.Interfaces;
using SAHL.Web.Views.Life.Presenters;
using SAHL.Web.Controllers.Life;
using SAHL.Common.Web.UI;
using Rhino.Mocks;

namespace SAHL.Web.Test.Presenters.Life
{
    [Ignore]
    [TestFixture]
    public class PolicyAdminTest : LifePresenterBaseTest
    {
        private IPolicy _view;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<IPolicy>();
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
            IEventRaiser InitEvent = LifeCycleEventsCommon(_view)[VIEWINIT];
            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            PolicyAdmin PA = new PolicyAdmin(_view, controller);
            _mockery.ReplayAll();

            InitEvent.Raise(_view, new EventArgs());
            _mockery.VerifyAll();

        }
    }
}
