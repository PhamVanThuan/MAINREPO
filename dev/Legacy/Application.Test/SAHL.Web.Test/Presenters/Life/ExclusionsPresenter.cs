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
using Rhino.Mocks.Impl;

namespace SAHL.Web.Test.Presenters.Life
{
    [Ignore]
    [TestFixture]
    public class ExclusionsPresenter : LifePresenterBaseTest
    {
        private IExclusions _view;
        private CacheManager _CM;
        private IDomainMessageCollection _messages = new DomainMessageCollection();
        private ILifeRepository _LR;      
        private IReadOnlyEventList<ITextStatement> _lstStatements;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<IExclusions>();
            base.SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal, true);
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _view = null;
            _CM = null;
            _LR = null;           
            _messages = null;
            _lstStatements = null;          
        }
        #endregion

        [Test]
        public void InitialiseTest()
        {
           
            // Hook up life cycle events and return the view initialized event raiser
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            //Hookup IContact view initialisation events.
            OnInitialiseCommon();
        }

        [Test]
        public void PreRenderTest()
        {

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

            _CM = CacheFactory.GetCacheManager("MOCK");

            _LR = _mockery.CreateMock<ILifeRepository>();
            _CM.Add(typeof(ILifeRepository).ToString(), _LR);

            ILookupRepository lookups = _mockery.CreateMock<ILookupRepository>();
            _lstStatements = _mockery.CreateMock<IReadOnlyEventList<ITextStatement>>();
            Expect.Call(_LR.GetTextStatementsForTypes( new int[0])).IgnoreArguments().Return(_lstStatements);
          
            // make sure we are calling the binds from initialise            
            BindOnInitialise();


            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            _view.OnNextButtonClicked += null;
            IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnNextButtonClicked", EventRaiser);


            return dict;
        }

        /// <summary>
        /// CancelPolicy view Binding during initialise.
        /// </summary>
        private void BindOnInitialise()
        {
            //_view.BindControls( null, null, -1, false);
            //LastCall.IgnoreArguments();
        }

        #endregion     
    }
}
