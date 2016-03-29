using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.Capability;
using System;
using System.Collections.Generic;

namespace SAHL.Services.FinanceDomain.Specs.Managers.InvoiceApprovalMandateProvider.GettingRange
{
    public class when_capability_is_loss_controlFee_invoice_approver_up_to_R60000 : WithFakes
    {
        private static IThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider;
        private static Tuple<decimal, decimal> expectedRange;
        private static string providedAuthoriserCapability;
        private static Tuple<decimal, decimal> actualRange;
        private static ICapabilityManager capabilityManager;

        private Establish context = () =>
        {
            providedAuthoriserCapability = LossControlFeeInvoiceApproverCapability.LOSS_CONTROL_FEE_INVOICE_APPROVER_UP_TO_R60000;
            expectedRange = new Tuple<decimal, decimal>(30000.00M, 60000.00M);
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
            actualRange = thirdPartyInvoiceApprovalMandateProvider.GetMandatedRange(providedAuthoriserCapability);
        };

        private It should_return_mandated_range_30K_to_60K = () =>
        {
            (actualRange.Item1 == expectedRange.Item1 && actualRange.Item2 == expectedRange.Item2).ShouldBeTrue();
        };
    }
}