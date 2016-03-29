using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.FinanceDomain.CommandHandlers;
using SAHL.Services.FinanceDomain.Managers;
using SAHL.Services.FinanceDomain.Managers.ThirdPartyInvoiceData;
using SAHL.Services.Interfaces.FinanceDomain.Commands;
using SAHL.Services.Interfaces.FinanceDomain.Model;
using System.Linq;

namespace SAHL.Services.FinanceDomain.Specs.CommandHandlersSpec.ApproveThirdPartyInvoice
{
    public class when_user_does_not_have_invoice_approver_capabilities : WithFakes
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
        private static int thirdPartyInvoiceKey;
        private static IThirdPartyInvoiceManager manager;
        private static string CapabilityWithHigherMandate;
        private static string[] currentUserAuthenticatedCapabilities;
        private static SystemMessage errorMsg;
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
            var user = "SAHL\busisaniG";
            errorMsg = new SystemMessage(string.Format("{0} has no invoice approving capabilities", user), SystemMessageSeverityEnum.Error);
            systemMessages = SystemMessageCollection.Empty();

            domainRuleManager = An<IDomainRuleManager<ThirdPartyInvoiceModel>>();

            currentUserAuthenticatedCapabilities = "Invoice Processor,Invoice Approver,Loss Control Fee Consultant".Split(',');
            serviceRequestMetadata.WhenToldTo(y => y.CurrentUserCapabilities).Return(currentUserAuthenticatedCapabilities);
            serviceRequestMetadata.WhenToldTo(y => y.UserName).Return(user);
            thirdPartyInvoiceKey = 1212;

            approveInvoiceCommand = new ApproveThirdPartyInvoiceCommand(thirdPartyInvoiceKey);
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

            CapabilityWithHigherMandate = string.Empty;
            thirdPartyInvoiceApprovalMandateProvider.WhenToldTo(x => x.GetCapabilityWithHigherMandate(currentUserAuthenticatedCapabilities)).Return(CapabilityWithHigherMandate);
        };

        private Because of = () =>
        {
            systemMessages = approveInvoiceCommandHandler.HandleCommand(approveInvoiceCommand, serviceRequestMetadata);
        };

        private It should_get_capability_with_higher_mandate = () =>
        {
            thirdPartyInvoiceApprovalMandateProvider.WasToldTo(x => x.GetCapabilityWithHigherMandate(currentUserAuthenticatedCapabilities));
        };

        private It should_not_check_if_the_capabilities_and_user_org_structure_key_are_related = () =>
        {
            metadataDomainRuleManager.WasNotToldTo(x => x.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<ServiceRequestMetadata>()));
        };

        private It should_not_run_business_rules = () =>
        {
            domainRuleManager.WasNotToldTo(y =>
                y.ExecuteRules(Param.IsAny<ISystemMessageCollection>(), Param.IsAny<ThirdPartyInvoiceModel>()));
        };

        private It should_return_user_not_mandated_error_message = () =>
        {
            systemMessages.ErrorMessages().Any(x => x.Message.Equals(errorMsg.Message)).ShouldBeTrue();
        };

    }
}