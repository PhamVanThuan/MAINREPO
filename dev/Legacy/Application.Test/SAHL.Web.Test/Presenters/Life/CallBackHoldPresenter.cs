using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Web;
using SAHL.Web.Views.Common.Interfaces;
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
using System.Collections;
using SAHL.Common.Globals;

namespace SAHL.Web.Test.Presenters.Life
{
    [Ignore]
    [TestFixture]
    public class CallBackHoldPresenter : LifePresenterBaseTest
    {
        private ICallBackHold _view;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<SAHL.Web.Views.Life.Interfaces.ICallBackHold>();
            base.SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal, true);
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
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ClearMockCache();
            // Hook up life cycle events and return the view initialized event raiser
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            //Hookup IContact view initialisation events.
            OnInitialiseCommon();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            CallBackHold presenter = new CallBackHold(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        #region Private Helpers

        /// <summary>
        /// Hooks the CancelPolicy view events.
        /// </summary>
        /// <returns>A Dictionary collection of IEventRaiser objects, one for each IContact view event.</returns>
        private IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {

            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ClearMockCache();

            CacheManager CM = CacheFactory.GetCacheManager("MOCK");

            // setup an offer repository
            IApplicationRepository OR = _mockery.CreateMock<IApplicationRepository>();
            CM.Add(typeof(IApplicationRepository).ToString(), OR);

            // setup a lookup repository
            ILookupRepository LR = _mockery.CreateMock<ILookupRepository>();
            CM.Add(typeof(ILookupRepository).ToString(), LR);


            IApplication offer = _mockery.CreateMock<IApplication>();

            IDomainMessageCollection messages = _mockery.CreateMock<IDomainMessageCollection>();

            SetupResult.For(OR.GetApplicationByKey(-1)).IgnoreArguments().Return(offer);

            SAHL.Common.BusinessModel.Interfaces.ICallback callback = _mockery.CreateMock<SAHL.Common.BusinessModel.Interfaces.ICallback>();

            SetupResult.For(OR.GetCallBacksByApplicationKey( -1,false)).IgnoreArguments().Return(callback);



            BindOnInitialise();

            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            return dict;
        }

        /// <summary>
        /// CancelPolicy view Binding during initialise.
        /// </summary>
        private void BindOnInitialise()
        {

            _view.BindCallBackGrid(null);
            LastCall.IgnoreArguments();

        }

        #endregion     
    }
}
