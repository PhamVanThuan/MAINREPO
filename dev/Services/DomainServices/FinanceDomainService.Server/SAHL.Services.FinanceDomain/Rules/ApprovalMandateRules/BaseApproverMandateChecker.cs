using System;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.Interfaces.FinanceDomain.Model;

namespace SAHL.Services.FinanceDomain.Rules.ApprovalMandateRules
{
    public abstract class BaseApproverMandateChecker
    {
        private IThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider;

        public void CapabilityMandateLimitCheck(ISystemMessageCollection systemMessages, ThirdPartyInvoiceModel ruleModel)
        {
            var capabilityMandateRange = thirdPartyInvoiceApprovalMandateProvider.GetMandatedRange(ruleModel.ApproverCurrentUserCapabilities);
            if (ruleModel.TotalAmountIncludingVAT > capabilityMandateRange.Item2)
            {
                systemMessages.AddMessage(
                    new SystemMessage(
                        string.Format("{0} cannot approve invoice amount greater than R{1}.", ruleModel.ApproverCurrentUserCapabilities
                        , Math.Round(capabilityMandateRange.Item2, 2))
                        , SystemMessageSeverityEnum.Error
                ));
            }
        }

        public BaseApproverMandateChecker(IThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider)
        {
            this.thirdPartyInvoiceApprovalMandateProvider = thirdPartyInvoiceApprovalMandateProvider;
        }
    }
}