using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Rules;
using SAHL.Common.BusinessModel.Rules.BulkBatch;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Rules.Test.BulkBatch
{
    [TestFixture]
    public class BulkBatch : RuleBase
    {
        private IBulkBatch _bulkBatch;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _bulkBatch = _mockery.StrictMock<IBulkBatch>();
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
        public void BulkBatchIdentifierReferenceKeyMandatoryTest()
        {
            BulkBatchIdentifierReferenceKeyMandatory rule = new BulkBatchIdentifierReferenceKeyMandatory();
            IBulkBatchType bulkBatchType = _mockery.StrictMock<IBulkBatchType>();

            // null type with bad reference key - will pass
            SetupResult.For(_bulkBatch.BulkBatchType).Return(null);
            SetupResult.For(_bulkBatch.IdentifierReferenceKey).Return(-1);
            ExecuteRule(rule, 0, _bulkBatch);

            foreach (int bulkBatchTypeKey in Enum.GetValues(typeof(BulkBatchTypes)))
            {
                // will always pass
                SetupResult.For(_bulkBatch.BulkBatchType).Return(bulkBatchType);
                SetupResult.For(bulkBatchType.Key).Return(bulkBatchTypeKey);
                SetupResult.For(_bulkBatch.IdentifierReferenceKey).Return(1);
                ExecuteRule(rule, 0, _bulkBatch);

                // no reference key will sometimes fail depending on type
                SetupResult.For(_bulkBatch.BulkBatchType).Return(bulkBatchType);
                SetupResult.For(bulkBatchType.Key).Return(bulkBatchTypeKey);
                SetupResult.For(_bulkBatch.IdentifierReferenceKey).Return(0);
                int messageCount = 1;
                if (bulkBatchTypeKey == (int)BulkBatchTypes.DataReportBatch
                    || bulkBatchTypeKey == (int)BulkBatchTypes.QuarterlyLoanStatements)
                    messageCount = 0;
                ExecuteRule(rule, messageCount, _bulkBatch);
            }
        }

        /// <summary>
        /// Tests the BulkBatchExportArrearBalanceParameterMandatory rule.
        /// </summary>
        [NUnit.Framework.Test]
        public void BulkBatchExportArrearBalanceParameterMandatoryTest()
        {
            BulkBatchExportArrearBalanceParameterMandatory rule = new BulkBatchExportArrearBalanceParameterMandatory();

            // unexpected type - will pass
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapImportClientList, BulkBatchParameterNames.ArrearBalance.ToString(), null, 0);

            // no bulk parameters - fail
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapExtractClientList, null, null, 1);

            // add a parameter of a different type - should fail as no ArrearBalance parameter
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapExtractClientList, BulkBatchParameterNames.Category.ToString(), "test", 1);

            // add an ArrearBalance parameter but with no value - will still fail
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapExtractClientList, BulkBatchParameterNames.ArrearBalance.ToString(), null, 1);

            // add an ArrearBalance parameter with a value - will pass
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapExtractClientList, BulkBatchParameterNames.ArrearBalance.ToString(), "1", 0);
        }

        private void BulkBatchExportParameterHelper(BusinessRuleBase rule, BulkBatchTypes bulkBatchType, string parameterName, string parameterValue, int expectedMessageCount)
        {
            IEventList<IBulkBatchParameter> lstParameters = new EventList<IBulkBatchParameter>();
            IBulkBatchType batchType = _mockery.StrictMock<IBulkBatchType>();

            SetupResult.For(_bulkBatch.BulkBatchType).Return(batchType);
            SetupResult.For(batchType.Key).Return((int)bulkBatchType);
            SetupResult.For(_bulkBatch.BulkBatchParameters).Return(lstParameters);

            if (parameterName != null)
            {
                IBulkBatchParameter parm = _mockery.StrictMock<IBulkBatchParameter>();
                SetupResult.For(parm.ParameterName).Return(parameterName);
                SetupResult.For(parm.ParameterValue).Return(parameterValue);
                lstParameters.Add(null, parm);
            }

            ExecuteRule(rule, expectedMessageCount, _bulkBatch);
        }

        /// <summary>
        /// Tests the BulkBatchExportSPVParameterMandatory rule.
        /// </summary>
        [NUnit.Framework.Test]
        public void BulkBatchExportSPVParameterMandatoryTest()
        {
            BulkBatchExportSPVParameterMandatory rule = new BulkBatchExportSPVParameterMandatory();

            // unexpected type - will pass
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapImportClientList, BulkBatchParameterNames.SPV.ToString(), null, 0);

            // no bulk parameters - fail
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapExtractClientList, null, null, 1);

            // add a parameter of a different type - should fail as no ArrearBalance parameter
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapExtractClientList, BulkBatchParameterNames.Category.ToString(), "test", 1);

            // add an ArrearBalance parameter but with no value - will still fail
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapExtractClientList, BulkBatchParameterNames.SPV.ToString(), null, 1);

            // add an ArrearBalance parameter with a value - will pass
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapExtractClientList, BulkBatchParameterNames.SPV.ToString(), "1", 0);
        }

        /// <summary>
        /// Tests the BulkBatchEffectiveDateMandatory rule.
        /// </summary>
        [NUnit.Framework.Test]
        public void BulkBatchEffectiveDateMandatoryTest()
        {
            BulkBatchEffectiveDateMandatory rule = new BulkBatchEffectiveDateMandatory();

            SetupResult.For(_bulkBatch.EffectiveDate).Return(DateTime.MinValue);
            ExecuteRule(rule, 1, _bulkBatch);

            SetupResult.For(_bulkBatch.EffectiveDate).Return(DateTime.Now);
            ExecuteRule(rule, 0, _bulkBatch);
        }

        /// <summary>
        /// Tests the BulkBatchImportFileParameterMandatory rule.
        /// </summary>
        [NUnit.Framework.Test]
        public void BulkBatchImportFileParameterMandatoryTest()
        {
            BulkBatchImportFileParameterMandatory rule = new BulkBatchImportFileParameterMandatory();

            // unexpected type - will pass
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapMailingHouseExtract, BulkBatchParameterNames.FileName.ToString(), null, 0);

            // no bulk parameters - fail
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapImportClientList, null, null, 1);

            // add a parameter of a different type - should fail as no FileName parameter
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapImportClientList, BulkBatchParameterNames.Category.ToString(), "test", 1);

            // add an ArrearBalance parameter but with no value - will still fail
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapImportClientList, BulkBatchParameterNames.FileName.ToString(), null, 1);

            // add an ArrearBalance parameter with a value - will pass
            BulkBatchExportParameterHelper(rule, BulkBatchTypes.CapImportClientList, BulkBatchParameterNames.FileName.ToString(), "1", 0);
        }

        /// <summary>
        /// Success : Bulk Batch Already Posted
        /// </summary>
        [NUnit.Framework.Test]
        public void BulkBatchAlreadyPostedSuccess()
        {
            BulkBatchAlreadyPosted rule = new BulkBatchAlreadyPosted(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            IBulkBatch bulkBatch = _mockery.StrictMock<IBulkBatch>();
            IBulkBatchType bulkBatchType = _mockery.StrictMock<IBulkBatchType>();

            //Set a date that will never exist as a bulk effective date
            DateTime effectiveDate = new DateTime(1900, 01, 01);
            int identifierReferenceKey = 1;
            int bulkBatchTypeKey = 1;

            SetupResult.For(bulkBatch.EffectiveDate).Return(effectiveDate);
            SetupResult.For(bulkBatch.IdentifierReferenceKey).Return(identifierReferenceKey);
            SetupResult.For(bulkBatchType.Key).Return(bulkBatchTypeKey);
            SetupResult.For(bulkBatch.BulkBatchType).Return(bulkBatchType);

            ExecuteRule(rule, 0, bulkBatch);
        }

        /// <summary>
        /// Fail : Bulk Batch Already Posted
        /// </summary>
        [NUnit.Framework.Test]
        public void BulkBatchAlreadyPostedFail()
        {
            BulkBatchAlreadyPosted rule = new BulkBatchAlreadyPosted(RepositoryFactory.GetRepository<ICastleTransactionsService>());

            IBulkBatch bulkBatch = _mockery.StrictMock<IBulkBatch>();
            IBulkBatchType bulkBatchType = _mockery.StrictMock<IBulkBatchType>();

            ParameterCollection parameters = new ParameterCollection();
            IDbConnection con = Helper.GetSQLDBConnection("DBConnectionString");
            DataTable DT = new DataTable();
            string query = @"select top 1
								EffectiveDate,
								IdentifierReferenceKey,
								BulkBatchTypeKey
							from
								BulkBatch
							where
								BulkBatchStatusKey != 2
                            and BulkBatchTypeKey not in (5, 6)";
            Helper.FillFromQuery(DT, query, con, parameters);

            DateTime effectiveDate = Convert.ToDateTime(DT.Rows[0][0]);
            int identifierReferenceKey = Convert.ToInt32(DT.Rows[0][1]);
            int bulkBatchTypeKey = Convert.ToInt32(DT.Rows[0][2]);

            SetupResult.For(bulkBatch.EffectiveDate).Return(effectiveDate);
            SetupResult.For(bulkBatch.IdentifierReferenceKey).Return(identifierReferenceKey);
            SetupResult.For(bulkBatchType.Key).Return(bulkBatchTypeKey);
            SetupResult.For(bulkBatch.BulkBatchType).Return(bulkBatchType);

            ExecuteRule(rule, 1, bulkBatch);
        }
    }
}