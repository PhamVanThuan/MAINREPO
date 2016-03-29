
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Rules.ApprovalMandateRules;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.Rules.RoleMustHaveMandateToApproveAmount
{
    public class when_checking_approval_for_loss_control_consultant_within_mandate : WithFakes
    {
        private static IThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider;
        private static LossControlConsultantCannotApproveInvoicesGreaterThanR15000Rule thirdPartyInvoiceApprovalRoleAmountRule;
        private static ISystemMessageCollection systemMessages;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static string approverRole;
        private static string roleWithMandate;

        private Establish context = () =>
        {
            approverRole = "Loss Control Consultant";
            roleWithMandate = "Loss Control Consultant";
            thirdPartyInvoiceApprovalMandateProvider = An<IThirdPartyInvoiceApprovalMandateProvider>();
            thirdPartyInvoiceApprovalRoleAmountRule = new LossControlConsultantCannotApproveInvoicesGreaterThanR15000Rule(thirdPartyInvoiceApprovalMandateProvider);
            systemMessages = SystemMessageCollection.Empty();

            invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null,1212, 1, 18212.34M, true),
                new InvoiceLineItemModel(null,1110, 1, 12982.34M, true) };

            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true);
            thirdPartyInvoiceModel.ApproverCurrentUserRole = approverRole;

            thirdPartyInvoiceApprovalMandateProvider.WhenToldTo(y => y.GetMandatedRange(approverRole))
                .Return(new Tuple<decimal, decimal>(14999.99M, 150000.00M));

            thirdPartyInvoiceApprovalMandateProvider.WhenToldTo(y => y.GetMandatedRole(invoiceLineItems.Sum(li => li.TotalAmountIncludingVAT)))
                .Return(roleWithMandate);
        };

        private Because of = () =>
        {
            thirdPartyInvoiceApprovalRoleAmountRule.ExecuteRule(systemMessages, thirdPartyInvoiceModel);
        };

        private It should_get_current_mandate_for_role = () =>
        {
            thirdPartyInvoiceApprovalMandateProvider.WasToldTo(y => y.GetMandatedRange(Param<string>.Matches(m => m.Equals(approverRole, StringComparison.Ordinal))));
        };

        private It should_confirm_role_can_approve = () =>
        {
            systemMessages.AllMessages.Any().ShouldBeFalse();
        };

        private It should_not_get_a_role_with_mandate = () =>
        {
            thirdPartyInvoiceApprovalMandateProvider.WasNotToldTo(y => y.GetMandatedRole(thirdPartyInvoiceModel.LineItems.Sum(li => li.TotalAmountIncludingVAT)));
        };
    }
}