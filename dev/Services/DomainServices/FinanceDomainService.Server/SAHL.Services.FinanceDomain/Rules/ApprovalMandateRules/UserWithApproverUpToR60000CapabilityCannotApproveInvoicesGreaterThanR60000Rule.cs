using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Rules.ApprovalMandateRules
{
    public class UserWithApproverUpToR60000CapabilityCannotApproveInvoicesGreaterThanR60000Rule : BaseApproverMandateChecker, IDomainRule<ThirdPartyInvoiceModel>
    {
        public UserWithApproverUpToR60000CapabilityCannotApproveInvoicesGreaterThanR60000Rule(IThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider)
            : base(thirdPartyInvoiceApprovalMandateProvider)
        {
        }

        public void ExecuteRule(ISystemMessageCollection systemMessages, ThirdPartyInvoiceModel ruleModel)
        {
            if(ruleModel.ApproverCurrentUserCapabilities.Equals(LossControlFeeInvoiceApproverCapability.LOSS_CONTROL_FEE_INVOICE_APPROVER_UP_TO_R60000, System.StringComparison.OrdinalIgnoreCase))
            {
                base.CapabilityMandateLimitCheck(systemMessages, ruleModel);
            }
        }
    }
}