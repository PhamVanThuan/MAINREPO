using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.Capability;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Managers.InvoiceApprovalMandateProvider
{
    public class when_invoice_amount_under_R15K : WithFakes
    {
        private static IThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider;
        private static List<string> sandatedCapabilities;
        private static decimal invoiceAmount;
        private static ICapabilityManager capabilityManager;

        private Establish context = () =>
        {
            invoiceAmount = 14000.01M;
            capabilityManager = An<ICapabilityManager>();
            var approvalRanges = new List<ApprovalMandateRanges>();
            var lossControlFeeInvoiceApproverUnderR15000 = new ApprovalMandateRanges() { LowerBound = 0.00M, UpperBound = 14999.99M, Capability = LossControlFeeInvoiceApproverCapability.LOSS_CONTROL_FEE_INVOICE_APPROVER_UNDER_R15000 };
            approvalRanges.Add(lossControlFeeInvoiceApproverUnderR15000);
            var lossControlFeeInvoiceApproverUnderR30000 = new ApprovalMandateRanges() { LowerBound = 15000.00M, UpperBound = 29999.99M, Capability = LossControlFeeInvoiceApproverCapability.Loss_Control_Fee_Invoice_Approver_Under_R30000 };
            approvalRanges.Add(lossControlFeeInvoiceApproverUnderR30000);
            var lossControlFeeInvoiceApproverUptoR60000 = new ApprovalMandateRanges() { LowerBound = 30000.00M, UpperBound = 60000.00M, Capability = LossControlFeeInvoiceApproverCapability.LOSS_CONTROL_FEE_INVOICE_APPROVER_UP_TO_R60000 };
            approvalRanges.Add(lossControlFeeInvoiceApproverUptoR60000);
            var InvoiceApproverOverR60000 = new ApprovalMandateRanges() { LowerBound = 60000.00M, UpperBound = Decimal.MaxValue, Capability = LossControlFeeInvoiceApproverCapability.INVOICE_APPROVER_OVER_R60000 };
            approvalRanges.Add(InvoiceApproverOverR60000);

            capabilityManager.WhenToldTo(x => x.GetCapabilityMandates()).Return(approvalRanges);
            thirdPartyInvoiceApprovalMandateProvider = new ThirdPartyInvoiceApprovalMandateProvider(capabilityManager);

        };

        private Because of = () =>
        {
            sandatedCapabilities = thirdPartyInvoiceApprovalMandateProvider.GetMandatedCapability(invoiceAmount);
        };

        private It should_return_Loss_control_fee_invoice_approver_under_R15000 = () =>
        {
            sandatedCapabilities.Any(x => x.Equals(LossControlFeeInvoiceApproverCapability.LOSS_CONTROL_FEE_INVOICE_APPROVER_UNDER_R15000, StringComparison.Ordinal)).ShouldBeTrue();
        };

        private It should_return_Loss_control_fee_invoice_approver_under_R30000 = () =>
        {
            sandatedCapabilities.Any(x => x.Equals(LossControlFeeInvoiceApproverCapability.Loss_Control_Fee_Invoice_Approver_Under_R30000, StringComparison.Ordinal)).ShouldBeTrue();
        };

        private It should_return_should_return_Loss_control_fee_invoice_approver_up_to_R60000 = () =>
        {
            sandatedCapabilities.Any(x => x.Equals(LossControlFeeInvoiceApproverCapability.LOSS_CONTROL_FEE_INVOICE_APPROVER_UP_TO_R60000, StringComparison.Ordinal)).ShouldBeTrue();
        };

        private It should_return_Invoice_approver_over_R60000 = () =>
        {
            sandatedCapabilities.Any(x => x.Equals(LossControlFeeInvoiceApproverCapability.INVOICE_APPROVER_OVER_R60000, StringComparison.Ordinal)).ShouldBeTrue();
        };
    }
}