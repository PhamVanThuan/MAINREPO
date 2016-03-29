using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Rules.BulkBatch;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Rules;

namespace SAHL.Common.BusinessModel.Rules.Test.BulkBatch
{
    [TestFixture]
    public class BatchTransaction : RuleBase
    {
        private IBatchTransaction _batchTransaction;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _batchTransaction = _mockery.StrictMock<IBatchTransaction>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        /// <summary>
        /// Tests the BatchTransactionAmountMandatory rule.
        /// </summary>
        [NUnit.Framework.Test]
        public void BatchTransactionAmountMandatoryTest()
        {
            BatchTransactionAmountMandatory rule = new BatchTransactionAmountMandatory();
            IBatchTransactionStatus status = _mockery.StrictMock<IBatchTransactionStatus>();

            foreach (int statusKey in Enum.GetValues(typeof(BatchTransactionStatuses)))
            {
                int errorCount = 1;
                switch (statusKey)
                {
                    case (int)BatchTransactionStatuses.Deleted:
                    case (int)BatchTransactionStatuses.Error:
                        errorCount = 0;
                        break;
                }

                SetupResult.For(_batchTransaction.BatchTransactionStatus).Return(status);
                SetupResult.For(status.Key).Return(statusKey);
                SetupResult.For(_batchTransaction.Amount).Return(0D);
                ExecuteRule(rule, errorCount, _batchTransaction);

                SetupResult.For(_batchTransaction.BatchTransactionStatus).Return(status);
                SetupResult.For(status.Key).Return(statusKey);
                SetupResult.For(_batchTransaction.Amount).Return(1D);
                ExecuteRule(rule, 0, _batchTransaction);
            }


        }

        /// <summary>
        /// Tests the BatchTransactionAmountMaximum rule.
        /// </summary>
        [NUnit.Framework.Test]
        public void BatchTransactionAmountMaximumTest()
        {
            BatchTransactionAmountMaximum rule = new BatchTransactionAmountMaximum();

            SetupResult.For(_batchTransaction.Amount).Return(10000000.01);
            ExecuteRule(rule, 1, _batchTransaction);

            SetupResult.For(_batchTransaction.Amount).Return(10000000.00);
            ExecuteRule(rule, 0, _batchTransaction);

            SetupResult.For(_batchTransaction.Amount).Return(9999999.99);
            ExecuteRule(rule, 0, _batchTransaction);

        }

        /// <summary>
        /// Tests the BatchTransactionTransactionTypeMandatory rule.
        /// </summary>
        [NUnit.Framework.Test]
        public void BatchTransactionTransactionTypeMandatoryTest()
        {
            BatchTransactionTransactionTypeMandatory rule = new BatchTransactionTransactionTypeMandatory();

            SetupResult.For(_batchTransaction.TransactionTypeNumber).Return(0);
            ExecuteRule(rule, 1, _batchTransaction);

            SetupResult.For(_batchTransaction.TransactionTypeNumber).Return(1);
            ExecuteRule(rule, 0, _batchTransaction);

        }

    }
}
