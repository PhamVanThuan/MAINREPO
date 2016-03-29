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
    public class when_one_of_capabilies_is_not_linked_user_org_structure_key : WithFakes
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
        private static int ThirdPartyInvoiceKey;
        private static IThirdPartyInvoiceManager manager;
        public static string[] currentUserAuthenticatedCapabilities;
        public static string CapabilityWithHigherMandate;
        private static IDomainRuleManager<ServiceRequestMetadata> metadataDomainRuleManager;
        private Establish context = () =>
        {
            metadataDomainRuleManager = An<IDomainRuleManager<ServiceRequestMetadata>>();
            serviceRequestMetadata = An<IServiceRequestMetadata>();
            thirdPartyInvoiceDataManager = An<IThirdPartyInvoiceDataManager>();
            eventRaiser = An<IEventRaiser>();
            manager = An<IThirdPartyInvoiceManager>();
            serviceCommandRouter = An<IServiceCommandRouter>();
            thirdPartyInvoiceApprovalMandateProvider = An<IThirdPartyInvoiceApprovalMandateProvider>();

            systemMessages = SystemMessageCollection.Empty();

            var metaDataDictionary = new Dictionary<string, string>();
            metaDataDictionary.Add(ServiceRequestMetadata.HEADER_USERORGANISATIONSTRUCTUREKEY, "1001");
            metaDataDictionary.Add(ServiceRequestMetadata.HEADER_CURRENTUSERCAPABILITIES, "Invoice Processor,Invoice Approver,Loss Control Fee Consultant,Loss Control Fee Invoice Approver Under R15000");
            serviceRequestMetadata = new ServiceRequestMetadata(metaDataDictionary);
            currentUserAuthenticatedCapabilities = serviceRequestMetadata.CurrentUserCapabilities;

            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();
            
            ThirdPartyInvoiceKey = 1212;

            approveInvoiceCommand = new ApproveThirdPartyInvoiceCommand(ThirdPartyInvoiceKey);
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
            thirdPartyInvoiceApprovalMandateProvider.WhenToldTo(x => x.GetCapabilityWithHigherMandate(Param<string[]>.Matches(y => y.All(currentUserAuthenticatedCapabilities.Contains)))).Return("Loss Control Fee Invoice Approver Under R15000");
            metadataDomainRuleManager.WhenToldTo(y => y.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param<ServiceRequestMetadata>
                .Matches(z => z.UserOrganisationStructureKey == serviceRequestMetadata.UserOrganisationStructureKey
                && z.CurrentUserCapabilities.All(serviceRequestMetadata.CurrentUserCapabilities.Contains))))
                .Callback<ISystemMessageCollection>
                (
                    y =>
                    {
                        systemMessages = y;
                        var errorMsg = "One or more capabilities are not linked to user";
                        y.AddMessage(new SystemMessage(errorMsg, SystemMessageSeverityEnum.Error));
                    }
                );
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

        private It should_return_capability_not_linked_error_message = () =>
        {
            systemMessages.ErrorMessages().ShouldContain(m => m.Message.Equals("One or more capabilities are not linked to user"));
        };

        private It should_not_check_for_validity_of_the_invoice = () =>
        {
            domainRuleManager.WasNotToldTo(drm => drm.ExecuteRules
            (
                  Param.IsAny<ISystemMessageCollection>()
                , Param.IsAny<ThirdPartyInvoiceModel>()

            ));
        };

        private It should_not_check_that_approver_has_correct_credentials_to_approve_amount = () =>
        {
            domainRuleManager.WasNotToldTo(drm => drm.ExecuteRules
            (
                  Param.IsAny<ISystemMessageCollection>()
                , Param.IsAny<ThirdPartyInvoiceModel>()

            ));
        };

        private It should_not_approve_invoice = () =>
        {
            thirdPartyInvoiceDataManager.WasNotToldTo(y => y.UpdateThirdPartyInvoiceStatus
            (
                    Param<int>.Matches(m => m == ThirdPartyInvoiceKey)
                , Param.IsAny<InvoiceStatus>()
            ));
        };

    }
}