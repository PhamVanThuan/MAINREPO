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
    public class RPARPresenter : LifePresenterBaseTest
    {
        private IRPAR _view;
        private CacheManager _CM;
        private ILifeRepository _LR;
        private IDictionary<string, string> _insurers;
        private IDomainMessageCollection _messages = new DomainMessageCollection();
        private IReadOnlyEventList<ITextStatement> _lstStatements;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<IRPAR>();
            base.SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _view = null;
            _CM = null;
            _LR = null;
            _insurers = null;
            _messages = null;
            _lstStatements = null;            
        }

        #endregion


        [NUnit.Framework.Test]
        public void InitialiseTest()
        {
            // Hook up life cycle events and return the view initialized event raiser
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            //Hookup IContact view initialisation events.
            OnInitialiseCommon();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            RPAR presenter = new RPAR(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [Test]
        public void PreRenderTest()
        {

        }

        #region Private Helpers

        /// <summary>
        /// Hooks the IContact view events.
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
            _lstStatements = _mockery.CreateMock < IReadOnlyEventList<ITextStatement>>();
            Expect.Call(_LR.GetTextStatementsForTypes( new int[0])).IgnoreArguments().Return(_lstStatements);

            IDictionary<int,string> _insurers = _mockery.CreateMock<IDictionary<int,string>>();
            Expect.Call(LookupRepository.Insurers).Return(_insurers);
         
            // make sure we are calling the binds from initialise            
            BindOnInitialise();

            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            _view.OnConsideringButtonClicked += null;
            IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnConsideringButtonClicked", EventRaiser);

            _view.OnDDLAssurerSelectedIndexChanged += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnDDLAssurerSelectedIndexChanged", EventRaiser);

            _view.OnNextButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnNextButtonClicked", EventRaiser);

            _view.OnNTUButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnNTUButtonClicked", EventRaiser);

            _view.OnYesNoButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnYesNoButtonClicked", EventRaiser);
            return dict;
        }
        

        /// <summary>
        /// IContact view Binding during initialise.
        /// </summary>
        private void BindOnInitialise()
        {
            _view.BindReplacePolicyConditions(_lstStatements,false);

            _view.BindInsurers(_insurers);
            LastCall.IgnoreArguments();
        }

        #endregion
    }
}
