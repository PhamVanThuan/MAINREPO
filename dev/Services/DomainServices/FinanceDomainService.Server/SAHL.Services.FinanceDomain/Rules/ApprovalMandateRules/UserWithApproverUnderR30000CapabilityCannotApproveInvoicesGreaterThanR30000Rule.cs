using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Rules.ApprovalMandateRules
{
    public class UserWithApproverUnderR30000CapabilityCannotApproveInvoicesGreaterThanR30000Rule : BaseApproverMandateChecker, IDomainRule<ThirdPartyInvoiceModel>
    {
        public UserWithApproverUnderR30000CapabilityCannotApproveInvoicesGreaterThanR30000Rule(IThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider)
            : base(thirdPartyInvoiceApprovalMandateProvider)
        {
        }

        public void ExecuteRule(ISystemMessageCollection systemMessages, ThirdPartyInvoiceModel ruleModel)
        {
            if (ruleModel.ApproverCurrentUserCapabilities.Equals(LossControlFeeInvoiceApproverCapability.Loss_Control_Fee_Invoice_Approver_Under_R30000, System.StringComparison.OrdinalIgnoreCase))
            {
                base.CapabilityMandateLimitCheck(systemMessages, ruleModel);
            }
        }
    }
}