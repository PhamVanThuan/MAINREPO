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

namespace SAHL.Common.BusinessModel.Rules.Test.BulkBatch
{
    [TestFixture]
    public class BulkBatchParameter : RuleBase
    {
        private IBulkBatchParameter _bulkBatchParameter;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _bulkBatchParameter = _mockery.StrictMock<IBulkBatchParameter>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }


        /// <summary>
        /// Tests the BulkBatchIdentifierReferenceKeyMandatory rule.
        /// </summary>
        [NUnit.Framework.Test]
        public void BulkBatchParameterArrearBalanceMultipleTest()
        {
            BulkBatchParameterArrearBalanceMultiple rule = new BulkBatchParameterArrearBalanceMultiple();

            // parameter of different type - will be ignored
            SetupResult.For(_bulkBatchParameter.ParameterName).Return(BulkBatchParameterNames.SPV.ToString());
            ExecuteRule(rule, 0, _bulkBatchParameter);

            // parameter of correct type but no value- will be ignored
            SetupResult.For(_bulkBatchParameter.ParameterName).Return(BulkBatchParameterNames.ArrearBalance.ToString());
            SetupResult.For(_bulkBatchParameter.ParameterValue).Return("");
            ExecuteRule(rule, 0, _bulkBatchParameter);

            // parameter of correct type with value that is not multiple of 500 - will fail
            SetupResult.For(_bulkBatchParameter.ParameterName).Return(BulkBatchParameterNames.ArrearBalance.ToString());
            SetupResult.For(_bulkBatchParameter.ParameterValue).Return("501");
            ExecuteRule(rule, 1, _bulkBatchParameter);

            // parameter of correct type with value that is not multiple of 500 - will fail
            SetupResult.For(_bulkBatchParameter.ParameterName).Return(BulkBatchParameterNames.ArrearBalance.ToString());
            SetupResult.For(_bulkBatchParameter.ParameterValue).Return("500.01");
            ExecuteRule(rule, 1, _bulkBatchParameter);

            // parameter of correct type with value that is multiple of 500 - will pass
            SetupResult.For(_bulkBatchParameter.ParameterName).Return(BulkBatchParameterNames.ArrearBalance.ToString());
            SetupResult.For(_bulkBatchParameter.ParameterValue).Return("500");
            ExecuteRule(rule, 0, _bulkBatchParameter);

        }
    }
}
