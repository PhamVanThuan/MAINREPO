using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class ApplicationDebitOrder : BusinessModelBase<ApplicationDebitOrder_DAO>, IApplicationDebitOrder
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("ApplicationDebitOrderPaymentBankAccountSelection");
            Rules.Add("ApplicationDebitOrderPaymentBankAccountNonDO");
            Rules.Add("ApplicationDebitOrderPaymentTypeSubsidy");
            Rules.Add("FinancialServiceBankAccountDebitOrderDayAllowedValues");
        }
    }
}