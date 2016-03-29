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
using SAHL.BusinessModel.Interfaces.Repositories;
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
    public class CancelPolicyPresenterTest : LifePresenterBaseTest 
    {
        private ICancelPolicy _view;

        #region Setup/TearDown

        [SetUp]
        public void FixtureSetup()
        {
            _view = _mockery.CreateMock<ICancelPolicy>();
            base.SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal, true);
        }

        [TearDown]
        public void FixtureTearDown()
        {
            _view = null;
        }
        #endregion

        [NUnit.Framework.Test]
        public void InitialiseTest()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ClearMockCache();
            // Hook up life cycle events and return the view initialized event raiser
            IEventRaiser initEventRaiser = LifeCycleEventsCommon(_view)[VIEWINIT];

            //Hookup IContact view initialisation events.
            OnInitialiseCommon();

            CacheManager CM = CacheFactory.GetCacheManager("MOCK");

            IDomainMessageCollection messages = new DomainMessageCollection();

            // setup a life repository
            ILifeRepository LR = _mockery.CreateMock<ILifeRepository>();
            CM.Add(typeof(ILifeRepository).ToString(), LR);

            // setup an account repository
            IAccountRepository AR = _mockery.CreateMock<IAccountRepository>();
            CM.Add(typeof(IAccountRepository).ToString(), AR);


            // setup a lookup repository
            ILookupRepository lookupRepository = _mockery.CreateMock<ILookupRepository>();
            CM.Add(typeof(ILookupRepository).ToString(), lookupRepository);

            // setup the data returned by a call to GetCancellationTypes
            IDictionary<string,int> dict = _mockery.CreateMock<IDictionary<string,int>>();      
            SetupResult.For(LR.GetCancellationTypes(messages)).Return(new Dictionary<string,int>());            
            Expect.Call(LR.GetCancellationTypes(null)).IgnoreArguments().Return(dict);
            
            SAHL.Common.BusinessModel.Interfaces.IAccount account = _mockery.CreateMock<SAHL.Common.BusinessModel.Interfaces.IAccount>();
            SetupResult.For(AR.GetAccountByKey(messages,-1)).Return(account);
            Expect.Call(AR.GetAccountByKey(null, -1)).IgnoreArguments().Return(account);

            ReadOnlyEventList<ILifePolicy> policy = _mockery.CreateMock<ReadOnlyEventList<ILifePolicy>>();
            SetupResult.For(account.GetFinancialServiceByType(SAHL.Common.Globals.FinancialServiceTypes.LifePolicy)).IgnoreArguments().Return(policy);
                       
            IEventList<IReasonType> reasonTypes = _mockery.CreateMock<IEventList<IReasonType>>();
            SetupResult.For(lookupRepository.ReasonTypes).IgnoreArguments().Return(reasonTypes);
            
            Expect.Call(reasonTypes.Count).IgnoreArguments().Return(0);

            Expect.Call(policy[0].DateOfAcceptance).IgnoreArguments().Return(DateTime.Today);

            Expect.Call(policy[0].DateOfCommencement).IgnoreArguments().Return(DateTime.Today);


            // make sure we are calling the binds from initialise            
            BindOnInitialise();

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            CancelPolicy presenter = new CancelPolicy(_view, controller);
            initEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void PreRenderTestWithPolicyStatusAcceptedAndDaysSinceAcceptedDateLessThanThirty()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ClearMockCache();

            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];
           // specific values that must be set

            _view.CancelFromInceptionEnabled = true;
            _view.CancelWithAuthorizationEnabled = false;         
            _view.CancelWithNoRefundEnabled = false;        
            _view.CancelWithProRateEnabled = false;
          

            CacheManager CM = CacheFactory.GetCacheManager("MOCK");

            IDomainMessageCollection messages = new DomainMessageCollection();

            // setup a life repository
            ILifeRepository LR = _mockery.CreateMock<ILifeRepository>();
            CM.Add(typeof(ILifeRepository).ToString(), LR);

            // setup an account repository
            IAccountRepository AR = _mockery.CreateMock<IAccountRepository>();
            CM.Add(typeof(IAccountRepository).ToString(), AR);

            // setup a lookup repository
            ILookupRepository lookupRepository = _mockery.CreateMock<ILookupRepository>();
            CM.Add(typeof(ILookupRepository).ToString(), lookupRepository);

            ReadOnlyEventList<ILifePolicy> policy = _mockery.CreateMock<ReadOnlyEventList<ILifePolicy>>();
            ILifePolicyStatus LifePolicyStatus = _mockery.CreateMock<ILifePolicyStatus>();
            SetupResult.For(LifePolicyStatus.Key).Return(2);
            SetupResult.For(policy[0].LifePolicyStatus).Return(LifePolicyStatus);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            CancelPolicy presenter = new CancelPolicy(_view, controller);
      //      presenter.Policy = policy;
            presenter.DaysSinceAcceptedDate = 20;
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void PreRenderTestWithPolicyStatusAcceptedAndDaysSinceAcceptedDateGreaterThanThirty()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ClearMockCache();

            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];
            // specific values that must be set
            _view.CancelFromInceptionEnabled = false ;
            _view.CancelFromInceptionEnabled = true;
            _view.CancelWithAuthorizationEnabled = false;
            _view.CancelWithNoRefundEnabled = false;
            _view.CancelWithProRateEnabled = false;



            CacheManager CM = CacheFactory.GetCacheManager("MOCK");

            IDomainMessageCollection messages = new DomainMessageCollection();

            // setup a life repository
            ILifeRepository LR = _mockery.CreateMock<ILifeRepository>();
            CM.Add(typeof(ILifeRepository).ToString(), LR);

            // setup an account repository
            IAccountRepository AR = _mockery.CreateMock<IAccountRepository>();
            CM.Add(typeof(IAccountRepository).ToString(), AR);

            // setup a lookup repository
            ILookupRepository lookupRepository = _mockery.CreateMock<ILookupRepository>();
            CM.Add(typeof(ILookupRepository).ToString(), lookupRepository);

            ReadOnlyEventList<ILifePolicy> policy = _mockery.CreateMock<ReadOnlyEventList<ILifePolicy>>();
            ILifePolicyStatus LifePolicyStatus = _mockery.CreateMock<ILifePolicyStatus>();
            SetupResult.For(LifePolicyStatus.Key).Return(2);
            SetupResult.For(policy[0].LifePolicyStatus).Return(LifePolicyStatus);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            CancelPolicy presenter = new CancelPolicy(_view, controller);
    //        presenter.Policy = policy;
            presenter.DaysSinceAcceptedDate = 50;
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]        
        public void PreRenderTestWithPolicyStatusNOTAcceptedAndDaysSinceAcceptedDateGreaterThanThirty()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ClearMockCache();

            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];
            // specific values that must be set
            _view.CancelFromInceptionEnabled = false;               
            CacheManager CM = CacheFactory.GetCacheManager("MOCK");

            IDomainMessageCollection messages = new DomainMessageCollection();

            // setup a life repository
            ILifeRepository LR = _mockery.CreateMock<ILifeRepository>();
            CM.Add(typeof(ILifeRepository).ToString(), LR);

            // setup an account repository
            IAccountRepository AR = _mockery.CreateMock<IAccountRepository>();
            CM.Add(typeof(IAccountRepository).ToString(), AR);

            // setup a lookup repository
            ILookupRepository lookupRepository = _mockery.CreateMock<ILookupRepository>();
            CM.Add(typeof(ILookupRepository).ToString(), lookupRepository);

            ReadOnlyEventList<ILifePolicy> policy = _mockery.CreateMock<ReadOnlyEventList<ILifePolicy>>();
            ILifePolicyStatus LifePolicyStatus = _mockery.CreateMock<ILifePolicyStatus>();
            SetupResult.For(LifePolicyStatus.Key).Return(1);
            SetupResult.For(policy[0].LifePolicyStatus).Return(LifePolicyStatus);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            CancelPolicy presenter = new CancelPolicy(_view, controller);
        //    presenter.Policy = policy;
            presenter.DaysSinceAcceptedDate = 50;
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

        [NUnit.Framework.Test]
        public void PreRenderTestWithPolicyStatusNOTAcceptedAndDaysSinceAcceptedDateLessThanThirty()
        {
            SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            ClearMockCache();

            IEventRaiser eventRaiser = LifeCycleEventsCommon(_view)[VIEWPRERENDER];
            // specific values that must be set
           
            CacheManager CM = CacheFactory.GetCacheManager("MOCK");

            IDomainMessageCollection messages = new DomainMessageCollection();

            // setup a life repository
            ILifeRepository LR = _mockery.CreateMock<ILifeRepository>();
            CM.Add(typeof(ILifeRepository).ToString(), LR);

            // setup an account repository
            IAccountRepository AR = _mockery.CreateMock<IAccountRepository>();
            CM.Add(typeof(IAccountRepository).ToString(), AR);

            // setup a lookup repository
            ILookupRepository lookupRepository = _mockery.CreateMock<ILookupRepository>();
            CM.Add(typeof(ILookupRepository).ToString(), lookupRepository);

            ReadOnlyEventList<ILifePolicy> policy = _mockery.CreateMock<ReadOnlyEventList<ILifePolicy>>();
            ILifePolicyStatus LifePolicyStatus = _mockery.CreateMock<ILifePolicyStatus>();
            SetupResult.For(LifePolicyStatus.Key).Return(1);
            SetupResult.For(policy[0].LifePolicyStatus).Return(LifePolicyStatus);

            SAHLCommonBaseController controller = new SAHLCommonBaseController(null);
            _mockery.ReplayAll();
            CancelPolicy presenter = new CancelPolicy(_view, controller);
       //     presenter.Policy = policy;
            presenter.DaysSinceAcceptedDate = 20;
            eventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
        }

#region Private Helpers

        /// <summary>
        /// Hooks the CancelPolicy view events.
        /// </summary>
        /// <returns>A Dictionary collection of IEventRaiser objects, one for each IContact view event.</returns>
        private IDictionary<string, IEventRaiser> OnInitialiseCommon()
        {
            Dictionary<string, IEventRaiser> dict = new Dictionary<string, IEventRaiser>();

            _view.OnCancelButtonClicked += null;
            IEventRaiser EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnCancelButtonClicked", EventRaiser);

            _view.OnSubmitButtonClicked += null;
            EventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
            dict.Add("OnSubmitButtonClicked", EventRaiser);
         
            return dict;
        }

        /// <summary>
        /// CancelPolicy view Binding during initialise.
        /// </summary>
        private void BindOnInitialise()
        {
            _view.BindControls(null, null, null, -1, false);
            LastCall.IgnoreArguments();
        }

        #endregion     
    }
}
