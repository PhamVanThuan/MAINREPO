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
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Web.Test.Presenters.Admin
{

    [Ignore]
    [TestFixture]
    public class BulkTransactionBase : TestViewBase
    {
        private IBulkTransactionBatch _view;
        private IBulkBatchRepository bulkBatchRepo;
        private IEmploymentRepository empRepo;
        private ILookupRepository lookups; 
        private IEventRaiser _initialiseEventRaiser;
        private IEventRaiser _preRenderEventRaiser;
        private SAHLCommonBaseController controller;

        [SetUp]
        public void Setup()
        {
            _mockery = new MockRepository();
            _view = _mockery.CreateMock<IBulkTransactionBatch>();

            SetupMockedView(_view);
            SetupPrincipalCache(base.TestPrincipal);

            base.SetRepositoryStrategy(SAHL.Test.TypeFactoryStrategy.Mock);
            bulkBatchRepo = _mockery.CreateMock<IBulkBatchRepository>();
            lookups = _mockery.CreateMock<ILookupRepository>();
            empRepo = _mockery.CreateMock<IEmploymentRepository>();

            base.MockCache.Add(typeof(IBulkBatchRepository).ToString(), bulkBatchRepo);
            base.MockCache.Add(typeof(ILookupRepository).ToString(),lookups);
            base.MockCache.Add(typeof(IEmploymentRepository).ToString(),empRepo);

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
            BulkBatchTransactionBase presenter = new BulkBatchTransactionBase(_view, controller);

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

        //[NUnit.Framework.Test]
        //public TestViewInitialised
        //{
          
        //}


    }
}
