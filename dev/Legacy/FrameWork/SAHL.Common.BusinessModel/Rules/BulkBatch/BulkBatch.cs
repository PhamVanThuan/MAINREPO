using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.BulkBatch
{
    [RuleDBTag("BulkBatchEffectiveDateMandatory",
       "Ensures the Effective Date is captured",
       "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.BulkBatch.BulkBatchEffectiveDateMandatory")]
    [RuleInfo]
    public class BulkBatchEffectiveDateMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IBulkBatch bulkBatch = (IBulkBatch)Parameters[0];

            // check against 1900/1/1 so we don't get Sql Server date errors
            if (bulkBatch.EffectiveDate <= new DateTime(1900, 1, 1))
            {
                string msg = "Invalid Effective Date.";
                AddMessage(msg, msg, Messages);
                return 0;
            }

            if (bulkBatch.EffectiveDate > System.DateTime.Now)
            {
                string msg = "Effective Date cannot be greater than today's date.";
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("BulkBatchIdentifierReferenceKeyMandatory",
       "Ensures the Identifier Reference Key is captured for a BulkBatch object",
       "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.BulkBatch.BulkBatchIdentifierReferenceKeyMandatory")]
    [RuleInfo]
    public class BulkBatchIdentifierReferenceKeyMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IBulkBatch bulkBatch = (IBulkBatch)Parameters[0];

            // this rule does not apply to some bulk batch jobs
            if (bulkBatch.BulkBatchType == null
                || bulkBatch.BulkBatchType.Key == (int)BulkBatchTypes.DataReportBatch
                || bulkBatch.BulkBatchType.Key == (int)BulkBatchTypes.QuarterlyLoanStatements)
                return 1;

            if (bulkBatch.IdentifierReferenceKey <= 0)
            {
                string msg = "Identifier Reference Key is a mandatory field";
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("BulkBatchExportArrearBalanceParameterMandatory",
       "An Export BulkBatch must have an ArrearBalance parameter.",
       "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.BulkBatch.BulkBatchExportArrearBalanceParameterMandatory")]
    [RuleInfo]
    public class BulkBatchExportArrearBalanceParameterMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IBulkBatch bulkBatch = (IBulkBatch)Parameters[0];
            string msg = "Loan Arrear Balance is a mandatory field";

            // only run the rule if the type is Export
            if (bulkBatch.BulkBatchType == null || bulkBatch.BulkBatchType.Key != (int)BulkBatchTypes.CapExtractClientList)
                return 1;

            // if no parameters exist, we've failed
            if (bulkBatch.BulkBatchParameters == null || bulkBatch.BulkBatchParameters.Count == 0)
            {
                AddMessage(msg, msg, Messages);
                return 0;
            }

            // loop through the parameters
            foreach (IBulkBatchParameter parm in bulkBatch.BulkBatchParameters)
            {
                if (parm.ParameterName == BulkBatchParameterNames.ArrearBalance.ToString() && !String.IsNullOrEmpty(parm.ParameterValue))
                    return 1;
            }

            // if we get here the parameter hasn't been found - raise the error
            AddMessage(msg, msg, Messages);
            return 0;
        }
    }

    [RuleDBTag("BulkBatchExportSPVParameterMandatory",
           "An Export BulkBatch must have an SPV parameter.",
           "SAHL.Rules.DLL",
          "SAHL.Common.BusinessModel.Rules.BulkBatch.BulkBatchExportSPVParameterMandatory")]
    [RuleInfo]
    public class BulkBatchExportSPVParameterMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IBulkBatch bulkBatch = (IBulkBatch)Parameters[0];
            string msg = "At least one SPV must be selected";

            // only run the rule if the type is Export
            if (bulkBatch.BulkBatchType == null || bulkBatch.BulkBatchType.Key != (int)BulkBatchTypes.CapExtractClientList)
                return 1;

            // if no parameters exist, we've failed
            if (bulkBatch.BulkBatchParameters == null || bulkBatch.BulkBatchParameters.Count == 0)
            {
                AddMessage(msg, msg, Messages);
                return 0;
            }

            // loop through the parameters
            foreach (IBulkBatchParameter parm in bulkBatch.BulkBatchParameters)
            {
                if (parm.ParameterName == BulkBatchParameterNames.SPV.ToString() && !String.IsNullOrEmpty(parm.ParameterValue))
                    return 1;
            }

            // if we get here the parameter hasn't been found - raise the error
            AddMessage(msg, msg, Messages);
            return 0;
        }
    }

    [RuleDBTag("BulkBatchImportFileParameterMandatory",
               "An Export BulkBatch must have a file parameter.",
               "SAHL.Rules.DLL",
             "SAHL.Common.BusinessModel.Rules.BulkBatch.BulkBatchImportFileParameterMandatory")]
    [RuleInfo]
    public class BulkBatchImportFileParameterMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IBulkBatch bulkBatch = (IBulkBatch)Parameters[0];
            string msg = "A file must be selected";

            // only run the rule if the type is Export
            if (bulkBatch.BulkBatchType == null || bulkBatch.BulkBatchType.Key != (int)BulkBatchTypes.CapImportClientList)
                return 1;

            // if no parameters exist, we've failed
            if (bulkBatch.BulkBatchParameters == null || bulkBatch.BulkBatchParameters.Count == 0)
            {
                AddMessage(msg, msg, Messages);
                return 0;
            }

            // loop through the parameters
            foreach (IBulkBatchParameter parm in bulkBatch.BulkBatchParameters)
            {
                if (parm.ParameterName == BulkBatchParameterNames.FileName.ToString() && !String.IsNullOrEmpty(parm.ParameterValue))
                    return 1;
            }

            // if we get here the parameter hasn't been found - raise the error
            AddMessage(msg, msg, Messages);
            return 0;
        }
    }

    [RuleDBTag("BulkBatchAlreadyPosted",
      "Ensures the Identifier Reference Key (SusbidyProviderKey) does not already have posted transactions for the same month",
       "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.BulkBatch.BulkBatchAlreadyPosted")]
    [RuleInfo]
    public class BulkBatchAlreadyPosted : BusinessRuleBase
    {
        public BulkBatchAlreadyPosted(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IBulkBatch bulkBatch = (IBulkBatch)Parameters[0];

            // this rule does not apply to some bulk batch jobs
            if (bulkBatch.BulkBatchType == null
                || bulkBatch.BulkBatchType.Key == (int)BulkBatchTypes.DataReportBatch
                || bulkBatch.BulkBatchType.Key == (int)BulkBatchTypes.QuarterlyLoanStatements)
                return 1;

            string query = UIStatementRepository.GetStatement("COMMON", "BulkBatchAlreadyPosted");

            // Create a collection and add the required parameters
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@BulkBatchTypeKey", bulkBatch.BulkBatchType.Key));
            prms.Add(new SqlParameter("@Month", bulkBatch.EffectiveDate.Month));
            prms.Add(new SqlParameter("@Year", bulkBatch.EffectiveDate.Year));
            prms.Add(new SqlParameter("@IdentifierReferenceKey", bulkBatch.IdentifierReferenceKey));

            // execute
            object returnVal = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(Account_DAO), prms);

            // Get the Return Values
            int exists = Convert.ToInt32(returnVal);

            if (exists != 0)
            {
                string msg = "Transactions for the same subsidy provider have been posted for this month.";
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }
}