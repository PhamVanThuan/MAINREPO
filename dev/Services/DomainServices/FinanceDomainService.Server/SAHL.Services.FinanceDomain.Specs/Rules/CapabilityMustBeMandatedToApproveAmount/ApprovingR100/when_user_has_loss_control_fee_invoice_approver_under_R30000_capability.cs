using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Rules.ApprovalMandateRules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules.CapabilityMustBeMandatedToApproveAmount.ApprovingR100
{
    public class when_user_has_loss_control_fee_invoice_approver_under_R30000_capability : WithFakes
    {
        private static IThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider;
        private static UserWithApproverUnderR30000CapabilityCannotApproveInvoicesGreaterThanR30000Rule thirdPartyInvoiceApprovalRoleAmountRule;
        private static ISystemMessageCollection systemMessages;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static string approverCapability;

        private Establish context = () =>
        {
            approverCapability = LossControlFeeInvoiceApproverCapability.Loss_Control_Fee_Invoice_Approver_Under_R30000;
            thirdPartyInvoiceApprovalMandateProvider = An<IThirdPartyInvoiceApprovalMandateProvider>();
            thirdPartyInvoiceApprovalRoleAmountRule = new UserWithApproverUnderR30000CapabilityCannotApproveInvoicesGreaterThanR30000Rule(thirdPartyInvoiceApprovalMandateProvider);
            systemMessages = SystemMessageCollection.Empty();

            invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null,1212, 1, 50.00M, true),
                new InvoiceLineItemModel(null,1110, 1, 50.00M, true) };

            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
            thirdPartyInvoiceModel.ApproverCurrentUserCapabilities = approverCapability;

            thirdPartyInvoiceApprovalMandateProvider.WhenToldTo(y => y.GetMandatedRange(approverCapability))
                .Return(new Tuple<decimal, decimal>(15000.00M, 29999.99M));
        };

        private Because of = () =>
        {
            thirdPartyInvoiceApprovalRoleAmountRule.ExecuteRule(systemMessages, thirdPartyInvoiceModel);
        };

        private It should_get_current_mandate_for_role = () =>
        {
            thirdPartyInvoiceApprovalMandateProvider.WasToldTo(y => y.GetMandatedRange(Param<string>.Matches(m => m.Equals(approverCapability, StringComparison.Ordinal))));
        };

        private It should_confirm_role_can_approve = () =>
        {
            systemMessages.AllMessages.Any().ShouldBeFalse();
        };
    }
}