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
    public class when_user_has_invoice_approver_over_R60000_capability : WithFakes
    {
        private static IThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider;
        private static UserWithApproverAboveR60000CapabilityCanApproveAnyInvoicesAmountRule rule;
        private static ISystemMessageCollection systemMessages;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static string approverCapability;

        private Establish context = () =>
        {
            approverCapability = LossControlFeeInvoiceApproverCapability.INVOICE_APPROVER_OVER_R60000;
            thirdPartyInvoiceApprovalMandateProvider = An<IThirdPartyInvoiceApprovalMandateProvider>();
            rule = new UserWithApproverAboveR60000CapabilityCanApproveAnyInvoicesAmountRule(thirdPartyInvoiceApprovalMandateProvider);
            systemMessages = SystemMessageCollection.Empty();

            invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null,1212, 1, 50.00M, true),
                new InvoiceLineItemModel(null,1110, 1, 50.00M, true) };

            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
            thirdPartyInvoiceModel.ApproverCurrentUserCapabilities = approverCapability;

            thirdPartyInvoiceApprovalMandateProvider.WhenToldTo(y => y.GetMandatedRange(approverCapability))
                .Return(new Tuple<decimal, decimal>(60000.01M, decimal.MaxValue));
        };

        private Because of = () =>
        {
            rule.ExecuteRule(systemMessages, thirdPartyInvoiceModel);
        };

        private It should_get_mandate_for_capability = () =>
        {
            thirdPartyInvoiceApprovalMandateProvider.WasToldTo(y => y.GetMandatedRange(Param<string>.Matches(m => m.Equals(approverCapability, StringComparison.Ordinal))));
        };

        private It should_confirm_capability_can_approve = () =>
        {
            systemMessages.AllMessages.Any().ShouldBeFalse();
        };
    }
}