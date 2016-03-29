using SAHL.Services.FinanceDomain.Managers.Capability;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Managers
{
    public class ThirdPartyInvoiceApprovalMandateProvider : IThirdPartyInvoiceApprovalMandateProvider
    {
        private List<ApprovalMandateRanges> approvalRanges;
        private ICapabilityManager capabilityManager;

        public ThirdPartyInvoiceApprovalMandateProvider(ICapabilityManager capabilityManager)
        {
            this.capabilityManager = capabilityManager;
            approvalRanges = this.capabilityManager.GetCapabilityMandates().ToList<ApprovalMandateRanges>();
        }

        public List<string> GetMandatedCapability(decimal invoiceAmount)
        {
            List<string> MandatedCapabilities = new List<string>();
            foreach (var approvalRange in approvalRanges)
            {
                if (TestRange(approvalRange.UpperBound, invoiceAmount))
                {
                    MandatedCapabilities.Add(approvalRange.Capability);
                }
            }
            return MandatedCapabilities;
        }

        Tuple<decimal, decimal> IThirdPartyInvoiceApprovalMandateProvider.GetMandatedRange(string capability)
        {
            var result = new Tuple<decimal, decimal>(0.0M, 0.0M);
            var foundApprovalRange = approvalRanges.FirstOrDefault(y => y.Capability.Equals(capability, StringComparison.OrdinalIgnoreCase));
            if (foundApprovalRange != null)
            {
                result = new Tuple<decimal, decimal>(foundApprovalRange.LowerBound, foundApprovalRange.UpperBound);
            }
            return result;
        }

        private bool TestRange(decimal upperBound, decimal invoiceAmountToApprove)
        {
            return (invoiceAmountToApprove <= upperBound);
        }

        public string GetCapabilityWithHigherMandate(string[] approverUserCapabilities)
        {
            if (approverUserCapabilities == null)
            {
                return string.Empty;
            }
            if (approverUserCapabilities.Contains(LossControlFeeInvoiceApproverCapability.INVOICE_APPROVER_OVER_R60000))
            {
                return LossControlFeeInvoiceApproverCapability.INVOICE_APPROVER_OVER_R60000;
            }
            if (approverUserCapabilities.Contains(LossControlFeeInvoiceApproverCapability.LOSS_CONTROL_FEE_INVOICE_APPROVER_UP_TO_R60000))
            {
                return LossControlFeeInvoiceApproverCapability.LOSS_CONTROL_FEE_INVOICE_APPROVER_UP_TO_R60000;
            }
            if (approverUserCapabilities.Contains(LossControlFeeInvoiceApproverCapability.Loss_Control_Fee_Invoice_Approver_Under_R30000))
            {
                return LossControlFeeInvoiceApproverCapability.Loss_Control_Fee_Invoice_Approver_Under_R30000;
            }
            if (approverUserCapabilities.Contains(LossControlFeeInvoiceApproverCapability.LOSS_CONTROL_FEE_INVOICE_APPROVER_UNDER_R15000))
            {
                return LossControlFeeInvoiceApproverCapability.LOSS_CONTROL_FEE_INVOICE_APPROVER_UNDER_R15000;
            }
            return string.Empty;
        }
    }
}