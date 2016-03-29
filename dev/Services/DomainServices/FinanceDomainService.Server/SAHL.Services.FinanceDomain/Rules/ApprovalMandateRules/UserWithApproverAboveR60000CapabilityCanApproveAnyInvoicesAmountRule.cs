using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Rules.ApprovalMandateRules
{
    public class UserWithApproverAboveR60000CapabilityCanApproveAnyInvoicesAmountRule : BaseApproverMandateChecker, IDomainRule<ThirdPartyInvoiceModel>
    {
        public UserWithApproverAboveR60000CapabilityCanApproveAnyInvoicesAmountRule(IThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider)
            : base(thirdPartyInvoiceApprovalMandateProvider)
        {
        }

        public void ExecuteRule(ISystemMessageCollection systemMessages, ThirdPartyInvoiceModel ruleModel)
        {
            if (ruleModel.ApproverCurrentUserCapabilities.Equals(LossControlFeeInvoiceApproverCapability.INVOICE_APPROVER_OVER_R60000
                , System.StringComparison.OrdinalIgnoreCase))
            {
                base.CapabilityMandateLimitCheck(systemMessages, ruleModel);
            }
        }
    }
}