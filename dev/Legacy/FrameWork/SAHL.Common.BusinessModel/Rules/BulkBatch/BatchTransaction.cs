using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.BulkBatch
{

    [RuleDBTag("BatchTransactionTransactionTypeMandatory",
       "Ensures the Transaction Type is captured",
       "SAHL.Rules.DLL",
      "SAHL.Common.BusinessModel.Rules.BulkBatch.BatchTransactionTransactionTypeMandatory")]
    [RuleInfo]
    public class BatchTransactionTransactionTypeMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IBatchTransaction batchTransaction = (IBatchTransaction)Parameters[0];

            if (batchTransaction.TransactionTypeNumber <= 0)
            {
                string msg = "Transaction Type is a mandatory field";
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("BatchTransactionAmountMandatory",
       "Ensures the Transaction Amount is captured",
       "SAHL.Rules.DLL",
     "SAHL.Common.BusinessModel.Rules.BulkBatch.BatchTransactionAmountMandatory")]
    [RuleInfo]
    public class BatchTransactionAmountMandatory : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IBatchTransaction batchTransaction = (IBatchTransaction)Parameters[0];

            // if status is error or deleted, we can ignore this rule
            if (batchTransaction.BatchTransactionStatus != null && 
                (batchTransaction.BatchTransactionStatus.Key == (int)BatchTransactionStatuses.Error || 
                (batchTransaction.BatchTransactionStatus.Key == (int)BatchTransactionStatuses.Deleted)))
            {
                return 1;
            }

            if (batchTransaction.Amount <= 0)
            {
                string msg = "Transaction Amount is a mandatory field";
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }

    [RuleDBTag("BatchTransactionAmountMaximum",
           "Transaction Amount maximum value",
           "SAHL.Rules.DLL",
        "SAHL.Common.BusinessModel.Rules.BulkBatch.BatchTransactionAmountMaximum")]
    [RuleInfo]
    [RuleParameterTag(new string[] { "@BatchTransactionAmountMaximum,10000000,7" })]
    public class BatchTransactionAmountMaximum : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IBatchTransaction batchTransaction = (IBatchTransaction)Parameters[0];
            double maxValue = Convert.ToDouble(RuleItem.RuleParameters[0].Value);

            if (batchTransaction.Amount > maxValue)
            {
                string msg = String.Format("Transaction Amount cannot be greater than R{0}", maxValue.ToString(SAHL.Common.Constants.CurrencyFormat));
                AddMessage(msg, msg, Messages);
                return 0;
            }
            return 1;
        }
    }
}
