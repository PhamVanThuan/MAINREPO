using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.BulkBatch
{
    [RuleDBTag("BulkBatchParameterArrearBalanceMultiple",
       "ArrearBalance BulkBatchParameter values must be a multiple of 500",
       "SAHL.Rules.DLL",
       "SAHL.Common.BusinessModel.Rules.BulkBatch.BulkBatchParameterArrearBalanceMultiple")]
    [RuleInfo]
    [RuleParameterTag(new string[] { "@Multiple,500,9" })]
    public class BulkBatchParameterArrearBalanceMultiple : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IBulkBatchParameter bulkBatchParm = (IBulkBatchParameter)Parameters[0];
            int multiple = Convert.ToInt32(RuleItem.RuleParameters[0].Value);

            // process only if the type is correct and there is a value entered
            if (bulkBatchParm.ParameterName != BulkBatchParameterNames.ArrearBalance.ToString() || 
                String.IsNullOrEmpty(bulkBatchParm.ParameterValue))
                return 1;

            double parmValue = Convert.ToDouble(bulkBatchParm.ParameterValue);
            if ((parmValue % multiple) != 0)
            {
                string msg = String.Format("Loan Arrear Balance must be a multiple of {0}", multiple.ToString());
                AddMessage(msg, msg, Messages);
                return 0;
            }

            return 1;
        }
    }
}
