using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Rules.FinancialService
{
    [RuleDBTag("FinancialServiceBankAccountDebitOrderDayAllowedValues",
    "Debit Order Day must be sourced from the DebitOrderDay table",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.FinancialService.FinancialServiceBankAccountDebitOrderDayAllowedValues")]
    [RuleInfo]
    public class FinancialServiceBankAccountDebitOrderDayAllowedValues : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IFinancialServiceBankAccount fsb = Parameters[0] as IFinancialServiceBankAccount;
            IApplicationDebitOrder ado = Parameters[0] as IApplicationDebitOrder;

            if (fsb == null && ado == null)
                throw new Exception("FinancialServiceBankAccountDebitOrderDayAllowedValues rule expects either an IFinancialServiceBankAccount or an IApplicationDebitOrder object");

            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            StringBuilder sb = new StringBuilder();

            foreach (IDebitOrderDay debitOrderDay in lookupRepo.DebitOrderDays.ObjectDictionary.Values)
            {
                if ((fsb != null && fsb.DebitOrderDay == debitOrderDay.Day) || 
                    (ado != null && ado.DebitOrderDay == debitOrderDay.Day))
                    return 1;

                if (sb.Length > 0)
                    sb.Append(",");
                sb.Append(debitOrderDay.Day);
            }

            string msg = String.Format("Debit Order Day is a mandatory field and must be one of the following values: [{0}]", sb.ToString());
            AddMessage(msg, msg, Messages);
            return 0;
        }
    }

}
