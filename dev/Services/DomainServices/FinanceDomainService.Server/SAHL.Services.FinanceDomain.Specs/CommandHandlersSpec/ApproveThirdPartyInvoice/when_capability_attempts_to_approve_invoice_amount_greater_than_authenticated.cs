using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ApproveThirdPartyInvoice
{
    public class when_capability_attempts_to_approve_invoice_amount_greater_than_authenticated : WithFakes
    {
        private static IServiceRequestMetadata serviceRequestMetadata;
        private static IThirdPartyInvoiceDataManager thirdPartyInvoiceDataManager;
        private static IEventRaiser eventRaiser;
        private static IServiceCommandRouter serviceCommandRouter;
        private static IThirdPartyInvoiceApprovalMandateProvider thirdPartyInvoiceApprovalMandateProvider;
        private static IDomainRuleManager<ThirdPartyInvoiceModel> domainRuleManager;
        private static ApproveThirdPartyInvoiceCommand approveInvoiceCommand;
        private static ApproveThirdPartyInvoiceCommandHandler approveInvoiceCommandHandler;
        private static ISystemMessageCollection systemMessages;
        private static ThirdPartyInvoiceModel thirdPartyInvoiceModel;
        private static IEnumerable<InvoiceLineItemModel> invoiceLineItems;
        private static decimal invoiceTotal;
        private static IThirdPartyInvoiceManager manager;
        public static string[] currentUserAuthenticatedCapabilities;
        public static string CapabilityWithHigherMandate;
        private static IDomainRuleManager<ServiceRequestMetadata> metadataDomainRuleManager;
        private Establish context = () =>
        {
            metadataDomainRuleManager = An<IDomainRuleManager<ServiceRequestMetadata>>();
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            eventRaiser = An<IEventRaiser>();
            serviceCommandRouter = An<IServiceCommandRouter>();
            thirdPartyInvoiceApprovalMandateProvider = An<IThirdPartyInvoiceApprovalMandateProvider>();
            manager = An<IThirdPartyInvoiceManager>();
            systemMessages = SystemMessageCollection.Empty();


            var metaDataDictionary = new Dictionary<string, string>();
            metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERORGANISATIONSTRUCTUREKEY, "1001");
            metaDataDictionary.Add(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES, "Invoice Processor,Invoice Approver,Loss Control Fee Consultant,Loss Control Fee Invoice Approver Under R15000");
            serviceRequestMetadata = new ServiceRequestMetadata(metaDataDictionary);
            currentUserAuthenticatedCapabilities = serviceRequestMetadata.CurrentUserCapabilities;



            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();

            invoiceLineItems = new List<InvoiceLineItemModel> { new InvoiceLineItemModel(null,1212, 1, 21212.34M, true),
                new InvoiceLineItemModel(null,1110, 1, 21212.34M, true) };

            thirdPartyInvoiceModel = new ThirdPartyInvoiceModel(1212, Guid.NewGuid(), "DD0011", DateTime.Now, invoiceLineItems, true, string.Empty);
            invoiceTotal = (invoiceLineItems.Sum(x => x.AmountExcludingVAT) * 114) / 100;
            approveInvoiceCommand = new ApproveThirdPartyInvoiceCommand(thirdPartyInvoiceModel.ThirdPartyInvoiceKey);
            approveInvoiceCommandHandler = new ApproveThirdPartyInvoiceCommandHandler
                                            (
                                                  thirdPartyInvoiceDataManager
                                                , eventRaiser
                                                , serviceCommandRouter
                                                , thirdPartyInvoiceApprovalMandateProvider
                                                , domainRuleManager
                                                , metadataDomainRuleManager
                                                , manager
                                            );

            manager.WhenToldTo(y => y.GetThirdPartyInvoiceModel(Param.IsAny<int>())).Return(thirdPartyInvoiceModel);

            domainRuleManager.WhenToldTo(y => y.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<ThirdPartyInvoiceModel>()))
                .Callback<ISystemMessageCollection>
                (
                    y =>
                    {
                        systemMessages = y;
                        var errorMsg = string.Format("Loss Control Fee Invoice Approver Under R15000 cannot approve invoice amount greater than R{0}.", thirdPartyInvoiceModel.TotalAmountIncludingVAT);
                        y.AddMessage(new SystemMessage(errorMsg, SystemMessageSeverityEnum.Error));
                    }
                );
            CapabilityWithHigherMandate = LossControlFeeInvoiceApproverCapability.LOSS_CONTROL_FEE_INVOICE_APPROVER_UNDER_R15000;
            thirdPartyInvoiceApprovalMandateProvider.WhenToldTo(x =>
                x.GetCapabilityWithHigherMandate(Param<string[]>.Matches(y => y.All(currentUserAuthenticatedCapabilities.Contains)))).Return(CapabilityWithHigherMandate);
        };

        private Because of = () =>
        {
            systemMessages = approveInvoiceCommandHandler.HandleCommand(approveInvoiceCommand, serviceRequestMetadata);
        };

        private It should_get_capability_with_higher_mandate = () =>
        {
            thirdPartyInvoiceApprovalMandateProvider.WasToldTo(x => x.GetCapabilityWithHigherMandate(Param<string[]>.Matches(y => y.All(currentUserAuthenticatedCapabilities.Contains))));
        };

        private It should_check_if_the_capabilities_and_user_org_structure_key_are_related = () =>
        {
            metadataDomainRuleManager.WasToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<ServiceRequestMetadata>()));
        };

        private It should_check_for_validity_of_the_invoice = () =>
        {
            domainRuleManager.WasToldTo(drm => drm.ExecuteRules
            (
                  Param.IsAny<ISystemMessageCollection>()
                , Param<ThirdPartyInvoiceModel>
                    .Matches
                    (
                       inv => inv.LineItems.Any()
                           && inv.ThirdPartyId != null && inv.ThirdPartyId != Guid.Empty
                    )
            ));
        };

        private It should_check_that_approver_has_correct_credentials_to_approve_amount = () =>
        {
            domainRuleManager.WasToldTo(drm => drm.ExecuteRules
            (
                  Param.IsAny<ISystemMessageCollection>()
                , Param<ThirdPartyInvoiceModel>
                    .Matches
                    (
                       inv => inv.LineItems.Sum(li => li.TotalAmountIncludingVAT) == invoiceTotal
                           && inv.ApproverCurrentUserCapabilities == CapabilityWithHigherMandate
                    )

            ));
        };

        private It should_fail_business_rules_checks = () =>
        {
            systemMessages.HasErrors.ShouldBeTrue();
        };

        private It should_not_approve = () =>
        {
            thirdPartyInvoiceDataManager.WasNotToldTo
            (
                y => y.UpdateThirdPartyInvoiceStatus
                       (
                          Param<int>.Matches(m => m == thirdPartyInvoiceModel.ThirdPartyInvoiceKey)
                         , Param<InvoiceStatus>.Matches(m => m == InvoiceStatus.Approved)
                       )

            );
        };

        private It should_include_correct_role_with_mandate_to_approve_amount_in_error_message = () =>
        {
            systemMessages.ErrorMessages().ShouldContain(m => m.Message.Contains(thirdPartyInvoiceModel.TotalAmountIncludingVAT.ToString()));
        };
    }
}