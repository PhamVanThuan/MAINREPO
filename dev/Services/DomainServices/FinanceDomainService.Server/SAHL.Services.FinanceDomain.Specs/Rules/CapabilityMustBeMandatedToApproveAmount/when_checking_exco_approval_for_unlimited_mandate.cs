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
    public class when_checking_exco_approval_for_unlimited_mandate : WithFakes
    {
        private static ThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider;
        private static ExcoMemberCanApproveAnyInvoiceValueRule thirdPartyInvoiceApprovalRoleAmountRule;
        private static ISystemMessageCollection systemMessages;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static string approverRole;

        private Establish context = () =>
        {
            approverRole = "Exco Member";
            thirdPartyInvoiceApprovalMandateProvider = new ThirdPartyInvoiceApprovalMandateProvider();
            thirdPartyInvoiceApprovalRoleAmountRule = new ExcoMemberCanApproveAnyInvoiceValueRule(thirdPartyInvoiceApprovalMandateProvider);
            systemMessages = SystemMessageCollection.Empty();

            invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null,1212, 1, 60000.00M, true),
                new InvoiceLineItemModel(null,1110, 1, 1950000M, true) };

            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true);
            thirdPartyInvoiceModel.ApproverCurrentUserRole = approverRole;
        };

        private Because of = () =>
        {
            thirdPartyInvoiceApprovalRoleAmountRule.ExecuteRule(systemMessages, thirdPartyInvoiceModel);
        };

        private It should_confirm_role_can_approve = () =>
        {
            systemMessages.AllMessages.Any().ShouldBeFalse();
        };
    }
}