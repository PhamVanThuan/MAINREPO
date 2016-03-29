using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Presenters;
using SAHL.Web.Controls;
using Rhino.Mocks.Constraints;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.DomainMessages;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;

namespace SAHL.Web.Test.Presenters.Admin
{
    [Ignore]
    [TestFixture]
    public class BatchProgressPresenter : TestViewBase
    {
        private IBatchProgress _view;
        private IBulkBatchRepository bulkBatchRepo;
        private IEventRaiser _initialiseEventRaiser;
        private IEventRaiser _preRenderEventRaiser;
        private SAHLCommonBaseController controller;
        

        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
            _view = _mockery.CreateMock<IBatchProgress>();

            SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);

            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            bulkBatchRepo = _mockery.CreateMock<IBulkBatchRepository>();
           
            base.ClearMockCache();
            base.MockCache.Add(typeof(IBulkBatchRepository).ToString(), bulkBatchRepo);
        }

        [TearDown]
        public void TearDown()
        {
            _view = null;
            _mockery = null;
        }

        [NUnit.Framework.Test]
        public void TestEventsHookUp()
        {
            SetUpBaseEventExpectancies();

            _mockery.ReplayAll();

            controller = new SAHLCommonBaseController(null);
            BatchProgress presenter = new BatchProgress(_view, controller);

        }

        public void SetUpBaseEventExpectancies()
        {
            _view.ViewInitialised += null;
            LastCall.IgnoreArguments();
            _initialiseEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();

            _view.ViewLoaded += null;
            LastCall.IgnoreArguments();

            _view.ViewPreRender += null;
            LastCall.IgnoreArguments();
            _preRenderEventRaiser = LastCall.IgnoreArguments().GetEventRaiser();
        }

        [NUnit.Framework.Test]
        public void TestInitialise()
        {
            SetUpBaseEventExpectancies();

            SetupResult.For(_view.Messages).Return(new DomainMessageCollection());
            IEventList<IBulkBatchType> bulkBatchType = _mockery.CreateMock<IEventList<IBulkBatchType>>();
            IEventList<IMessageType> messageType = _mockery.CreateMock<IEventList<IMessageType>>();

            SetupResult.For(LookupRepository.BulkBatchTypes).IgnoreArguments().Return(bulkBatchType);
            SetupResult.For(LookupRepository.MessageTypes).IgnoreArguments().Return(messageType);

            _view.BindLookUps(null, null);
            LastCall.IgnoreArguments();

            _view.OnBatchGridSelectedIndexChanged += null;
            LastCall.IgnoreArguments();
            _view.OnMessageTypeListSelectedIndexChange += null;
            LastCall.IgnoreArguments();
            _view.OnBatchTypeListSelectedIndexChange += null;
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();
            BatchProgress presenter = new BatchProgress(_view, controller);
            _initialiseEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();
      
        }


        [NUnit.Framework.Test]
        public void TestPreRender()
        {
            SetUpBaseEventExpectancies();

            SetupResult.For(_view.Messages).Return(new DomainMessageCollection());
            IList<IBulkBatch> bulkBatchLst = _mockery.CreateMock<IList<IBulkBatch>>();
            SetupResult.For(bulkBatchRepo.GetBulkBatchTransactionsByBulkBatchTypeKey(0)).IgnoreArguments().Return(bulkBatchLst);

            IList<IBulkBatchLog> bulkBatchLog = _mockery.CreateMock<IList<IBulkBatchLog>>();
            SetupResult.For(bulkBatchRepo.GetBatchLogByBatchKeyMessageType(0, 0)).IgnoreArguments().Return(bulkBatchLog);

            SetupResult.For(bulkBatchLst.Count).IgnoreArguments().Return(1);

            IBulkBatch bulkBatch = _mockery.CreateMock<IBulkBatch>();

            _view.BindBatchGrid(null);
            LastCall.IgnoreArguments();

            int batchGridIndexSelected = 0;
            SetupResult.For(bulkBatchLst[batchGridIndexSelected]).IgnoreArguments().Return(bulkBatch);

            int genericMessageTypeKey=0;

            _view.BindMessageGrid(null);
            LastCall.IgnoreArguments();

            _mockery.ReplayAll();
            BatchProgress presenter = new BatchProgress(_view, controller);
            _preRenderEventRaiser.Raise(_view, new EventArgs());
            _mockery.VerifyAll();

        }
    }
}
